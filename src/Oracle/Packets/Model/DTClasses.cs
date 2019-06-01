using System.Collections.Generic;

namespace Oracle.Packets.Model
{
    public class DTClasses
    {
        private Dictionary<int, DtClass> _byClassId { get; set; } = new Dictionary<int, DtClass>();
        private Dictionary<string, DtClass> _byDtName = new Dictionary<string, DtClass>();
        public int ClassBits { get; set; }

        public void OnDtClass(DtClass dtClass)
        {
            _byDtName.TryAdd(dtClass.GetDtName, dtClass);
        }

        public DtClass ForClassId(int id)
        {
            return _byClassId[id];
        }

        public void AddClassId(int id, DtClass cls)
        {
            _byClassId.Add(id, cls);
        }

        public DtClass ForDtName(string dtName)
        {
            return _byDtName[dtName];
        }
        public int ClassCount { get => _byClassId.Count; }
    }
}