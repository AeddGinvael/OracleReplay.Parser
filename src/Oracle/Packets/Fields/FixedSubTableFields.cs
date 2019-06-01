using System;
using System.Collections.Generic;
using System.Threading;
using Oracle.BitStream;
using Oracle.Packets.Unpacker;
using Oracle.Packets.Unpacker.Factory;

namespace Oracle.Packets.Fields
{
    public class FixedSubTableFields: Field
    {
        private Func<BitArrayStream, object> baseUnpacker;
        
        public FixedSubTableFields(FieldProperties properties) : base(properties)
        {
            baseUnpacker = UnpackerSource2Factory.CreateBoolFunc();
        }

        public override object GetInitialState()
        {
            return Properties.Serializer.GetInitialState();
        }

        public override void AccumulateName(FieldPath fp, int pos, List<string> parts)
        {
            AddBasePropertyName(parts);
            if (fp.Last > pos)
            {
                Properties.Serializer.AccumulateName(fp, pos + 1, parts);
            }
            
        }


        public override Field GetFieldForFieldPath(FieldPath fp, int pos)
        {
            if (fp.Last == pos)
            {
                return this;
            }
            return Properties.Serializer.GetFieldForFieldPath(fp, pos + 1);
            
        }

        public override FieldType GetTypeForFieldPath(FieldPath fp, int pos)
        {
            if (fp.Last == pos)
            {
                return Properties.Type;
            }

            return Properties.Serializer.GetTypeForFieldPath(fp, pos + 1);
        }

        public override object GetValueForFieldPath(FieldPath fp, int pos, object[] state)
        {
            int i = fp.Path[pos];
            object[] subState = (object[]) state[i];
            if (fp.Last == pos)
            {
                return subState != null;
            }

            return Properties.Serializer.GetValueForFieldPath(fp, pos + 1, subState);
        }

        public override void SetValueForFieldPath(FieldPath fp, int pos, object[] state, object data)
        {
            int i = fp.Path[pos];
            object[] subState = (object[]) state[i];
            if (fp.Last == pos)
            {
                bool existing = (bool) data;
                if (subState == null && existing)
                {
                    state[i] = Properties.Serializer.GetInitialState();
                } 
                else if (subState != null && !existing)
                {
                    state[i] = null;
                }
            }
            else
            {
                Properties.Serializer.SetValueForFieldPath(fp, pos + 1, subState, data);
            }
           
        }

        public override FieldPath GetFieldPathForName(FieldPath fp, string property)
        {
            return Properties.Serializer.GetFieldPathForName(fp, property);
        }

        public override void CollectDump(FieldPath fp, string namePrefix, List<DumpEntry> entries, object[] state)
        {
            object[] subState = (object[]) state[fp.Path[fp.Last]];
            var name = JoinPropertyName(new []{namePrefix, Properties.Name});
            if (subState != null)
            {
                fp.Last++;
                Properties.Serializer.CollectDump(fp, name, entries, subState);
                fp.Last--;
            }
        }

        public override void CollectFieldPaths(FieldPath fp, List<FieldPath> entries, object[] state)
        {
            object[] subState = (object[]) state[fp.Path[fp.Last]];
            if (subState != null)
            {
                fp.Last++;
                Properties.Serializer.CollectFieldPaths(fp, entries, subState);
                fp.Last--;
            }
        }

        public override Func<BitArrayStream, object> GetUnpackerForFieldPath(FieldPath fp, int pos)
        {
            if (fp.Last == pos)
            {
                return baseUnpacker;
            }

            return Properties.Serializer.GetUnpackerForFieldPath(fp, pos + 1);
        }
    }
}