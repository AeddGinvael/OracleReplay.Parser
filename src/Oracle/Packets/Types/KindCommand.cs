using System;
using System.IO;
using Proto.Dota;

namespace Oracle.Packets.Types
{
    public struct KindCommand
    {
        /// <summary>
        /// Mask for source 2 engine type.
        /// </summary>
        public const int CompressMask = (int) EDemoCommands.DemIsCompressedS2;
        /// <summary>
        /// Raw kind.
        /// From this param we should know is data compressed.
        /// </summary>
        private int RawKind { get; }
        /// <summary>
        /// Compressed kind, or may no be compressed.
        /// </summary>
        public int Kind { get; }

        public DemoCommandsEnum Type { get; }

        public KindCommand(uint kind)
        {
            RawKind = (int) kind;
            Kind = RawKind & ~ CompressMask;
            Type = (DemoCommandsEnum) Kind;
            Validate();
        }

        public KindCommand(int kind)
        {
            RawKind = kind;
            Kind = RawKind & ~ CompressMask;
            Type = (DemoCommandsEnum) Kind;
            Validate();
        }

        public bool IsDataCompressed() => (RawKind & CompressMask) == CompressMask;
        public static implicit operator KindCommand(uint kind)
        {
            return new KindCommand(kind);
        }

        public static implicit operator KindCommand(int kind)
        {
            return new KindCommand(kind);
        }

        private void Validate()
        {
            if (!Enum.IsDefined(typeof(DemoCommandsEnum), (sbyte) Kind))
            {
               // In replay cannot be unknown dem commands
               // If there is unknown commands so something going wrong while parsing
               throw new InvalidDataException($"Unknown type of EDemoCommands: {Kind}");
            }
        }

    }
}