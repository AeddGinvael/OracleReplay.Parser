using System;
using System.Text;

namespace Oracle.Packets.Fields
{
    public class FieldPath: IComparable<FieldPath>
    {
        public int[] Path { get; }
        public int Last { get; set; }

        public FieldPath()
        {
            Path = new int[6];
            Path[0] = -1;
            Last = 0;
        }

        public FieldPath(int[] elements)
        {
            Path = new int[6];
            Last = Math.Min(6, elements.Length) - 1;
            Array.Copy(elements, 0, Path, 0, Last + 1);
        }

        public FieldPath(FieldPath fp)
        {
            Path = new int[6];
            Last = fp.Last;
            Array.Copy(fp.Path, 0, Path,0, Last + 1);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i <= Last; i++)
            {
                if (i != 0)
                {
                    sb.Append('/');
                }

                sb.Append(Path[i]);
            }
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is FieldPath)) return false;
            var fieldPath = (FieldPath) obj;

            if (Last != fieldPath.Last) return false;

            for (var i = 0; i <= Last; i++)
            {
                if (Path[i] != fieldPath.Path[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            var prime = 5;
            for (var i = 0; i <= Last; i++)
            {
                prime = 7 * prime + Path[i];
            }

            return prime;
        }

        public int CompareTo(FieldPath other)
        {
            if (Equals(this, other)) return 0;

            var n = Math.Min(Last, other.Last);
            for (var i = 0; i <= n; i++)
            {
                var r = Path[i].CompareTo(other.Path[i]);
                if (r != 0)
                {
                    return r;
                }
            }

            return Last.CompareTo(other.Last);
        }
    }
}