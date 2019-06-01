using System;

namespace Oracle.Packets.Model
{
    public class Vector
    {
        private float[] _vector;
        public int Length
        {
            get => _vector.Length;
        }
        public Vector(float[] vect)
        {
            this._vector = vect;
        }

        public float GetElement(int i) => _vector[i];

        public override string ToString() => "[" + string.Join(",",_vector) + "]";
    }
}