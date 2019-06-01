using Oracle.BitStream;
using Oracle.Packets.Model;

namespace Oracle.Packets.Unpacker
{
    public class QAngleNoBitCountUnpacker: Unpacker<Vector>
    {
        public Vector Unpack(BitArrayStream stream)
        {
            var v = new float[3];
            bool b0 = stream.ReadBit();
            bool b1 = stream.ReadBit();
            bool b2 = stream.ReadBit();

            if (b0)
                v[0] = stream.ReadBitCoord();
            if (b1)
                v[1] = stream.ReadBitCoord();
            if (b2)
                v[2] = stream.ReadBitCoord();
            return new Vector(v);
        }
    }
}