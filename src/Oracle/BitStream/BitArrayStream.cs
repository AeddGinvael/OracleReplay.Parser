using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Snappy;
using Oracle.Packets.Fields;
using System.Runtime.CompilerServices;
namespace Oracle.BitStream
{
	public class BitArrayStream : IBitStream
	{
		private BitArray array;
		private long[] Data { get; set; }
		private readonly List<int> RemainingInOldChunks = new List<int>();
		private int RemainingInCurrentChunk = -1;
		/// <summary>
		/// In bits
		/// </summary>
		public int Position { get; private set; }
		/// <summary>
		/// In bits
		/// </summary>
		private long _len;
		public long Length {
			get => _len;
			private set => _len = value * 8;
		}

		public BitArrayStream()
		{
		}

		public BitArrayStream(byte[] data)
		{
			array = new BitArray(data);
			Position = 0;
			Length = data.Length;
			Data = new long[(Length + 15) >> 3];
		}

		public void Initialize(Stream stream)
		{
			using (var memstream = new MemoryStream(checked((int)stream.Length))) {
				stream.CopyTo(memstream);
				array = new BitArray(memstream.GetBuffer());
				Length = stream.Length;
				Data = new long[(Length + 15) >> 3];
			}

			Position = 0;
		}

		public uint ReadInt(int numBits)
		{
			uint result = PeekInt(numBits);
			Position += numBits;
			if (RemainingInCurrentChunk >= 0) {
				if (numBits > RemainingInCurrentChunk)
					throw new OverflowException("Trying to read beyond a chunk boundary!");
				else {
					RemainingInCurrentChunk -= numBits;
					for (int i = 1; i < RemainingInOldChunks.Count; i++)
						RemainingInOldChunks[i] -= numBits;
				}
			}

			return result;
		}

		public bool ReadBitFlag()
		{
			return (int)((Data[Position >> 6] >> (Position & 63)) & 1L) != 0;
		}

		private uint PeekInt(int numBits)
		{
			uint result = 0;
			int intPos = 0;
			
			for (int i = 0; i < numBits; i++) {
				result |= ( ( array[i + Position] ? 1u : 0u ) << intPos++ );
			}

			return result;
		}

		public bool ReadBit()
		{
			return ReadInt(1) == 1;
		}

		public byte ReadByte()
		{
			return (byte)ReadInt(8);
		}

		public byte ReadByte(int numBits)
		{
			return (byte)ReadInt(numBits);
		}

		public byte[] ReadBytes(int length)
		{
			byte[] result = new byte[length];

			for (int i = 0; i < length; i++) {
				result[i] = this.ReadByte();
			}

			return result;
		}

		public string PeekBools(int length)
		{
			byte[] buffer = new byte[length];

			int idx = 0;
			for (int i = Position; i < Math.Min(Position + length, array.Count); i++) {
				if (array[i])
					buffer[idx++] = 49;
				else
					buffer[idx++] = 48;
			}

			return Encoding.ASCII.GetString(buffer, 0, Math.Min(length, array.Count - Position));
		}

		public int ReadSignedInt(int numBits)
		{
			// Read the int normally and then shift it back and forth to extend the sign bit.
			return ( ( (int)ReadInt(numBits) ) << ( 32 - numBits ) ) >> ( 32 - numBits );
		}

		void IDisposable.Dispose()
		{
			array = null;
		}


		public float ReadFloat()
		{
			return BitConverter.ToSingle(ReadBytes(4), 0);
		}

		public byte[] ReadBits(int bits)
		{
			byte[] result = new byte[(bits + 7) / 8];

			for (int i = 0; i < (bits / 8); i++)
				result[i] = this.ReadByte();

			if ((bits % 8) != 0)
				result[bits / 8] = ReadByte(bits % 8);

			return result;
		}

		public int ReadProtobufVarInt()
		{
			return BitStreamUtil.ReadProtobufVarIntStub(this);
		}

		private long ReadSpanLong(int offset = 0)
		{
			var byteData = new byte[Length];
			array.CopyTo(byteData, 0);
			Span<byte> result = byteData;
			var v = MemoryMarshal.Read<long>(result.Slice(offset, 8 + offset));
			result.Clear();
			return v;
		}

        public FieldOpType ReadFieldOpType()
        {
            var offset = (long)(Position >> 3 & -8);
            var b = 1L << (Position & 63);
            var i = 0;
            var v = ReadSpanLong((int)offset);

            while (true)
            {
                ++Position;
                i = FieldOpHuffmanTree.Tree[i][(v & b) != 0L ? 1 : 0];
                if (i < 0)
                {
	                return FieldOpHuffmanTree.Ops[-i - 1];
                }

                b <<= 1;
                if (b == 0L)
                {
	                offset+= 8L;
	                v = ReadSpanLong((int)offset);
	                b = 1L;
                }
            }
        }
	}
}
