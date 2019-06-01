using System.IO;

namespace Oracle.Packets.Model
{
    public struct SerializerId
    {
        public string Name { get; }
        public int Version { get; }

        public SerializerId(string Name, int Version)
        {
            this.Name = Name;
            this.Version = Version;
        }

        public override bool Equals(object obj)
        {
           if (obj == null) return false;
           if (!(obj is SerializerId)) return false;
           var si = (SerializerId) obj;
           return si.Name.Equals(Name) && si.Version == Version;
        }

        public override int GetHashCode() => 31 * Name.GetHashCode() + Version;

        public override string ToString() => $"{Name}({Version})";
    }
}