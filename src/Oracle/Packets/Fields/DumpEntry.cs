namespace Oracle.Packets.Fields
{
    public class DumpEntry
    {
        public string FieldPath { get; }
        public string Name { get; }
        public string Value { get; }

        public DumpEntry(FieldPath fp, string name, object value)
        {
            FieldPath = fp.ToString();
            Name = name;
            Value = value.ToString();
        }
    }
}