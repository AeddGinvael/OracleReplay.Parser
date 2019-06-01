using System;
using Oracle.BitStream;
using Oracle.Packets.Model;

namespace Oracle.Packets.Unpacker
{
    public class QAngleNoScaleUnpacker: Unpacker<Vector>
    {
        public Vector Unpack(BitArrayStream stream)
        {
            var v = new float[3];
            v[0] = BitConverter.ToSingle(BitConverter.GetBytes(stream.ReadInt(32)), 0);
            v[1] = BitConverter.ToSingle(BitConverter.GetBytes(stream.ReadInt(32)), 0);
            v[2] = BitConverter.ToSingle(BitConverter.GetBytes(stream.ReadInt(32)), 0);
            return new Vector(v);
        }
    }
}