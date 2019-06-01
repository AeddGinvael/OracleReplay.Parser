using System;
using Oracle.BitStream;
using Oracle.Packets.Fields;
using Oracle.Packets.Model;

namespace Oracle.Packets.Unpacker.Factory
{
    public class QAngleUnpackerFactory: UnpackerFactory<Vector>
    {
        public Func<BitArrayStream, object> CreateFunc(FieldProperties props)
        {
            var bitCount = props.GetBitCountOrDefault(0);
            if ("qangle_pitch_yaw".Equals(props.EncoderType))
            {
                return f =>
                {
                    var unpacker = new QAnglePitchYawOnlyUnpacker(bitCount.GetValueOrDefault());
                    return unpacker.Unpack(f);
                };
            }

            if (bitCount == 0)
            {
                return f =>
                {
                    var unpacker = new QAngleNoBitCountUnpacker();
                    return unpacker.Unpack(f);
                };
            }

            if (bitCount == 32)
            {
                return f =>
                {
                    var unpacker = new QAngleNoScaleUnpacker();
                    return unpacker.Unpack(f);
                };
            }
            return f =>
                {
                    var unpacker = new QAngleNoBitCountUnpacker();
                    return unpacker.Unpack(f);
                };
        }
    }
}