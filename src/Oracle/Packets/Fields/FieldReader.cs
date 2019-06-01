using System;
using System.Collections.Generic;
using System.Text;
using Oracle.BitStream;
using Oracle.Packets.Model;

namespace Oracle.Packets.Fields
{
    abstract class FieldReader<T> where T: DtClass
    {
        public const int MAX_PROPERTIES = 16383;
        protected FieldPath[] FieldPaths = new FieldPath[MAX_PROPERTIES];

        public FieldPath[] GetFileFieldPaths()
        {
            return this.FieldPaths;
        }

        public abstract int ReadFields(BitArrayStream stream, T cls, object[] objs, bool flag);
        public abstract int ReadDeletions(BitArrayStream stream, int num, int[] nums);
    }
}
