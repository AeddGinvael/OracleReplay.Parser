using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Oracle.BitStream;
using Oracle.BitStream.Utilities;
using Oracle.Packets.Unpacker.Factory;

namespace Oracle.Packets.Fields
{
  
    public class VarArrayField: Field
    {
        private Func<BitArrayStream, object> BaseUnpacker;
        private Func<BitArrayStream, object> ElementUnpacker;
        public VarArrayField(FieldProperties properties) : base(properties)
        {
            BaseUnpacker = UnpackerSource2Factory.CreateReaderFunc(properties, "uint32");
            ElementUnpacker = UnpackerSource2Factory.CreateReaderFunc(properties, properties.Type.GenericType.BaseType);
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
            }
        }

        public override Field GetFieldForFieldPath(FieldPath fp, int pos)
        {
            return this;
        }

        public override FieldType GetTypeForFieldPath(FieldPath fp, int pos)
        {
            if (fp.Last == pos)
            {
                return Properties.Type;
                
            }

            return Properties.Type.GenericType;

        }

        public override object GetValueForFieldPath(FieldPath fp, int pos, object[] state)
        {
            object[] subState = (object[]) state[fp.Path[pos]];
            if (fp.Last == pos)
            {
                return subState.Length;
            }

            return subState[fp.Path[pos + 1]];
        }

        public override void SetValueForFieldPath(FieldPath fp, int pos, object[] state, object data)
        {
            int i = fp.Path[pos];
            if (fp.Last == pos)
            {
                EnsureSubStateCapacity(state, i, int.Parse((string)data), true);
            }

            int j = fp.Path[pos + 1];
            object[] subState = EnsureSubStateCapacity(state, i, j + 1, false);
            subState[j] = data;
        }

        public override FieldPath GetFieldPathForName(FieldPath fp, string property)
        {
            if (property.Length != 4)
            {
                throw new Exception("unresolvable fieldpath");
            }

            fp.Path[fp.Last] = int.Parse(property);
            return fp;
        }

        public override void CollectDump(FieldPath fp, string namePrefix, List<DumpEntry> entries, object[] state)
        {
            object[] subState = (object[]) state[fp.Path[fp.Last]];
            fp.Last++;
            for (int i = 0; i < subState.Length; i++)
            {
                if(subState[i] != null)
                {
                    fp.Path[fp.Last] = i;
                    var str = new[] {namePrefix, Properties.Name, Utils.ArrayIdxToString(i) };
                    entries.Add(new DumpEntry(fp, JoinPropertyName(str), subState[i]));
                }
            }

            fp.Last--;
        }

        public override void CollectFieldPaths(FieldPath fp, List<FieldPath> entries, object[] state)
        {
            object[] subState = (object[]) state[fp.Path[fp.Last]];
            fp.Last++;
            for (int i = 0; i < subState.Length; i++)
            {
                if (subState[i] != null)
                {
                    fp.Path[fp.Last] = i;
                    entries.Add(new FieldPath(fp));
                }
            }

            fp.Last--;
        }

        public override Func<BitArrayStream, object> GetUnpackerForFieldPath(FieldPath fp, int pos)
        {
            if (pos == fp.Last)
            {
                return BaseUnpacker;
            }

            return ElementUnpacker;
        }
    }
}