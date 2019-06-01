using System;
using System.Collections.Generic;
using Oracle.BitStream;
using Oracle.BitStream.Utilities;
using Oracle.Packets.Unpacker;
using Oracle.Packets.Unpacker.Factory;

namespace Oracle.Packets.Fields
{
    public class VarSubTableField: Field
    {
        private Func<BitArrayStream, object> baseUnpacker;


        public VarSubTableField(FieldProperties properties) : base(properties)
        {
            baseUnpacker = UnpackerSource2Factory.CreateUInt32Func();
        }

        public override object GetInitialState()
        {
            return null;
        }

        public override void AccumulateName(FieldPath fp, int pos, List<string> parts)
        {
            AddBasePropertyName(parts);
            if (fp.Last > pos)
            {
                parts.Add(Utils.ArrayIdxToString(fp.Path[pos + 1]));
                if (fp.Last > pos + 1)
                {
                    Properties.Serializer.AccumulateName(fp, pos + 2, parts);
                }
            }
        }

        public override Field GetFieldForFieldPath(FieldPath fp, int pos)
        {
            if (fp.Last == pos)
            {
                return this;
            }

            return Properties.Serializer.GetFieldForFieldPath(fp, pos + 2);
        }

        public override FieldType GetTypeForFieldPath(FieldPath fp, int pos)
        {
            if (fp.Last == pos)
            {
                return Properties.Type;
            }

            return Properties.Serializer.GetTypeForFieldPath(fp, pos + 2);
        }

        public override object GetValueForFieldPath(FieldPath fp, int pos, object[] state)
        {
            object[] subState = (object[]) state[fp.Path[pos]];
            if (fp.Last == pos) {
                return subState.Length;
            } else {
                return Properties.Serializer.GetValueForFieldPath(fp, pos + 2, (object[]) subState[fp.Path[pos + 1]]);
            }
        }

        public override void SetValueForFieldPath(FieldPath fp, int pos, object[] state, object data)
        {
            int i = fp.Path[pos];
            if (fp.Last == pos) {
                EnsureSubStateCapacity(state, i, (int) data, true);
            } else {
                int j = fp.Path[pos + 1];
                object[] subState = EnsureSubStateCapacity(state, i, j + 1, false);
                Properties.Serializer.SetValueForFieldPath(fp, pos + 2, (object[]) subState[j], data);
            }
        }

        public override FieldPath GetFieldPathForName(FieldPath fp, string property)
        {
            if (property.Length < 5) {
                throw new Exception("unresolvable fieldpath");
            }
            String idx = property.Substring(0, 4);
            fp.Path[fp.Last] = int.Parse(idx);
            fp.Last++;
            return Properties.Serializer.GetFieldPathForName(fp, property.Substring(5));
        }

        public override void CollectDump(FieldPath fp, string namePrefix, List<DumpEntry> entries, object[] state)
        {
            Object[] subState = (Object[]) state[fp.Path[fp.Last]];
            String name = JoinPropertyName(new[] {namePrefix, Properties.Name});
            if (subState.Length > 0) {
                entries.Add(new DumpEntry(fp, name, subState.Length));
                fp.Last += 2;
                for (int i = 0; i < subState.Length; i++) {
                    fp.Path[fp.Last - 1] = i;
                    Properties.Serializer.CollectDump(fp, JoinPropertyName(new[] {name, Utils.ArrayIdxToString(i)}), entries, (Object[])subState[i]);
                }
                fp.Last -= 2;
            }
        }

        public override void CollectFieldPaths(FieldPath fp, List<FieldPath> entries, object[] state)
        {
            Object[] subState = (Object[]) state[fp.Path[fp.Last]];
            if (subState.Length > 0) {
                fp.Last += 2;
                for (int i = 0; i < subState.Length; i++) {
                    fp.Path[fp.Last - 1] = i;
                    Properties.Serializer.CollectFieldPaths(fp, entries, (Object[])subState[i]);
                }
                fp.Last -= 2;
            }
        }

        public override Func<BitArrayStream, object> GetUnpackerForFieldPath(FieldPath fp, int pos)
        {
            if (fp.Last == pos)
            {
                return baseUnpacker;
            }

            return Properties.Serializer.GetUnpackerForFieldPath(fp, pos + 2);
        }
    }
}