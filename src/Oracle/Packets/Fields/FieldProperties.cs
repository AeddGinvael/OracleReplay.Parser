using Oracle.Packets.Model;

namespace Oracle.Packets.Fields
{
    public class FieldProperties
    {
        public FieldType Type { get; set; }
        public string Name { get; set; }
        public string SendNode { get; set; }
        public int? EncodeFlags { get; set; }
        public int? BitCount { get; set; }
        public float? LowVal { get; set; }
        public float? HighVal { get; set; }
        public Serializer Serializer { get; set; }
        public string EncoderType { get; set; }
        public string SerializerType { get; set; }

        public int? GetEncoderFlagsoOrDefault(int defaultVal)
        {
            return EncodeFlags ?? defaultVal;
        }

        public int? GetBitCountOrDefault(int defaultVal)
        {
            return BitCount ?? defaultVal;
        }

        public float? GetLowValueOrDefault(float defaultVal)
        {
            return LowVal ?? defaultVal;
        }
        
        public float? GetHighValueOrDefault(float defaultVal)
        {
            return HighVal ?? defaultVal;
        }

    }
}