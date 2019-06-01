using System;
using System.Collections.Generic;
using System.Text;
using Oracle.BitStream;
using Oracle.Packets.Unpacker;

namespace Oracle.Packets.Fields
{
    public abstract class Field
    {
        public Func<BitArrayStream, object> Unpacker;
        public FieldProperties Properties { get; }

        public Field(FieldProperties properties)
        {
            Properties = properties;
        }

        public abstract object GetInitialState();
        public abstract void AccumulateName(FieldPath fp, int pos, List<string> parts);
        public abstract Field GetFieldForFieldPath(FieldPath fp, int pos);
        public abstract FieldType GetTypeForFieldPath(FieldPath fp, int pos);
        public abstract object GetValueForFieldPath(FieldPath fp, int pos, object[] state);
        public abstract void SetValueForFieldPath(FieldPath fp, int pos, object[] state, object data);
        public abstract FieldPath GetFieldPathForName(FieldPath fp, string property);
        public abstract void CollectDump(FieldPath fp, string namePrefix, List<DumpEntry> entries, object[] state);
        public abstract void CollectFieldPaths(FieldPath fp, List<FieldPath> entries, object[] state);
        
        public abstract Func<BitArrayStream, object> GetUnpackerForFieldPath(FieldPath fp, int pos);


        protected void AddBasePropertyName(List<string> parts)
        {
            parts.Add(Properties.Name);
        }

        protected string JoinPropertyName(string[] parts)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var part in parts)
            {
                if (builder.Length != 0)
                {
                    builder.Append('.');
                }

                builder.Append(part);
            }

            return builder.ToString();
        }

        protected object[] EnsureSubStateCapacity(object[] state, int i, int wantedSize, bool shrinkIfNeeded)
        {
            object[] subState = (object[]) state[i];

            if (wantedSize < 0)
            {
                return subState;
            }

            var growth = 0;
            var curSize = subState?.Length ?? 0;

            if (subState == null && wantedSize > 0)
            {
                state[i] = new object[wantedSize];
                growth = wantedSize;
            } else if (shrinkIfNeeded && wantedSize == 0)
            {
                state[i] = null;
            }
            else if (wantedSize != curSize)
            {
                if (shrinkIfNeeded || wantedSize > curSize)
                {
                    state[i] = new object[wantedSize];
                    curSize = wantedSize;
                }

                long min = Math.Min(subState.Length, curSize);
                Array.Copy(subState, 0, (Array)state[i], 0, min);
                
                growth = Math.Max(0, curSize - subState.Length);
            }
            
            if (growth > 0 && Properties.Serializer != null) {
                subState = (object[]) state[i];
                int j = subState.Length;
                while (growth-- > 0) {
                    subState[--j] = Properties.Serializer.GetInitialState();
                }
            }
            return (object[]) state[i];
        }

    }
}