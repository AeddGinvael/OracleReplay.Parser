using System;
using Oracle.BitStream;
using Oracle.Packets.Fields;

namespace Oracle.Packets.Unpacker.Factory
{
    public class FloatUnpackerFactory: UnpackerFactory<float>
    {
        private static float FrameTime = (1.0f / 30.0f);

        public static Func<BitArrayStream, object> CreateFuncStatic (FieldProperties props)
        {
            if ("coord".Equals(props.EncoderType))
            {
                return f => f.ReadBitCoord();
            }

            if ("simulationtime".Equals(props.SerializerType))
            {
                return f => f.ReadInt(8) * FrameTime;
            }

            var bc = props.GetBitCountOrDefault(0);
            if (bc <= 0 || bc >= 32)
            {
                return f => BitConverter.ToSingle(BitConverter.GetBytes((int)f.ReadInt(32)), 0);
            }

            return f => {
                var unpacker =
                    new FloatQuantizedUnpacker(props.Name, bc, props.GetEncoderFlagsoOrDefault(0) & 0xF, props.GetLowValueOrDefault(0.0f), props.GetHighValueOrDefault(0.0f));
                return unpacker.Unpack(f);
            };
        }

        public Func<BitArrayStream, object> CreateFunc (FieldProperties props)
        {
            return CreateFuncStatic(props);
        }

    }
}