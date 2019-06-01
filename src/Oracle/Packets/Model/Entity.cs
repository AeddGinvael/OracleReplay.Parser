using Oracle.Packets.Fields;

namespace Oracle.Packets.Model
{
    public class Entity
    {
        public int Index { get; }
        public int Serial { get; }
        public DtClass ClassDt { get; }
        public bool Active { get; set; }
        public object[] State { get; }

        public Entity(int index, int serial, DtClass dtClass, bool active, object[] state)
        {
            this.Index = index;
            this.Serial = serial;
            this.ClassDt = dtClass;
            this.Active = active;
            this.State = state;
        }

        public bool HasProperty(string prop)
        {
            return ClassDt.GetFieldPathForName(prop) != null;
        }


        public bool HasProperties(string[] props)
        {
            foreach (var p in props)
            {
                if (!HasProperty(p))
                {
                    return false;
                }
            }

            return true;
        }

        public T GetPropertyForFieldPath<T>(FieldPath fp)
        {
            
            return ClassDt.GetValueForFieldPath<T>(fp, State);
        }

        public override string ToString()
        {
            var title = $"Idx: {Index}, serial: {Serial}, class: {ClassDt.GetDtName}";
            return title;
        }
    }
}