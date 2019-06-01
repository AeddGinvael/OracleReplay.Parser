using System;
using Google.Protobuf;

namespace Oracle.BitStream.Utilities
{
    public class Lzss
    {
        private const int Magic = 'L' | 'Z' << 8 | 'S' << 16 | 'S' << 24;

        public static byte[] Unpack(ByteString raw)
        {
            return Unpack(new BitArrayStream(raw.ToByteArray()));
        }

        public static byte[] Unpack(BitArrayStream bitStream)
        {
            if (bitStream.ReadUBitInt(32) != Magic)
            {
                throw new Exception("Wrong LZSS constant");
            }

            var dst = new byte[(int)bitStream.ReadUBitInt(32)];
            var di = 0;
            var cmd = 0;
            var bit = 0x100;
            while (true)
            {
                if (bit == 0x100)
                {
                    cmd = (int)bitStream.ReadUBitInt(8);
                    bit = 1;
                }

                if ((cmd & bit) == 0)
                {
                    dst[di++] = (byte) bitStream.ReadUBitInt(8);
                }
                else
                {
                    var a = (int)bitStream.ReadUBitInt(8);
                    var b = (int) bitStream.ReadUBitInt(8);
                    var offset = 1 + ((a << 4) | (b >> 4));
                    var count = 1 + (b & 0x0F);
                    if (count == 1)
                    {
                        break;
                    }

                    var end = di + count;
                    for (var i = di; i < end; i++)
                    {
                        dst[i] = dst[i - offset];
                    }

                    di += count;
                }

                bit = bit << 1;

            }
            return dst;
        }
    }
}