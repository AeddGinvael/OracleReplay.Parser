using System;
using System.Collections;
using Oracle.BitStream;
using Oracle.Packets.Fields;
using Oracle.Packets.Model;

namespace Oracle.Packets.Unpacker.Factory
{
    public class VectorUnpackerFactory: UnpackerFactory<Vector>
    {
        private int Dim;

        public VectorUnpackerFactory(int dim)
        {
            Dim = dim;
        }

        public Func<BitArrayStream, object> CreateFunc(FieldProperties props)
        {
            if (Dim == 3 && "normal".Equals(props.EncoderType))
            {
                return f => new Vector(f.Read3BitNormal());
            }

            return f =>
            {
                var unpacker = new VectorDefaultUnpacker(Dim, FloatUnpackerFactory.CreateFuncStatic(props));
                return unpacker.Unpack(f);
            };
        }
    }
}