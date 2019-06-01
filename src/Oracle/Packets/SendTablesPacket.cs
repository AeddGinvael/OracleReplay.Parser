
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Google.Protobuf;
using Oracle.BitStream;
using Oracle.Packets.Fields;
using Oracle.Packets.Model;
using Proto.Dota;
using Utils = Microsoft.VisualBasic.CompilerServices.Utils;

namespace Oracle.Packets
{
    public class SendTablesPacket
    {
        private ReplayContext _ctx;
        public SendTablesPacket(ReplayContext ctx)
        {
            _ctx = ctx;
        }
        private Dictionary<string, int> ItemCounts = new Dictionary<string, int>
        {
            {"MAX_ITEM_STOCKS", 8},
            {"MAX_ABILITY_DRAFT_ABILITIES", 48}
        };
 
        private FieldType CreateFieldType(string type)
        {
            return new FieldType(type);
        }

        private Field CreateField(FieldProperties props)
        {
            if (props.Serializer != null)
            {
                if (TablesField.Pointers.Contains(props.Type.BaseType))
                {
                    return new FixedSubTableFields(props);
                }
                    
                return new VarSubTableField(props);
            }

            string elemCount = props.Type.ElementCount;
            if (elemCount != null && !"char".Equals(props.Type.BaseType))
            {
                ItemCounts.TryGetValue(elemCount, out var countAsBit);
                if (countAsBit == 0)
                {
                    countAsBit = int.Parse(elemCount);
                }

                return new FixedArrayField(props, countAsBit);
            }

            if ("CUltVector".Equals(props.Type.BaseType))
            {
                return new VarArrayField(props);
            }

            return new SimpleField(props);
        }

        public void ParseClassInfo(CDemoClassInfo classInfo)
        {
            foreach (var class_t in classInfo.Classes)
            {
                DtClass dt = _ctx.DtClasses.ForDtName(class_t.NetworkName);
                dt.SetClassId(class_t.ClassId);
                _ctx.DtClasses.AddClassId(class_t.ClassId, dt);
            }

            _ctx.DtClasses.ClassBits = BitStream.Utilities.Utils.CalculateBitsNeededFor(_ctx.DtClasses.ClassCount - 1);
        }
        

        public void ParseSendTable(byte[] data, IBitStream stream)
        {
            var sendTable = CDemoSendTables.Parser.ParseFrom(data);
            var cis = new CodedInputStream(sendTable.Data.ToByteArray());
            var protoMessage = CSVCMsg_FlattenedSerializer.Parser.ParseFrom(cis.ReadBytes());
            var serializers = new Dictionary<SerializerId, Serializer>();
            var fieldTypes = new Dictionary<string, FieldType>();
            var fields = new Field[protoMessage.Fields.Count];
            var currentFields = new List<Field>(128);

            for (var si = 0; si < protoMessage.Serializers.Count; si++)
            {
                var protoSerializer = protoMessage.Serializers[si];
                currentFields.Clear();

                foreach (var fi in protoSerializer.FieldsIndex)
                {
                    var field = fields[fi];
                    if (field == null)
                    {
                        var protoField = new SerializerField(protoMessage.Symbols[protoSerializer.SerializerNameSym],
                                protoMessage, protoMessage.Fields[fi]);

                        foreach (var patch in TablesField.Patches)
                        {
                            if (patch.Key.AppliesTo(_ctx.GameBuild.BuildNumber))
                            {
                                patch.Value(protoField);
                            }
                        }

                        fieldTypes.TryGetValue(protoField.VarType, out var fieldType);
                        if (fieldType == null)
                        {
                            fieldType = CreateFieldType(protoField.VarType);
                            fieldTypes.Add(protoField.VarType, fieldType);
                        }

                        Serializer fieldSerializer = null;
                        if (protoField.SerializerName != null)
                        {
                            serializers.TryGetValue(new SerializerId(protoField.SerializerName,
                                protoField.SerializerVersion), out fieldSerializer);
                        }

                        var fieldProperties = new FieldProperties
                        {
                            Type = fieldType,
                            Name = protoField.VarName,
                            SendNode = protoField.SendNode,
                            EncodeFlags = protoField.EncodeFlags,
                            BitCount = protoField.BitCount,
                            LowVal = protoField.LowValue,
                            HighVal = protoField.HighValue,
                            Serializer = fieldSerializer,
                            EncoderType = protoField.EncoderType,
                            SerializerType = protoField.SerializerType
                        };


                        field = CreateField(fieldProperties);
                        fields[fi] = field;

                    }
                    currentFields.Add(field);
                }
                var sid = new SerializerId(
                    protoMessage.Symbols[protoSerializer.SerializerNameSym],
                    protoSerializer.SerializerVersion);

                var serializer = new Serializer(sid, currentFields.ToArray());
                serializers.Add(sid, serializer);
            }

            foreach (var serializer in serializers.Values)
            {
                DtClass dtClass = new S2DTClass(serializer);
                _ctx.DtClasses.OnDtClass(dtClass);
            }

        }
    }
}