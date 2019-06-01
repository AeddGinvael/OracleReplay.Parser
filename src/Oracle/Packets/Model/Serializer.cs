using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oracle.BitStream;
using Oracle.Packets.Fields;

namespace Oracle.Packets.Model
{
    public class Serializer
    {
        public SerializerId Id { get; }
        public Field[] Fields { get; }

        private HashSet<string> SendNodePrefix = new HashSet<string>();
 
        public Serializer(SerializerId Id, Field[] Fields)
        {
            this.Id = Id;
            this.Fields = Fields;

            foreach (var field in Fields)
            {
                if (field.Properties.SendNode != null)
                {
                    SendNodePrefix.Add(field.Properties.SendNode);
                }
            }
            SendNodePrefix = SendNodePrefix.OrderByDescending(s => s.Length).ToHashSet();
        }

        public object[] GetInitialState()
        {
            object[] result = new object[Fields.Length];

            for (int i = 0; i < Fields.Length; i++)
            {
                result[i] = Fields[i].GetInitialState();
            }

            return result;
        }

        public void AccumulateName(FieldPath fp, int pos, List<string> parts) {
            Fields[fp.Path[pos]].AccumulateName(fp, pos, parts);
        }
    
        public Func<BitArrayStream, object> GetUnpackerForFieldPath(FieldPath fp, int pos) {
            return Fields[fp.Path[pos]].GetUnpackerForFieldPath(fp, pos);
        }
    
        public Field GetFieldForFieldPath(FieldPath fp, int pos) {
            return Fields[fp.Path[pos]].GetFieldForFieldPath(fp, pos);
        }
    
        public FieldType GetTypeForFieldPath(FieldPath fp, int pos) {
            return Fields[fp.Path[pos]].GetTypeForFieldPath(fp, pos);
        }
    
        public object GetValueForFieldPath(FieldPath fp, int pos, object[] state) {
            return Fields[fp.Path[pos]].GetValueForFieldPath(fp, pos, state);
        }
    
        public void SetValueForFieldPath(FieldPath fp, int pos, object[] state, object data) {
            Fields[fp.Path[pos]].SetValueForFieldPath(fp, pos, state, data);
        }
    
        private FieldPath GetFieldPathForNameInternal(FieldPath fp, string property) {
            for (int i = 0; i < Fields.Length; i++) {
                Field field = Fields[i];
                string fieldName = field.Properties.Name;
                if (property.StartsWith(fieldName)) {
                    fp.Path[fp.Last] = i;
                    if (property.Length == fieldName.Length) {
                        return fp;
                    } else {
                        if (property[fieldName.Length] != '.') {
                            continue;
                        }
                        property = property.Substring(fieldName.Length + 1);
                        fp.Last++;
                        return field.GetFieldPathForName(fp, property);
                    }
                }
            }
            return null;
        }
    
        public FieldPath GetFieldPathForName(FieldPath fp, string property) {
            return GetFieldPathForNameInternal(fp, property);
        }
    
        public void CollectDump(FieldPath fp, string namePrefix, List<DumpEntry> entries, object[] state) {
            for (int i = 0; i < Fields.Length; i++) {
                if (state[i] != null) {
                    fp.Path[fp.Last] = i;
                    Fields[i].CollectDump(fp, namePrefix, entries, state);
                }
            }
        }
    
        public void CollectFieldPaths(FieldPath fp, List<FieldPath> entries, object[] state) {
            for (int i = 0; i < Fields.Length; i++) {
                if (state[i] != null) {
                    fp.Path[fp.Last] = i;
                    Fields[i].CollectFieldPaths(fp, entries, state);
                }
            }
        }
    }
}