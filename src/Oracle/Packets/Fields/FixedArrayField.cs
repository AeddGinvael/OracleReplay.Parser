using System;
using System.Collections.Generic;
using Oracle.BitStream;
using Oracle.BitStream.Utilities;
using Oracle.Packets.Unpacker.Factory;

namespace Oracle.Packets.Fields
{
    
    public class FixedArrayField: Field
    {
        private int Length;
        private Func<BitArrayStream, object> ElementUnpacker;
        
        public FixedArrayField(FieldProperties properties, int len) : base(properties)
        {
            this.Length = len;
            ElementUnpacker = UnpackerSource2Factory.CreateReaderFunc(properties, properties.Type.BaseType);
            
        }

        public override object GetInitialState()
        {
            return new object[Length];
        }

        public override void AccumulateName(FieldPath fp, int pos, List<string> parts)
        {
            AddBasePropertyName(parts);
            if (fp.Last > pos)
            {
                parts.Add(Utils.ArrayIdxToString(fp.Path[fp.Path[pos + 1]]));
                
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

            return Properties.Type.ElementType;
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
            if (fp.Last == pos)
            {
                throw new Exception("Can not be set");
            }

            object[] subState = (object[]) state[fp.Path[pos]];
            subState[fp.Path[pos + 1]] = data;
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
            for (var i = 0; i < subState.Length; i++)
            {
                if (subState[i] != null)
                {
                    fp.Path[fp.Last] = i;
                    var str = new string[] { namePrefix, Properties.Name, Utils.ArrayIdxToString(i)};
                    entries.Add(new DumpEntry(fp, JoinPropertyName(str), subState[i]));
                }
            }

            fp.Last--;
        }

        public override void CollectFieldPaths(FieldPath fp, List<FieldPath> entries, object[] state)
        {
            object[] subState = (object[]) state[fp.Path[fp.Last]];
            fp.Last++;
            for (var i = 0; i < subState.Length; i++)
            {
                if (subState[i] != null)
                {
                    fp.Path[fp.Last] = i;
                    entries.Add(new FieldPath());
                }
            }
            
        }

        public override Func<BitArrayStream, object> GetUnpackerForFieldPath(FieldPath fp, int pos)
        {
            if (fp.Last == pos)
            {
                return null;
            }

            return ElementUnpacker;
        }
    }
}