using System;
using System.Collections.Generic;
using System.Text;
using Oracle.BitStream;
using Oracle.Packets.Fields;

namespace Oracle.Packets.Model
{
    public class S2DTClass: DtClass
    {

        private Serializer serializer;
        private int classId = -1;

        public S2DTClass(Serializer serializer)
        {
            this.serializer = serializer;
        }
        public string GetDtName { get => serializer.Id.Name; }

        public int GetClassId { get => classId; }

        public void SetClassId(int classId)
        {
            this.classId = classId;
        }

        public object[] GetEmptyStateArray()
        {
            return serializer.GetInitialState();
        }

        public Serializer GetSerializer()
        {
            return serializer;
        }

        public string GetNameForFieldPath(FieldPath fp)
        {
           var parts = new List<string>();
           serializer.AccumulateName(fp, 0, parts);
           var builder = new StringBuilder();
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

        public Func<BitArrayStream, object> GetUnpackerForFieldPath(FieldPath fp)
        {
            return serializer.GetUnpackerForFieldPath(fp, 0);
        }

        public Field GetFieldForFieldPath(FieldPath fp)
        {
            return serializer.GetFieldForFieldPath(fp, 0);
        }

        public FieldType GetTypeForFieldPath(FieldPath fp)
        {
            return serializer.GetTypeForFieldPath(fp, 0);
        }

        public FieldPath GetFieldPathForName(string property)
        {
            FieldPath fp = new FieldPath();
            return serializer.GetFieldPathForName(fp, property);
        }

        public T GetValueForFieldPath<T>(FieldPath fp, object[] state)
        {
            throw new System.NotImplementedException();
        }

        public List<FieldPath> CollectFieldPaths(object[] state)
        {
            var result = new List<FieldPath>(state.Length);
            serializer.CollectFieldPaths(new FieldPath(), result, state);
            return result;
        }

        public void SetValuesForFieldPath(FieldPath fp, object[] state, object val)
        {
            serializer.SetValueForFieldPath(fp, 0, state, val);
        }


    }
}