using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Google.Protobuf;
using Oracle.BitStream;
using Oracle.BitStream.Utilities;
using Oracle.Events;
using Oracle.Packets.Entry;
using Oracle.Packets.Fields;
using Oracle.Packets.Model;
using Proto.Dota;
using Snappy;

namespace Oracle.Packets
{
    public class DemPacket
    {
        private Dictionary<Predicate<IMessage>, Action<IMessage>> MappedMessages;
        private S2FieldReader fieldReader;

        private Entity[] Entities;
        private int[] Deletions;
        private Dictionary<int, BaselineEntry> BaselineEntries = new Dictionary<int, BaselineEntry>();
        
        private Dictionary<int, GameEventEntry> GameEventEntryId = new Dictionary<int, GameEventEntry>();
        private Dictionary<string, GameEventEntry> GameEventEntryName = new Dictionary<string, GameEventEntry>();

        public StringTablesModel StringTables;
        private static int MaxNameLength = 0x400;
        private static int KeyHistorySize = 32;
        private HashSet<string> RequestedTables = new HashSet<string>();
        private HashSet<string> UpdatedTables = new HashSet<string>();
        private Dictionary<string, CDemoStringTables.Types.table_t> ResetStringTables = new Dictionary<string, CDemoStringTables.Types.table_t>();
        private byte[] tempBuf = new byte[0x4000];
        private int numTables = 0;
        private ReplayContext _ctx;
        public DemPacket(ReplayContext ctx)
        {
            _ctx = ctx;
            StringTables = new StringTablesModel();
            fieldReader = new S2FieldReader();
            Entities = new Entity[1 << 17];

            RequestedTables.Add("CombatLogNames");
            RequestedTables.Add("EntityNames");
            RequestedTables.Add("instancebaseline");
            RequestedTables.Add("ActiveModifiers");

            MappedMessages = new Dictionary<Predicate<IMessage>, Action<IMessage>>
            {
                [m => m is CSVCMsg_ServerInfo] = ParseBuild,
                [m => m is CSVCMsg_ClassInfo] = ParseClassInfoMessage,
                [m => m is CSVCMsg_CreateStringTable] = OnCreateStringTable,
                //[m => m is CSVCMsg_PacketEntities] = ParseEntity,
                [m => m is CMsgDOTACombatLogEntry] = ParseCombatLog,
                [m => m is CSVCMsg_GameEventList] = ParseEventList,
                [m => m is CSVCMsg_GameEvent] = ParseGameEvent,
                [m => m is CUserMessageSayText2] = ParseSayText2,
                [m => m is CUserMessageTextMsg] = ParseTextMsg,
                [m => m is CUserMessageSendAudio] = ParseSendAudio,
                [m => m is CDOTAUserMsg_ParticleManager] = ParseParticleManager,
                [m => m is CDOTAUserMsg_ChatEvent] = ParseChatEvent,
                [m => m is CDOTAUserMsg_LocationPing] = ParseLocationPing,
                [m => m is CDOTAUserMsg_ItemPurchased] = ParseItemPurchased,
                [m => m is CDOTAUserMsg_ChatWheel] = ParseChatWheel,
                [m => m is CMsgDOTAMatch] = ParseDotaMatch,
                [m => m is CDOTAUserMsg_TipAlert] = ParseTipAlert,
                [m => m is CDOTAUserMessage_TeamCaptainChanged] = ParseCapChanged

            };
        }

        private void ParseTipAlert(IMessage tipAlert)
        {
            var message = (CDOTAUserMsg_TipAlert)tipAlert;
            _ctx.Observer.Raise(typeof(CDOTAUserMsg_TipAlert), new ObjectArgs(message));
        }

        private void ParseCapChanged(IMessage capChanged)
        {
            var message = (CDOTAUserMessage_TeamCaptainChanged)capChanged;
            _ctx.Observer.Raise(typeof(CDOTAUserMessage_TeamCaptainChanged), new ObjectArgs(message));
        }

        private void ParseDotaMatch(IMessage dotaMatch)
        {
            var message = (CMsgDOTAMatch)dotaMatch;
            _ctx.Observer.Raise(typeof(CMsgDOTAMatch), new ObjectArgs(message));
        }

        private void ParseChatWheel(IMessage chatWheel)
        {
            var message = (CDOTAUserMsg_ChatWheel)chatWheel;
            _ctx.Observer.Raise(typeof(CDOTAUserMsg_ChatWheel), new ObjectArgs(message));
        }

        private void ParseItemPurchased(IMessage itemPurchased)
        {
            var message = (CDOTAUserMsg_ItemPurchased)itemPurchased;
            _ctx.Observer.Raise(typeof(CDOTAUserMsg_ItemPurchased), new ObjectArgs (message));
        }

        private void ParseLocationPing(IMessage locPing)
        {
            var message = (CDOTAUserMsg_LocationPing)locPing;
            _ctx.Observer.Raise(typeof(CDOTAUserMsg_LocationPing), new ObjectArgs(message));
        }

        private void ParseChatEvent(IMessage chatEvent)
        {
            var message = (CDOTAUserMsg_ChatEvent)chatEvent;
            _ctx.Observer.Raise(typeof(CDOTAUserMsg_ChatEvent), new ObjectArgs(message));
        }

        private void ParseParticleManager(IMessage particleManager)
        {
            var message = (CDOTAUserMsg_ParticleManager)particleManager;
            _ctx.Observer.Raise(typeof(CDOTAUserMsg_ParticleManager), new ObjectArgs(message));
        }

        private void ParseSendAudio(IMessage sendAudio)
        {
            var message = (CUserMessageSendAudio)sendAudio;
            _ctx.Observer.Raise(typeof(CUserMessageSendAudio), new ObjectArgs(message));
        }

        private void ParseTextMsg(IMessage textMsg)
        {
            var message = (CUserMessageTextMsg)textMsg;
            _ctx.Observer.Raise(typeof(CUserMessageTextMsg), new ObjectArgs(message));
        }

        private void ParseSayText2(IMessage sayText)
        {
            var message = (CUserMessageSayText2)sayText;
            _ctx.Observer.Raise(typeof(CUserMessageSayText2), new ObjectArgs(message));
        }

        public void ParsePacket(byte[] data)
        {
            var packet = CDemoPacket.Parser.ParseFrom(data);
            ParseEmbeddedPackets(packet.Data.ToByteArray());
        }

        private void ParseEmbeddedPackets(byte[] data)
        {
            var stream = new BitArrayStream(data);

            while (stream.Length - stream.Position > 32)
            {
                var kind = stream.ReadUBitVar();
                var dataLength = stream.ReadProtobufVarInt();
                var packetData = stream.ReadBytes(dataLength);

                var message = PacketParserType.GetParserByPacketType((int) kind)?.ParseFrom(packetData);

                ProcessMessage(message);
            }
        }

        private void ProcessMessage(IMessage message)
        {
            foreach (var item in MappedMessages)
            {
                if(item.Key(message))
                {
                    item.Value(message);
                }
            }
        }
        private void ParseEntity(IMessage entityPacket)
        {
            CSVCMsg_PacketEntities message = (CSVCMsg_PacketEntities)entityPacket;

            var stream = new BitArrayStream(message.EntityData.ToByteArray());
            var updateCount = message.UpdatedEntries;
            var entityIndex = -1;
            var dtClasses = _ctx.DtClasses;
            int cmd;
            int classId;
            DtClass cls;
            int serial;
            object[] state;
            Entity entity;

            while (updateCount-- != 0)
            {
                entityIndex += (int) stream.ReadUBitVar() + 1;
                cmd = (int) stream.ReadInt(2);
                if ((cmd & 1) == 0)
                {
                    if ((cmd & 2) != 0)
                    {
                        classId = (int)stream.ReadInt(dtClasses.ClassBits);
                        cls = dtClasses.ForClassId(classId);
                        if (cls == null)
                        {
                            throw new Exception("Not found class in DtClass");
                        }
                        serial = (int) stream.ReadInt(17);
                        stream.ReadVarUInt(); // extra varint for s2 type engine. 17 is for dota 2 on s2, indexbits is 14
                        state = GetBaseline(cls.GetClassId);
                        fieldReader.ReadFields(stream, (S2DTClass)cls, state, false);
                        entity = new Entity(entityIndex, serial, cls, true, state);
                        Entities[entityIndex] = entity;

                        _ctx.Observer.Raise(typeof(Entity), new ObjectArgs(entity));
                        //evCreated.raise(entity);
                        //evEntered.raise(entity);
                    }
                    else
                    {
                        entity = Entities[entityIndex];
                        if(entity == null)
                        {
                            throw new Exception($"Entity wan not found at index for update {entityIndex}");
                        }

                        cls = (S2DTClass)entity.ClassDt;
                        state = entity.State;
                        int changed = fieldReader.ReadFields(stream, (S2DTClass)cls, state, false);
                        // event update evUpdated.raise
                        if(!entity.Active)
                        {
                            entity.Active = true;
                            // evEntered.raise(entity)
                        }
                    }
                }
                else
                {
                    entity = Entities[entityIndex];
                    if (entity == null)
                    {
                        // log about it
                    }
                    else
                    {
                        if (entity.Active)
                        {
                            entity.Active = false;
                            //evLeft.raise(entity)
                        }

                        if ((cmd & 2) != 0)
                        {
                            Entities[entityIndex] = null;
                            //evDeleted.raise(entity);
                        }
                    }
                }
            }

            if (message.IsDelta)
            {
                int n = fieldReader.ReadDeletions(stream, 14, Deletions);
                for (int i = 0; i < n; i++)
                {
                    entityIndex = Deletions[i];
                    entity = Entities[entityIndex];
                    if (entity != null)
                    {
                        if (entity.Active)
                        {
                            entity.Active = false;
                            //evLeft.raise(entity);
                        }
                        //evDeleted.raise(entity);
                    }
                    else
                    {
                        // nothing
                    }

                    Entities[entityIndex] = null;
                }
            }

            //evUpdatesCompleted.raise();
        }

        public void ParseClassInfoMessage(IMessage classInfo)
        {
            var message = (CSVCMsg_ClassInfo)classInfo;
            foreach (var class_t in message.Classes)
            {
                var class_dt = _ctx.DtClasses.ForDtName(class_t.ClassName);
                class_dt.SetClassId(class_t.ClassId);
                _ctx.DtClasses.AddClassId(class_t.ClassId, class_dt);
            }

            _ctx.DtClasses.ClassBits =
                Utils.CalculateBitsNeededFor(_ctx.DtClasses.ClassCount - 1);

        }

        public void ParseBuild(IMessage serverInfo)
        {
            var message = (CSVCMsg_ServerInfo)serverInfo;
            var dir = message.GameDir;
            var regex = new Regex("dota_v(\\d+)");
            var matches = regex.Matches(dir);
            var test = matches[0].Value.Substring(6);
            _ctx.GameBuild.BuildNumber = int.Parse(test);
        }

        private void OnCreateStringTable()
        {
            numTables = 0;
        }

        private void OnCreateStringTable(IMessage table)
        {
            CSVCMsg_CreateStringTable message = (CSVCMsg_CreateStringTable)table;
            if(IsProcessed(message.Name))
            {
                var tbl = new StringTableModel(
                    message.Name,
                    100,
                    message.UserDataFixedSize,
                    message.UserDataSize,
                    message.UserDataSizeBits,
                    message.Flags);
                var data = message.StringData;

                if (message.DataCompressed)
                {
                    byte[] dst;
                    if (_ctx.GameBuild.BuildNumber != -1 && _ctx.GameBuild.BuildNumber <= 962)
                    {
                        dst = Lzss.Unpack(data);
                    }
                    else
                    {
                        dst = SnappyCodec.Uncompress(data.ToByteArray());
                    }
                    data = ByteString.CopyFrom(dst);
                }

                DecodeEntries(tbl, 3, data, message.NumEntries);
                StringTables.OnStringTableModelCreated(numTables, tbl);
            }

            numTables++;
        }

        private bool IsProcessed(string tblName)
        {
            return RequestedTables.Contains("*") || RequestedTables.Contains(tblName);
        }

        private void OnBaseLineEntry(StringTableModel table, ByteString value, string name)
        {
            if (!BaselineEntries.ContainsKey(int.Parse(name)))
                BaselineEntries.Add(int.Parse(name), new BaselineEntry(value));
        }

        private void DecodeEntries(StringTableModel table, int mode, ByteString data, int numEntries)
        {
            var stream = new BitArrayStream(data.ToByteArray());
            var keyHistory = new LinkedList<string>();

            int index = -1;
            ByteString value;

            for(var nameBuf = new StringBuilder(); numEntries-- > 0; SetSingleEntry(table, mode, index, nameBuf.ToString(), value))
            {
                if (stream.ReadBit())
                {
                    index++;
                }
                else
                {
                    index = stream.ReadVarUInt() + 1;
                }

                nameBuf.Length = 0;
                if (stream.ReadBit())
                {
                    if (stream.ReadBit())
                    {
                        int basis = (int)stream.ReadInt(5);
                        int len = (int) stream.ReadInt(5);
                        if (basis >= keyHistory.Count)
                        {
                            for (int i = 0; i < len; i++)
                            {
                                nameBuf.Append('_');
                            }
                            nameBuf.Append(stream.ReadString(MaxNameLength));
                        }
                        else
                        {
                            nameBuf.Append(keyHistory.ElementAt(basis).Substring(0, len));
                            nameBuf.Append(stream.ReadString(MaxNameLength - len));
                        }
                    }
                    else
                    {
                        nameBuf.Append(stream.ReadString(MaxNameLength));
                    }

                    if (keyHistory.Count == KeyHistorySize)
                    {
                        keyHistory.Remove(keyHistory.ElementAt(0));
                    }

                    keyHistory.AddLast(nameBuf.ToString());
                }

                value = null;
                if (stream.ReadBit())
                {
                    bool isCompressed = false;
                    int bitLen;
                    if (table.UserDataFixedSize)
                    {
                        bitLen = table.UserDataSizeBits;
                    }
                    else
                    {
                        if ((table.Flags & 0x1) != 0)
                        {
                            isCompressed = stream.ReadBit();
                        }

                        bitLen = (int) stream.ReadInt(17) * 8;
                    }

                    int byteLen = (bitLen + 7) / 8;
                    byte[] valBuf;
                    if (isCompressed)
                    {
                        tempBuf = stream.ReadBytes(byteLen);
                        int byteLenUncompressed = SnappyCodec.GetUncompressedLength(tempBuf,0 ,byteLen);
                        valBuf = new byte[byteLenUncompressed];
                        SnappyCodec.Uncompress(tempBuf, 0, byteLen, valBuf, 0);
                    }
                    else
                    {
                        valBuf = new byte[byteLen];
                        valBuf = stream.ReadBytes(byteLen);
                    }

                    value = ByteString.CopyFrom(valBuf);
                }
            }
        }

        private void SetSingleEntry(StringTableModel table, int mode, int index, string name, ByteString value)
        {
            if (name.Length == 0 && table.HasIndex(index))
            {
                name = table.GetNameByIndex(index);
            }
            table.Set(mode, index, name, value);

            if (table.Name.Equals("instancebaseline"))
                    OnBaseLineEntry(table, value, name);
        }

        private object[] GetBaseline(int classId)
        {
            this.BaselineEntries.TryGetValue(classId, out var baseline);

            if (baseline == null)
            {
                throw new Exception($"Baseline not found for classId = {classId}");
            }
            if (baseline.BaseLine == null)
            {
                var cls = _ctx.DtClasses.ForClassId(classId);
                var stream = new BitArrayStream(baseline.RawBaseLine.ToByteArray());
                baseline.BaseLine = cls.GetEmptyStateArray();
                fieldReader.ReadFields(stream, (S2DTClass)cls, baseline.BaseLine, false);
            }
            return (object[])baseline.BaseLine.Clone();
        }

        private void ParseCombatLog(IMessage combatLog)
        {
            var message = (CMsgDOTACombatLogEntry)combatLog;
            var log = new CombatLogEntry(StringTables.ForName("CombatLogNames"), message);
            _ctx.Observer.Raise(typeof(CombatLogEntry), new ObjectArgs(log));
        }

        private void ParseEventList(IMessage eList)
        {
            CSVCMsg_GameEventList message = (CSVCMsg_GameEventList)eList;
            foreach(var e in message.Descriptors)
            {
                var keys = new string[e.Keys.Count];
                for (var i = 0; i < e.Keys.Count; i++)
                {
                    var key = e.Keys[i];
                    keys[i] = key.Name;
                }

                var gameEvent = new GameEventEntry(e.Eventid, e.Name, keys);
                GameEventEntryId[gameEvent.EventId] = gameEvent;
                GameEventEntryName[gameEvent.Name] = gameEvent;
            }

        }

        private void ParseGameEvent(IMessage gEvent)
        {
            var message = (CSVCMsg_GameEvent)gEvent;
            var @event = GameEventEntryId[message.Eventid];
            var e = new GameEventModel(@event);

            for (var i = 0; i < message.Keys.Count; i ++)
            {
                var key = message.Keys[i];
                object value;

                switch(key.Type)
                {
                    case 1:
                        value = key.ValString;
                        break;
                    case 2:
                        value = key.ValFloat;
                        break;
                    case 3:
                        value = key.ValLong;
                        break;
                    case 4:
                        value = key.ValShort;
                        break;
                    case 5:
                        value = key.ValByte;
                        break;
                    case 6:
                        value = key.ValBool;
                        break;
                    case 7:
                        value = key.ValUint64;
                        break;

                    default:
                        throw new Exception($"Cannot handle game event key type {key.Type}");
                }
                e.Set(i, value);
            }
            _ctx.Observer.Raise(typeof(GameEventModel), new ObjectArgs(e));
        }

    }
}