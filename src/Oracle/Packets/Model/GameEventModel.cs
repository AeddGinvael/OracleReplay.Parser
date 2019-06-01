using System.Text;
using System;
using Oracle.Packets.Entry;

namespace Oracle.Packets.Model
{
    public class GameEventModel
    {
        private GameEventEntry _eventEntry;
        private object[] _state;
        public GameEventModel(GameEventEntry e)
        {
            _eventEntry = e;
            _state = new object[e.Keys.Length];

            for (var i = 0; i < e.Keys.Length; i ++)
            {
                _state[i] = null;
            }
        }

        public void Set(int index, object value)
        {
            _state[index] = value;
        }

        public T Property<T>(int index)
        {
            return (T)_state[index];
        }

        public T Property<T>(string property)
        {
            var index = _eventEntry.GetIndexForKey(property);
            if (index == null)
            {
                throw new ArgumentException($"property {property} not found on game event of class {_eventEntry.Name}");
            }

            return (T) _state[index.Value];
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            for (var i = 0; i < _state.Length; i ++)
            {
                if (i > 0)
                    strBuilder.Append(", ");

                strBuilder.Append(_eventEntry.Keys[i])
                    .Append("=")
                    .Append(_state[i]);   
            }

            return strBuilder.ToString();
        }
    }
}