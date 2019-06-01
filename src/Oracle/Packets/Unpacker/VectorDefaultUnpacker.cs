using System;
using Oracle.BitStream;
using Oracle.Packets.Model;

namespace Oracle.Packets.Unpacker
{
    public class VectorDefaultUnpacker: Unpacker<Vector>
    {
        private int Dim;
        private Func<BitArrayStream, object> FloatUnpacker;

        public VectorDefaultUnpacker(int dim, Func<BitArrayStream, object> floatUnpacker)
        {
            this.Dim = dim;
            this.FloatUnpacker = floatUnpacker;
        }
        public Vector Unpack(BitArrayStream stream)
        {
            var result = new float[Dim];
            for (int i = 0; i < Dim; i++)
            {
                result[i] = (float)FloatUnpacker(stream);
            }
            return new Vector(result);
        }
    }
}