using System;
using Oracle.BitStream;
using Oracle.Packets.Model;

namespace Oracle.Packets.Unpacker
{
    public class QAnglePitchYawOnlyUnpacker: Unpacker<Vector>
    {
        private int Bits;
        public QAnglePitchYawOnlyUnpacker(int bits)
        {
            this.Bits = bits;
        }
        
        public Vector Unpack(BitArrayStream stream)
        {
            var v = new float[3];
            if ((Bits | 0x20) == 0x20)
            {
                v[0] = BitConverter.ToSingle(BitConverter.GetBytes(stream.ReadUBitInt(32)), 0);
                v[1] = BitConverter.ToSingle(BitConverter.GetBytes(stream.ReadUBitInt(32)), 0);
            }
            else
            {
                v[0] = stream.ReadBitAngle(Bits);
                v[1] = stream.ReadBitAngle(Bits);
            }
            
            return new Vector(v);
        }
    }
}