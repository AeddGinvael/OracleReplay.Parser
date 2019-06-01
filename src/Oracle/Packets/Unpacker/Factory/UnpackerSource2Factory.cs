using System;
using System.Collections;
using System.Collections.Generic;
using Oracle.BitStream;
using Oracle.Packets.Fields;

namespace Oracle.Packets.Unpacker.Factory
{
    public static class UnpackerSource2Factory
    {
        private static readonly Dictionary<string, Func<BitArrayStream, object>> UnpackerFunc = new Dictionary<string, Func<BitArrayStream, object>>
        {
            //Bool
            {"bool", s => s.ReadBit() },

            // Unsigned ints
            {"uint8", s => (int)s.ReadVarInt() },
            {"uint16", s => (int)s.ReadVarInt() },
            {"uint32", s => (int)s.ReadVarInt() },

            // Signed ints
            {"int8", s => (int)s.ReadSignedVarInt() },
            {"int16", s => (int)s.ReadSignedVarInt() },
            {"int32", s => (int)s.ReadSignedVarInt() },
            {"int64", s => s.ReadVarSLong()},

            // Strings
            {"CUtlSymbolLarge", (s) => s.ReadString() },
            {"char", s => s.ReadString() },
            {"CUtlString", s => s.ReadString() },
            {"CUtlStringToken", s => (int)s.ReadVarInt()},

            // handles
            {"CHandle", s => (int)s.ReadVarInt() },
            {"CEntityHandle", s => (int)s.ReadVarInt() },
            {"CGameSceneNodeHandle", s => (int)s.ReadVarInt()},
            {"CBaseVRHandAttachmentHandle", (s) => (int)s.ReadVarInt() },
            {"CStrongHandle", s => s.ReadVarULong() },

            // Colors
            {"Color", s => (int)s.ReadVarInt() },
            {"color32", s => (int)s.ReadVarInt() },

            // Specials
            {"HSequence", s => (int)s.ReadVarInt() }
        };


        private static readonly Dictionary<string, Func<FieldProperties, Func<BitArrayStream, object>>> FactoryFunc =
                new Dictionary<string, Func<FieldProperties, Func<BitArrayStream, object>>>
        {
            //Unsigned ints
            {"uint64", f => new LongUnsignedUnpackerFactory().CreateFunc(f) },
            // Floats
            {"float32", f => new FloatUnpackerFactory().CreateFunc(f) },
            {"CNetworkedQuantizedFloat", f => new FloatUnpackerFactory().CreateFunc(f) },
            {"QAngle", f => new QAngleUnpackerFactory().CreateFunc(f) },
            {"Vector2D", f => new VectorUnpackerFactory(2).CreateFunc(f)}, // 2
            {"Vector", f => new VectorUnpackerFactory(3).CreateFunc(f)}, // 3
            {"Vector4D", f => new VectorUnpackerFactory(4).CreateFunc(f)}, // 4
            {"Quaternion", f => new VectorUnpackerFactory(4).CreateFunc(f)}   // 4
        };

        public static Func<BitArrayStream, object> CreateBoolFunc() => s => s.ReadBit();
        public static Func<BitArrayStream, object> CreateUInt32Func() => s => (int)s.ReadVarInt();

        private static Func<BitArrayStream, object> DefaultFuncUnpacker => s => (int)s.ReadVarInt();

        public static Func<BitArrayStream, object> CreateReaderFunc(FieldProperties fieldProperties, string type)
        {
            FactoryFunc.TryGetValue(type, out var factory);

            if (factory != null)
            {
                return factory(fieldProperties);
            }

            UnpackerFunc.TryGetValue(type, out var unpacker);
            if (unpacker == null)
            {
                unpacker = DefaultFuncUnpacker;
            }

            return unpacker;

        }

    }
}