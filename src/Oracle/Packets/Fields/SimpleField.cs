using System.Collections;
using System;
using System.Collections.Generic;
using Oracle.Packets.Unpacker.Factory;
using Oracle.BitStream;

namespace Oracle.Packets.Fields
{
    public class SimpleField: Field
    {
        private Func<BitArrayStream, object> Unpacker;
        
        public SimpleField(FieldProperties properties) : base(properties)
        {
            Unpacker = UnpackerSource2Factory.CreateReaderFunc(properties, properties.Type.BaseType);
        }

        public override object GetInitialState()
        {
            return null;
        }

        public override void AccumulateName(FieldPath fp, int pos, List<string> parts)
        {
            AddBasePropertyName(parts);
        }

        public override Field GetFieldForFieldPath(FieldPath fp, int pos)
        {
            return this;
        }

        public override FieldType GetTypeForFieldPath(FieldPath fp, int pos)
        {
            return Properties.Type;
        }

        public override object GetValueForFieldPath(FieldPath fp, int pos, object[] state)
        {
            return state[fp.Path[pos]];
        }

        public override void SetValueForFieldPath(FieldPath fp, int pos, object[] state, object data)
        {
            state[fp.Path[pos]] = data;
        }

        public override FieldPath GetFieldPathForName(FieldPath fp, string property)
        {
            throw new NotImplementedException();
        }

        public override void CollectDump(FieldPath fp, string namePrefix, List<DumpEntry> entries, object[] state)
        {
            var str = new[] {namePrefix, Properties.Name};
            entries.Add(new DumpEntry(fp, JoinPropertyName(str), state[fp.Path[fp.Last]]));
        }

        public override void CollectFieldPaths(FieldPath fp, List<FieldPath> entries, object[] state)
        {
            entries.Add(new FieldPath(fp));
        }

        public override Func<BitArrayStream, object> GetUnpackerForFieldPath(FieldPath fp, int pos)
        {
            return Unpacker;
        }
    }
}