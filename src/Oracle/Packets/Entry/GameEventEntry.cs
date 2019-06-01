using System.Collections.Generic;
namespace Oracle.Packets.Entry
{
    public class GameEventEntry
    {
        private int _eventId;
        private string _name;
        private string[] _keys;

        private Dictionary<string, int?> _indexByKey = new Dictionary<string, int?>();

        public GameEventEntry(int eventId, string name, string[] keys)
        {
            _eventId = eventId;
            _name = name;
            _keys = keys;

            for (var i = 0; i < _keys.Length; i++)
            {
                _indexByKey[_keys[i]] = i;
            }

        }

        public int EventId { get => _eventId;}
        public string Name { get => _name; }
        public string[] Keys { get => _keys; }
        public int? GetIndexForKey(string key) 
        {
            _indexByKey.TryGetValue(key, out var value);
            return value;
        }

        public override string ToString()
        {
            return $"eventId={_eventId}, name={_name}";
        }
    }
}