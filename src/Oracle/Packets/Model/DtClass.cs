using System;
using System.Collections.Generic;
using Oracle.Packets.Fields;

namespace Oracle.Packets.Model
{
    public interface DtClass
    {
        string GetDtName { get; }

        int GetClassId { get; }
        void SetClassId(int classId);

        object[] GetEmptyStateArray();

        string GetNameForFieldPath(FieldPath fp);
        FieldPath GetFieldPathForName(string property);

        T GetValueForFieldPath<T>(FieldPath fp, object[] state);

        List<FieldPath> CollectFieldPaths(object[] state);
    }

}