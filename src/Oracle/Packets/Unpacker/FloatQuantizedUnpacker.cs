using System;
using System.Runtime.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using Oracle.BitStream;
using Utils = Oracle.BitStream.Utilities.Utils;

namespace Oracle.Packets.Unpacker
{
    public class FloatQuantizedUnpacker: Unpacker<float>
    {
        private const int QfeRoundDown = 0x1;
        private const int QfeRoundUp = 0x2;
        private const int QfeEncodeZeroExactly = 0x4;
        private const int QfeEncodeIntegerExactly = 0x8;

        private string fieldName;
        private int BitCount;
        private float MinVal;
        private float MaxVal;
        private int Flags;
        private int EncodeFlags;

        private float HighLowMultiplier;
        private float DecodeMultiplier;
        
        public FloatQuantizedUnpacker(string fieldName, int? bitCount, int? flags, float? minVal, float? maxVal)
        {
            this.fieldName = fieldName;
            this.BitCount = bitCount.GetValueOrDefault();
            this.MinVal = minVal.GetValueOrDefault();
            this.MaxVal = maxVal.GetValueOrDefault();
            this.Flags = flags.GetValueOrDefault();
            this.EncodeFlags = ComputeEncodeFlags(flags.GetValueOrDefault());
            Init();

        }

        private int ComputeEncodeFlags(int flags)
        {
            if ((MinVal == 0.0f && (flags & QfeRoundDown) != 0) || (MaxVal == 0.0f && (flags & QfeRoundUp) != 0))
            {
                flags &= ~QfeEncodeZeroExactly;
            }

            if (MinVal == 0.0f && (flags & QfeEncodeZeroExactly) != 0)
            {
                flags |= QfeRoundDown;
                flags &= ~QfeEncodeZeroExactly;
                
            }

            if (MaxVal == 0.0f && (flags & QfeEncodeZeroExactly) != 0)
            {
                flags |= QfeRoundDown;
                flags &= ~QfeEncodeZeroExactly;
                
            }

            if (!(MinVal < 0.0f && MaxVal > 0.0f))
            {
                if ((flags & QfeEncodeZeroExactly) != 0)
                {
                    
                }

                flags &= ~ QfeEncodeIntegerExactly;
            }

            if ((flags & QfeEncodeIntegerExactly) != 0)
            {
                flags &= ~(QfeRoundUp | QfeRoundDown | QfeEncodeZeroExactly);
            }

            return flags;
        }

        private void Init()
        {
            float offset;
            int quanta = (1 << BitCount);
            
            if ((Flags & (QfeRoundDown | QfeRoundUp)) == (QfeRoundDown | QfeRoundUp))
            {
                //round up and round down
            }

            if ((Flags & QfeRoundDown) != 0)
            {
                offset = ((MaxVal - MinVal) / quanta);
                MaxVal -= offset;
            } else if ((Flags & QfeRoundUp) != 0)
            {
                offset = ((MaxVal - MinVal) / quanta);
                MinVal += offset;
            }

            if ((Flags & QfeEncodeIntegerExactly) != 0)
            {
                int delta = ((int) MinVal) - ((int) MaxVal);
                int trueRange = (1 << Utils.CalculateBitsNeededFor(Math.Max(delta, 1)));

                int bits = this.BitCount;
                while ((1 << bits) < trueRange)
                {
                   ++bits;
                }

                if (bits > BitCount)
                {
                    BitCount = bits;
                    quanta = (1 << BitCount);
                }

                float floatRange = (float) trueRange;
                offset = (floatRange / (float)quanta);

                MaxVal = MinVal + floatRange - offset;
            }

            HighLowMultiplier = AssignRangeMultiplier(BitCount, MaxVal - MinVal);
            DecodeMultiplier = 1.0f / (quanta - 1);

            if (HighLowMultiplier == 0.0f)
            {
                throw new Exception("HighLowMultiplier is zero");
            }

            if ((EncodeFlags & QfeRoundDown) != 0)
            {
                if (Quantize(MaxVal) == MaxVal)
                {
                    EncodeFlags &= ~ QfeRoundUp;
                }
            }

            if ((EncodeFlags & QfeEncodeZeroExactly) != 0)
            {
                if (Quantize(0.0f) == 0.0f)
                {
                    EncodeFlags &= ~ QfeEncodeZeroExactly;
                }
            }
        }

        public float AssignRangeMultiplier(int bits, float range)
        {
            long highVal;
            if (bits == 32)
            {
                highVal = 0xFFFFFFFEL;
            }
            else
            {
                highVal = (long) BitStreamUtil.Masks[bits];
            }

            float highLowMul;
            if (Math.Abs(range) <= 0.001)
            {
                highLowMul = highVal;
            }
            else
            {
                highLowMul = highVal / range;
            }

            if ((long) (highLowMul * range) > highVal || (highLowMul * range) > (double) highVal)
            {
                float[] multipliers = { 0.9999f, 0.99f, 0.9f, 0.8f, 0.7f };
                int i;
                for (i = 0; i < multipliers.Length; i++)
                {
                    highLowMul = (highVal / range) * multipliers[i];
                    if ((long) (highLowMul * range) > highVal || (highLowMul * range) > (double) highVal)
                    {
                        continue;
                    }
                    break;
                }

                if (i == multipliers.Length)
                {
                    throw new Exception("We seem to be unable to represent this range.");
                }
            }

            return highLowMul;

        }

        public float Quantize(float val)
        {
            if (val < MinVal)
            {
                if ((Flags & QfeRoundUp) == 0)
                {
                    // out-of-range val
                }

                return MinVal;
            } else if (val > MaxVal)
            {
                if ((Flags & QfeRoundDown) == 0)
                {
                    // out of range val
                }

                return MaxVal;
            }

            int i = (int) ((val - MinVal) * HighLowMultiplier);
            return MinVal + (MaxVal - MinVal) * ((float) i * DecodeMultiplier);

        }

        public float Unpack(BitArrayStream stream)
        {
            // Was ReadBit()
            if ((EncodeFlags & QfeRoundDown) != 0 && stream.ReadBitFlag())
            {
                return MinVal;
            }
            
            if ((EncodeFlags & QfeRoundUp) != 0 && stream.ReadBitFlag())
            {
                return MaxVal;
            }
            
            if ((EncodeFlags & QfeEncodeZeroExactly) != 0 && stream.ReadBit())
            {
                return 0.0f;
            }

            //float v = stream.ReadUBitInt(BitCount) * DecodeMultiplier;
            float v = stream.ReadInt(BitCount);
            float res = v * DecodeMultiplier;
            return MinVal + (MaxVal - MinVal) * res;
        }
    }
}