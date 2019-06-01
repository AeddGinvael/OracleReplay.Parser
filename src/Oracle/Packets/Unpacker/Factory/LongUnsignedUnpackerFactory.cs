using System;
using Oracle.BitStream;
using Oracle.Packets.Fields;

namespace Oracle.Packets.Unpacker.Factory
{
    public class LongUnsignedUnpackerFactory: UnpackerFactory<long>
    {

        public Func<BitArrayStream, object> CreateFunc(FieldProperties props)
        {
            if ("fixed64".Equals(props.EncoderType))
            {
                return f => f.ReadUBitLong(64);
            }

            return f => f.ReadVarULong();
        }

    }
}