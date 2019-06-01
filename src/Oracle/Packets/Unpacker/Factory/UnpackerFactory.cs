using System;
using Oracle.BitStream;
using Oracle.Packets.Fields;

namespace Oracle.Packets.Unpacker.Factory
{
    public interface UnpackerFactory<T>
    {
        Func<BitArrayStream, object> CreateFunc(FieldProperties props);
    }
}