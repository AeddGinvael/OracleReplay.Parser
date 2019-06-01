using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace Oracle.Packets.Fields
{
    public class FieldType
    {
        private const string ElementCountPattern = @"(\[(.*?)\])+";
        private const string GenericTypePattern = @"(\<{0} .*?\ >{0})+";
        private const string ElementTypePattern = @"(\<{0} (.*?)<{0} (.*?)\ >)";
        private const string BasePattern = @"[a-zA-Z\d]+";


        public string BaseType { get; }
        public FieldType GenericType { get;  }
        public bool Pointer { get;  }
        public string ElementCount { get; }
        public FieldType ElementType { get; }

        public FieldType(string typeString)
        {
            var baseMatcher = Regex.Matches(typeString, BasePattern);
            var genericMatcher = Regex.Matches(typeString, GenericTypePattern);
            var elementCountMatcher = Regex.Matches(typeString, ElementCountPattern);

            if (baseMatcher.Count == 0)
            {
                throw new Exception("Cannot parse Field Type");
            }

            BaseType = baseMatcher[0].Value;
            GenericType = genericMatcher.Count != 0 ? new FieldType(baseMatcher[1].Value.Trim()) : null;
            ElementCount = elementCountMatcher.Count != 0 ? elementCountMatcher[0].Groups[2].ToString().Trim() : null;
            Pointer = false;

            if (ElementCount == null)
            {
                ElementType = null;
            }
            else
            {
                ElementType = new FieldType(ToString(true));
            }
        }
        
        

        public override string ToString()
        {
            return ToString(false);
        }

        private string ToString(bool omitElementCount)
        {
            var sb = new StringBuilder();
            sb.Append(BaseType);
            if (GenericType != null) {
                sb.Append("< ");
                sb.Append(GenericType);
                sb.Append(" >");
            }
            if (Pointer) {
                sb.Append('*');
            }
            if (!omitElementCount && ElementCount != null) {
                sb.Append('[');
                sb.Append(ElementCount);
                sb.Append(']');
            }
            return sb.ToString();
        }
    }
}