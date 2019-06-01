using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Oracle.Packets.Fields;

namespace Oracle.BitStream
{
	public static class BitStreamUtil
	{
		private static int _coordIntBits = 14;
		private static  int _coordFractionalBits = 5;
		private static  float _coordResolution = (1.0f / (1 << _coordFractionalBits));

		private static int _coordIntBitsMp = 11;
		private static int _coordFractionalBitsMpLowPrecision = 3;
		private static int _coordDenominatorLowPrecision = (1 << _coordFractionalBitsMpLowPrecision);
		private static float _coordResolutionLowPrecision = (1.0f / _coordDenominatorLowPrecision);

		private static int _normalFractionalBits = 11;
		private static float _normalFractionalResolution = (1.0f / ((1 << _normalFractionalBits) - 1));

		public static  ulong[] Masks =
		{
			0x0L,               0x1L,                0x3L,                0x7L,
			0xfL,               0x1fL,               0x3fL,               0x7fL,
			0xffL,              0x1ffL,              0x3ffL,              0x7ffL,
			0xfffL,             0x1fffL,             0x3fffL,             0x7fffL,
			0xffffL,            0x1ffffL,            0x3ffffL,            0x7ffffL,
			0xfffffL,           0x1fffffL,           0x3fffffL,           0x7fffffL,
			0xffffffL,          0x1ffffffL,          0x3ffffffL,          0x7ffffffL,
			0xfffffffL,         0x1fffffffL,         0x3fffffffL,         0x7fffffffL,
			0xffffffffL,        0x1ffffffffL,        0x3ffffffffL,        0x7ffffffffL,
			0xfffffffffL,       0x1fffffffffL,       0x3fffffffffL,       0x7fffffffffL,
			0xffffffffffL,      0x1ffffffffffL,      0x3ffffffffffL,      0x7ffffffffffL,
			0xfffffffffffL,     0x1fffffffffffL,     0x3fffffffffffL,     0x7fffffffffffL,
			0xffffffffffffL,    0x1ffffffffffffL,    0x3ffffffffffffL,    0x7ffffffffffffL,
			0xfffffffffffffL,   0x1fffffffffffffL,   0x3fffffffffffffL,   0x7fffffffffffffL,
			0xffffffffffffffL,  0x1ffffffffffffffL,  0x3ffffffffffffffL,  0x7ffffffffffffffL,
			0xfffffffffffffffL, 0x1fffffffffffffffL, 0x3fffffffffffffffL, 0x7fffffffffffffffL,
			0xffffffffffffffffL
		};

    	private static int[] _ubvCount = {0, 4, 8, 28};
    	private static int[] _ubvfpCount = {2, 4, 10, 17, 31};

		/// <summary>
		/// Creates an instance of the preferred <see cref="IBitStream"/> implementation for streams.
		/// </summary>
		public static IBitStream Create(Stream stream)
		{
			var bitStream = new BitArrayStream();

			bitStream.Initialize(stream);
			return bitStream;
		}

		/// <summary>
		/// Creates an instance of the preferred <see cref="IBitStream"/> implementation for byte arrays.
		/// </summary>
		public static IBitStream Create(byte[] data)
		{
			var bitStream = new BitArrayStream();
			bitStream.Initialize(new MemoryStream(data));
			return bitStream;
		}

		public static uint ReadUBitInt(this IBitStream bs, int bits)
		{
			uint ret = bs.ReadInt(bits);

			uint a = ret >> 4;

			if (a == 0)
			{
				return ret;
			}

			if (a > _ubvCount.Length - 1)
			{
				return ret;
			}
			var res = (ret & 15) | (bs.ReadInt(_ubvCount[a]) << 4);
			return res;
		}
		public static uint ReadUBitVar(this IBitStream bs)
		{
			uint ret = bs.ReadInt(6);

			uint a = ret >> 4;

			if (a == 0)
			{
				return ret;
			}
			var res = (ret & 15) | (bs.ReadInt(_ubvCount[a]) << 4);
			return res;

		}

		public static string ReadString(this IBitStream bs)
		{
			return bs.ReadString(Int32.MaxValue);
		}

		public static string ReadString(this IBitStream bs, int limit)
		{
			var result = new List<byte>(512);
			for (int pos = 0; pos < limit; pos++) {
				var b = bs.ReadByte();
				if ((b == 0) || (b == 10))
					break;
				result.Add(b);
			}
			return Encoding.ASCII.GetString(result.ToArray());
		}
		
		public static int ReadUBitVarFieldPath(this IBitStream reader)
		{
			int i = -1;
			while (++i < 4)
			{
				if (reader.ReadBit())
					break;
				
			}

			//return (int)reader.ReadUBitInt(UBV_COUNT[i]);
			return (int)reader.ReadInt(_ubvfpCount[i]);
		}

		public static string ReadCString(this IBitStream reader, int length)
		{
			return Encoding.Default.GetString(reader.ReadBytes(length)).Split(new char[] { '\0' }, 2)[0];
		}

		public static uint ReadVarInt(this IBitStream bs, int side = 5)
		{
			uint tmpByte = 0x80;
			uint result = 0;
			for (int count = 0; (tmpByte & 0x80) != 0; count++) {
				if (count > side)
					throw new InvalidDataException("VarInt32 out of range");
				tmpByte = bs.ReadByte();
				result |= (tmpByte & 0x7F) << (7 * count);
			}
			return result;
		}


		public static uint ReadSignedVarInt(this IBitStream bs)
		{
			uint result = bs.ReadVarInt();
			return (uint)((result >> 1) ^ -(result & 1));
		}

		public static int ReadProtobufVarIntStub(IBitStream reader)
		{
			byte b = 0x80;
			int result = 0;
			for (int count = 0; (b & 0x80) != 0; count++) {
				b = reader.ReadByte();

				if ((count < 4) || ((count == 4) && (((b & 0xF8) == 0) || ((b & 0xF8) == 0xF8))))
					result |= (b & ~0x80) << (7 * count);
				else {
					if (count >= 10)
						throw new OverflowException("10 bytes max!");
					if ((count == 9) ? (b != 1) : ((b & 0x7F) != 0x7F))
						throw new NotSupportedException("more than 32 bits are not supported");
				}
			}

			return result;
		}

		public static long ReadVarU(this IBitStream reader, int max)
		{
			int m = ((max + 6) / 7) * 7;
			int s = 0;
			long v = 0L;
			long b;
			while (true)
			{
				//b = reader.ReadUBitInt(8);
				b = reader.ReadInt(8);
				v |= (b & 0x7FL) << s;
				s += 7;
				if ((b & 0x80L) == 0L || s == m)
				{
					return v;
				}
			}
		}

		public static long ReadUBitLong(this IBitStream reader, int n)
		{
			if (n > 32)
			{
				long l = reader.ReadUBitInt(32);
				long h = reader.ReadUBitInt(n - 32);
				return h << 32 | l;
			}

			return reader.ReadUBitInt(n);
		}

		public static float ReadBitCoord(this IBitStream reader)
		{
			var i = reader.ReadBit();
			var f = reader.ReadBit();
			var v = 0.0f;
			if (!(i || f))
				return v;

			var s = reader.ReadBit();
			if (i)
				//v = (float) (reader.ReadUBitInt(COORD_FRACTIONAL_BITS) * COORD_RESOLUTION);
				v = (float) (reader.ReadInt(_coordIntBits) + 1);
			if (f)
				v += reader.ReadInt(_coordFractionalBits) * _coordResolution;

			return s ? -v : v;

		}

		public static float ReadBitAngle(this IBitStream reader, int n)
		{
			return reader.ReadUBitInt(n) * 360.0f / (1 << n);
		}

		public static float[] Read3BitNormal(this IBitStream reader)
		{
			var v = new float[3];
			bool x = reader.ReadBit();
			bool y = reader.ReadBit();

			if (x)
				v[0] = reader.ReadBitNormal();
			if (y)
				v[1] = reader.ReadBitNormal();
			bool s = reader.ReadBit();
			float p = v[0] * v[0] + v[1] * v[1];

			if (p < 1.0f)
				v[2] = (float) Math.Sqrt(1.0f - p);

			if (s)
				v[2] = -v[2];
			return v;
		}

		public static float ReadBitNormal(this IBitStream reader)
		{
			bool s = reader.ReadBit();
			float v = (float) reader.ReadUBitInt(_normalFractionalBits) * _normalFractionalResolution;

			return s ? -v : v;
		}
		
		public static long ReadVarULong(this IBitStream reader) {
			return ReadVarU(reader, 64);
		}
		
		public static int ReadVarUInt(this IBitStream reader) {
			return (int) ReadVarU(reader,32);
		}

		public static int ReadVarSInt(this IBitStream reader)
		{
			return (int) reader.ReadVarS(32);
		}

		public static long ReadVarSLong(this IBitStream reader)
		{
			return reader.ReadVarS(64);
		}
		

		public static long ReadVarS(this IBitStream reader, int max)
		{
			long v = reader.ReadVarU(max);
			return (v >>  1) ^ -(v & 1L);
		}
	}
}

