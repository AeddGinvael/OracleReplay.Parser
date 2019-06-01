using System;
using System.Collections.Generic;
using Proto.Dota;

namespace Oracle.Packets.Model
{
    public class StringTablesModel
    {
        public Dictionary<int, StringTableModel> ById { get; } = new Dictionary<int, StringTableModel>();
        public Dictionary<string, StringTableModel> ByName { get; } = new Dictionary<string, StringTableModel>();

        public void OnStringTableModelCreated(int tableNum, StringTableModel table)
        {
            if (ById.ContainsKey(tableNum) || ByName.ContainsKey(table.Name))
            {
                throw new Exception("Already exist");
            }
            ById.Add(tableNum, table);
            ByName.Add(table.Name, table);
        }


        public void ClearAllStringTables()
        {
            ById.Clear();
            ByName.Clear();
        }

        public StringTableModel ForName(string name)
        {
            ByName.TryGetValue(name, out var res);
            return res;
        }

        public StringTableModel ForId(int id)
        {
            ById.TryGetValue(id, out var res);
            return res;
        }
    }
}