using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Oracle.BitStream;
using Oracle.Events;
using Oracle.Packets;
using Oracle.Packets.Model;
using Oracle.Packets.Types;
using Oracle.Utilities;
using Proto.Dota;

namespace Oracle
{
    public class ReplayParser
    {
        public SendTablesPacket SendTablesParser { get; set; }
        public DemPacket DemParser { get; set; }
        public StringTablePacket StringTableParser { get; set; }
        public long CurrentTick { get; private set; }
        private readonly IBitStream BitStream;

        private ReplayContext Context { get; set; }

        public ReplayParser(string pathToFile)
        {
            if (!File.Exists(pathToFile))
                throw new FileNotFoundException($"File is not found: {pathToFile}");

            BitStream = BitStreamUtil.Create(File.OpenRead(pathToFile));
            Init();
        }

        public ReplayParser(Stream stream)
        {
            BitStream = BitStreamUtil.Create(stream);
            Init();
        }

        public void ConfigureObserver(Action<EventBuilder> configureBuilder)
        {
            if (configureBuilder == null)
            {
                throw new ArgumentNullException(nameof(configureBuilder));
            }
            var eventBuilder = new EventBuilder();
            configureBuilder(eventBuilder);
            Context.Observer = eventBuilder.Build();
        }

        private void Init()
        {
            ParseHeader();

            Context = new ReplayContext();
            SendTablesParser = new SendTablesPacket(Context);
            DemParser = new DemPacket(Context);
            StringTableParser = new StringTablePacket(Context);
        }


        private void ParseHeader()
        {
            var header = BitStream.ReadCString(8);
            if (!header.Equals("PBDEMS2"))
                throw new InvalidDataException("Header does not match to PBDEMS2");
            BitStream.ReadBytes(8);
        }

        public void ParseDemReplay()
        {
            ParseDemReplay(CancellationToken.None);
        }

        public void ParseDemReplay(CancellationToken token)
        {
            while (ParseNext())
            {
                if (token.IsCancellationRequested) return;
            }
        }

        public void ParseTick()
        {
            ParseNext();
        }

        private bool ParseNext()
        {
            //first 32 bits is a kind of command
            //KindT is special struct for Kind of the replay's commands.
            KindCommand kind = (int) BitStream.ReadVarInt();
            //if tick is equals to -1 it means pre-game stage
            var tick = (int) BitStream.ReadVarInt();
            CurrentTick = tick;
            //Size in bytes
            var size = BitStream.ReadVarInt();
            var data = BitStream.ReadBytes((int) size);
            if (kind.IsDataCompressed())
            {
                data = Snappy.SnappyCodec.Uncompress(data);
            }

            var dataPacket = new DataPacket(kind, (int)tick, data);
            return DataConveyer(dataPacket);
        }

        private bool DataConveyer(DataPacket packet)
        {
            if (packet.Kind.Type is DemoCommandsEnum.DemFileHeader)
            {
                var header = CDemoFileHeader.Parser.ParseFrom(packet.Data);
                Context.Observer.Raise(typeof(CDemoFileHeader), new ObjectArgs(header));
            }

            if (packet.Kind.Type is DemoCommandsEnum.DemSendTables)
            {
                SendTablesParser.ParseSendTable(packet.Data, BitStream);
            }
            if (packet.Kind.Type is DemoCommandsEnum.DemClassInfo)
            {
                var classInfo = CDemoClassInfo.Parser.ParseFrom(packet.Data);
                SendTablesParser.ParseClassInfo(classInfo);
            }
            if (packet.Kind.Type is DemoCommandsEnum.DemSignonPacket ||
                packet.Kind.Type is DemoCommandsEnum.DemPacket)
            {
                DemParser.ParsePacket(packet.Data);
            }
            if (packet.Kind.Type is DemoCommandsEnum.DemStringTables)
            {
                StringTableParser.ParsePacket(packet.Data);
            }
            if (packet.Kind.Type is DemoCommandsEnum.DemFileInfo)
            {
                var fileinfo = CDemoFileInfo.Parser.ParseFrom(packet.Data);
                Context.Observer.Raise(typeof(CDemoFileInfo), new ObjectArgs(fileinfo));
                return false;
            }

            if (packet.Kind.Type is DemoCommandsEnum.DemError)
                throw new ReplayException($"Replay contains error on tick: {packet.Tick}");

            return true;
        }
    }
}