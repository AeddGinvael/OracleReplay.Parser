using System;
using System.IO;

namespace Oracle.BitStream
{
	public interface IBitStream : IDisposable
	{
		void Initialize(Stream stream);

		uint ReadInt(int bits);
		int ReadSignedInt(int numBits);
		bool ReadBit();
		byte ReadByte();
		byte ReadByte(int bits);
		byte[] ReadBytes(int bytes);
		float ReadFloat();
		byte[] ReadBits(int bits);
		int ReadProtobufVarInt();
	}
}

