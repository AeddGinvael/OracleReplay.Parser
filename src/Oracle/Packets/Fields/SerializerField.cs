using Proto.Dota;

namespace Oracle.Packets.Fields
{
    public class SerializerField
    {
         public string Parent { get; }
        public  string VarName{ get; }
        public  string VarType{ get; }
        public  string SendNode{ get; }
        public  string SerializerName{ get; }
        public  int SerializerVersion{ get; }
        public  string EncoderType { get; set; }
        public  string SerializerType{ get; set; }
        public  int EncodeFlags{ get; }
        public  int BitCount{ get; }
        public  float? LowValue{ get; set; }
        public  float? HighValue{ get; set; }

        public SerializerField(string parent, CSVCMsg_FlattenedSerializer serializer, ProtoFlattenedSerializerField_t field) {
            this.Parent = parent;
            this.VarName = serializer.Symbols[field.VarNameSym];
            this.VarType = serializer.Symbols[field.VarTypeSym];
            var sn = serializer.Symbols[field.SendNodeSym];
            this.SendNode = !"(root)".Equals(sn) ? sn : null;
            this.SerializerName =  field.FieldSerializerNameSym != 0 ? serializer.Symbols[field.FieldSerializerNameSym] : null;
            this.SerializerVersion = field.FieldSerializerVersion != 0 ? field.FieldSerializerVersion :  0;
            this.EncoderType = field.VarEncoderSym != 0 ? serializer.Symbols[field.VarEncoderSym] : null;
            this.SerializerType = null;
            this.EncodeFlags = field.EncodeFlags;
            this.BitCount = field.BitCount;
            this.LowValue = field.LowValue != 0 ? field.LowValue : (float?) null;
            this.HighValue = field.HighValue != 0 ? field.HighValue : (float?) null;
        }
    }
}