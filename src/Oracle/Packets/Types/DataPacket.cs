namespace Oracle.Packets.Types
{
    public struct DataPacket
    {
        public KindCommand Kind { get; }
        public int Tick { get; }
        public byte[] Data { get; }

        public DataPacket(KindCommand kind, int tick,  byte[] data)
        {
            Kind = kind;
            Tick = tick;

            Data = data;
        }

        public override string ToString()
        {
            return $"Type: {Kind.Type}. Tick: {Tick}. IsCompressed: {Kind.IsDataCompressed()}." +
            $" Data Length: {Data.Length}";
        }
    }
}