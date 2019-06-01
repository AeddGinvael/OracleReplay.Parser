using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json.Bson;
using Oracle.BitStream;

namespace Oracle.Packets.Fields
{
    public abstract class FieldOpType
    {
        public int ordinal { get; set; } = -1;
        public FieldOpType(int weight)
        {
            Weight = weight;
        }

        public static FieldOpType[] GetValues()
        {
            return new FieldOpType[]
            {
                new PlusOne(),
                new PlusTwo(),
                new PlusThree(),
                new PlusFour(),
                new PlusN(),
                new PushOneLeftDeltaZeroRightZero(),
                new PushOneLeftDeltaZeroRightNonZero(),
                new PushOneLeftDeltaOneRightZero(),
                new PushOneLeftDeltaOneRightNonZero(),
                new PushOneLeftDeltaNRightZero(),
                new PushOneLeftDeltaNRightNonZero(),
                new PushOneLeftDeltaNRightNonZeroPack6Bits(),
                new PushOneLeftDeltaNRightNonZeroPack8Bits(),
                new PushTwoLeftDeltaZero(),
                new PushTwoPack5LeftDeltaZero(),
                new PushThreeLeftDeltaZero(),
                new PushThreePack5LeftDeltaZero(),
                new PushTwoLeftDeltaOne(),
                new PushTwoPack5LeftDeltaOne(),
                new PushThreeLeftDeltaOne(),
                new PushThreePack5LeftDeltaOne(),
                new PushTwoLeftDeltaN(),
                new PushTwoPack5LeftDeltaN(),
                new PushThreeLeftDeltaN(),
                new PushThreePack5LeftDeltaN(),
                new PushN(),
                new PushNAndNonTopographical(),
                new PopOnePlusOne(),
                new PopOnePlusN(),
                new PopAllButOnePlusOne(),
                new PopAllButOnePlusN(),
                new PopAllButOnePlusNPack3Bits(),
                new PopAllButOnePlusNPack6Bits(),
                new PopNPlusOne(),
                new PopNPlusN(),
                new PopNAndNonTopographical(),
                new NonTopoComplex(),
                new NonTopoPenultimatePluseOne(),
                new NonTopoComplexPack4Bits(),
                new FieldPathEncodeFinish()
            };
        }

        public static int Ordinal(FieldOpType op)
        {
            var classes = GetValues();
            var index = 0;
            for (var i = 0; i < GetValues().Length; i++)
            {
                var opCls = classes[i];
                Console.WriteLine(opCls.Weight);
                if (opCls == op)
                {
                    index = i;
                }

            }
            return index;

        }
        public int Weight { get; }

        public abstract void Execute(FieldPath fp, BitArrayStream stream);
    }

    public class PlusOne : FieldOpType
    {

        public PlusOne() : base(36271)
        {
            ordinal = 0;
        }
        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last] += 1;
        }
    }

    public class PlusTwo : FieldOpType
    {
        public PlusTwo() : base(10334){ordinal = 1;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last] += 2;
        }
    }

    public class PlusThree : FieldOpType
    {
        public PlusThree() : base(1375){ordinal = 2;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last] += 3;
        }
    }
    
    public class PlusFour : FieldOpType
    {
        public PlusFour() : base(646)
        {ordinal = 3;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last] += 4;
        }
    }
    
    public class PlusN : FieldOpType
    {
        public PlusN() : base(4128)
        {
            ordinal = 4;
        }

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last] += stream.ReadUBitVarFieldPath() + 5;
        }
    }
    
    public class PushOneLeftDeltaZeroRightZero : FieldOpType
    {
        public PushOneLeftDeltaZeroRightZero() : base(35){ordinal = 5;}
        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[++fp.Last] = 0;
        }
    }
    
    public class PushOneLeftDeltaZeroRightNonZero : FieldOpType
    {
        public PushOneLeftDeltaZeroRightNonZero() : base(3){ordinal = 6;}
        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[++fp.Last] = stream.ReadUBitVarFieldPath();
        }
    }
    
    public class PushOneLeftDeltaOneRightZero : FieldOpType
    {
        public PushOneLeftDeltaOneRightZero() : base(521){ordinal = 7;}
        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last]++;
            fp.Path[++fp.Last] = 0;
        }
    }
    
    public class PushOneLeftDeltaOneRightNonZero : FieldOpType
    {
        public PushOneLeftDeltaOneRightNonZero() : base(2942){ordinal = 8;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last]++;
            fp.Path[++fp.Last] = stream.ReadUBitVarFieldPath();
        }
    }
    
    public class PushOneLeftDeltaNRightZero : FieldOpType
    {
        public PushOneLeftDeltaNRightZero() : base(560){ordinal = 9;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last] += stream.ReadUBitVarFieldPath();
            fp.Path[++fp.Last] = 0;
        }
    }
    
    public class PushOneLeftDeltaNRightNonZero : FieldOpType
    {
        public PushOneLeftDeltaNRightNonZero() : base(471){ordinal = 10;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last] += (int)stream.ReadInt(3) + 2;
            fp.Path[++fp.Last] = (int)stream.ReadInt(3) + 1;
        }
    }
    
    public class PushOneLeftDeltaNRightNonZeroPack6Bits : FieldOpType
    {
        public PushOneLeftDeltaNRightNonZeroPack6Bits() : base(10530){ordinal = 11;}
        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last] += (int)stream.ReadInt(3) + 2;
            fp.Path[++fp.Last] = (int)stream.ReadInt(3) + 1;
        }
    }
    
    public class PushOneLeftDeltaNRightNonZeroPack8Bits : FieldOpType
    {
        public PushOneLeftDeltaNRightNonZeroPack8Bits() : base(251){ordinal = 12;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last] += (int)stream.ReadInt(4) + 2;
            fp.Path[++fp.Last] = (int)stream.ReadInt(4) + 1;
        }
    }
    
    public class PushTwoLeftDeltaZero : FieldOpType
    {
        public PushTwoLeftDeltaZero() : base(0){ordinal = 13;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[++fp.Last] += stream.ReadUBitVarFieldPath();
            fp.Path[++fp.Last] += stream.ReadUBitVarFieldPath();
        }
    }
    
    public class PushTwoPack5LeftDeltaZero : FieldOpType
    {
        public PushTwoPack5LeftDeltaZero() : base(0){ordinal = 14;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[++fp.Last] = (int)stream.ReadInt(5);
            fp.Path[++fp.Last] = (int) stream.ReadInt(5);
        }
    }
    
    public class PushThreeLeftDeltaZero : FieldOpType
    {
        public PushThreeLeftDeltaZero() : base(0){ordinal = 15;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[++fp.Last] += stream.ReadUBitVarFieldPath();
            fp.Path[++fp.Last] += stream.ReadUBitVarFieldPath();
            fp.Path[++fp.Last] += stream.ReadUBitVarFieldPath();
        }
    }
    
    public class PushThreePack5LeftDeltaZero : FieldOpType
    {
        public PushThreePack5LeftDeltaZero() : base(0){ordinal = 16;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[++fp.Last] = (int)stream.ReadInt(5);
            fp.Path[++fp.Last] = (int)stream.ReadInt(5);
            fp.Path[++fp.Last] = (int)stream.ReadInt(5);
        }
    }
    
    public class PushTwoLeftDeltaOne : FieldOpType
    {
        public PushTwoLeftDeltaOne() : base(0){ordinal = 17;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last]++;
            fp.Path[++fp.Last] += stream.ReadUBitVarFieldPath();
            fp.Path[++fp.Last] += stream.ReadUBitVarFieldPath();
        }
    }
    
    public class PushTwoPack5LeftDeltaOne : FieldOpType
    {
        public PushTwoPack5LeftDeltaOne() : base(0){ordinal = 18;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last]++;
            fp.Path[++fp.Last] += (int)stream.ReadInt(5);
            fp.Path[++fp.Last] += (int)stream.ReadInt(5);
        }
    }
    
    public class PushThreeLeftDeltaOne : FieldOpType
    {
        public PushThreeLeftDeltaOne() : base(0){ordinal = 19;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last]++;
            fp.Path[++fp.Last] += stream.ReadUBitVarFieldPath();
            fp.Path[++fp.Last] += stream.ReadUBitVarFieldPath();
            fp.Path[++fp.Last] += stream.ReadUBitVarFieldPath();
        }
    }
    
    public class PushThreePack5LeftDeltaOne : FieldOpType
    {
        public PushThreePack5LeftDeltaOne() : base(0){ordinal = 20;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last]++;
            fp.Path[++fp.Last] += (int)stream.ReadInt(5);
            fp.Path[++fp.Last] += (int)stream.ReadInt(5);
            fp.Path[++fp.Last] += (int)stream.ReadInt(5);
        }
    }
    
    public class PushTwoLeftDeltaN : FieldOpType
    {
        public PushTwoLeftDeltaN() : base(0){ordinal = 21;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last] += (int)stream.ReadUBitVar() + 2;
            fp.Path[++fp.Last] += (int) stream.ReadInt(5);
            fp.Path[++fp.Last] += (int) stream.ReadInt(5);
        }
    }
    
    public class PushTwoPack5LeftDeltaN : FieldOpType
    {
        public PushTwoPack5LeftDeltaN() : base(0){ordinal = 22;}
        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last] += (int)stream.ReadUBitVar() + 2;
            fp.Path[++fp.Last] += (int)stream.ReadInt(5);
            fp.Path[++fp.Last] += (int)stream.ReadInt(5);
        }
    }
    
    public class PushThreeLeftDeltaN : FieldOpType
    {
        public PushThreeLeftDeltaN() : base(0){ordinal = 23;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last] += (int)stream.ReadUBitVar() + 2;
            fp.Path[++fp.Last] += stream.ReadUBitVarFieldPath();
            fp.Path[++fp.Last] += stream.ReadUBitVarFieldPath();
            fp.Path[++fp.Last] += stream.ReadUBitVarFieldPath();
        }
    }
    
    public class PushThreePack5LeftDeltaN : FieldOpType
    {
        public PushThreePack5LeftDeltaN() : base(0){ordinal = 24;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last] += (int)stream.ReadUBitVar() + 2;
            fp.Path[++fp.Last] += (int)stream.ReadInt(5);
            fp.Path[++fp.Last] += (int)stream.ReadInt(5);
            fp.Path[++fp.Last] += (int)stream.ReadInt(5);
        }
    }
    
    public class PushN : FieldOpType
    {
        public PushN() : base(0){ordinal = 25;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            int n = (int)stream.ReadUBitVar();
            fp.Path[fp.Last] += (int)stream.ReadUBitVar();
            for (int i = 0; i < n; i++)
            {
                fp.Path[++fp.Last] += stream.ReadUBitVarFieldPath();
            }
        }
    }
    
    public class PushNAndNonTopographical : FieldOpType
    {
        public PushNAndNonTopographical() : base(310){ordinal = 26;}
        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            for (int i = 0; i <= fp.Last; i++)
            {
                if (stream.ReadBit())
                {
                    fp.Path[i] += (int)stream.ReadSignedVarInt() + 1;
                }
            }

            int c = (int)stream.ReadUBitVar();
            for (int i = 0; i < c; i++)
            {
                fp.Path[++fp.Last] = stream.ReadUBitVarFieldPath();
            }
        }
    }
    
    public class PopOnePlusOne : FieldOpType
    {
        public PopOnePlusOne() : base(2){ordinal = 27;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[--fp.Last]++;
        }
    }
    
    public class PopOnePlusN : FieldOpType
    {
        public PopOnePlusN() : base(0){ordinal = 28;}
        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[--fp.Last] += stream.ReadUBitVarFieldPath() + 1;
        }
    }
    
    public class PopAllButOnePlusOne : FieldOpType
    {
        public PopAllButOnePlusOne() : base(1837){ordinal = 29;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Last = 0;
            fp.Path[0]++;
        }
    }
    
    public class PopAllButOnePlusN : FieldOpType
    {
        public PopAllButOnePlusN() : base(149){ordinal = 30;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Last = 0;
            fp.Path[0] += stream.ReadUBitVarFieldPath() + 1;
        }
    }
    
    public class PopAllButOnePlusNPack3Bits : FieldOpType
    {
        public PopAllButOnePlusNPack3Bits() : base(300){ordinal = 31;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Last = 0;
            fp.Path[0] += (int)stream.ReadInt(3) + 1;
        }
    }
    
    public class PopAllButOnePlusNPack6Bits : FieldOpType
    {
        public PopAllButOnePlusNPack6Bits() : base(634){ordinal = 32;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Last = 0;
            fp.Path[0] += (int)stream.ReadInt(6) + 1;
        }
    }
    
    public class PopNPlusOne : FieldOpType
    {
        public PopNPlusOne() : base(0){ordinal = 33;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Last -= stream.ReadUBitVarFieldPath();
            fp.Path[fp.Last]++;
        }
    }
    
    public class PopNPlusN : FieldOpType
    {
        public PopNPlusN() : base(0){ordinal = 34;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Last -= stream.ReadUBitVarFieldPath();
            fp.Path[fp.Last] += (int)stream.ReadSignedVarInt();
        }
    }
    
    public class PopNAndNonTopographical : FieldOpType
    {
        public PopNAndNonTopographical() : base(1){ordinal = 35;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Last -= stream.ReadUBitVarFieldPath();
            for (int i = 0; i <= fp.Last; i++)
            {
                if (stream.ReadBit())
                {
                    fp.Path[i] += (int)stream.ReadSignedVarInt();
                }
            }
        }
    }
    
    public class NonTopoComplex : FieldOpType
    {
        public NonTopoComplex() : base(76){ordinal = 36;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            for (int i = 0; i <= fp.Last; i++)
            {
                if (stream.ReadBit())
                {
                    fp.Path[i] += (int)stream.ReadSignedVarInt();
                }
            }
        }
    }
    
    public class NonTopoPenultimatePluseOne : FieldOpType
    {
        public NonTopoPenultimatePluseOne() : base(271){ordinal = 37;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            fp.Path[fp.Last - 1]++;
        }
    }
    
    public class NonTopoComplexPack4Bits : FieldOpType
    {
        public NonTopoComplexPack4Bits() : base(99){ordinal = 38;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
            for (int i = 0; i <= fp.Last; i++)
            {
                if (stream.ReadBit())
                {
                    fp.Path[i] += (int)stream.ReadInt(4) - 7;
                }
            }
        }
    }
    public class FieldPathEncodeFinish : FieldOpType
    {
        public FieldPathEncodeFinish() : base(25474){ordinal = 39;}

        public override void Execute(FieldPath fp, BitArrayStream stream)
        {
        }
    }
}
