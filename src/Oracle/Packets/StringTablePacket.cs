using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Oracle.BitStream;
using Oracle.Packets.Model;
using Proto.Dota;

namespace Oracle.Packets
{
    public class StringTablePacket
    {
        private ReplayContext _ctx;
        public List<CDemoStringTables.Types.table_t> StringTables = new List<CDemoStringTables.Types.table_t>();
        

        public StringTablePacket(ReplayContext ctx)
        {
            _ctx = ctx;
        }

        public void ParsePacket(byte[] data)
        {
            var table = CDemoStringTables.Parser.ParseFrom(data);

            foreach (CDemoStringTables.Types.table_t table_t in table.Tables)
            {
                OnStringTables(table_t);
            }
        }

        private void OnStringTables(CDemoStringTables.Types.table_t table_t)
        {
            // var table = Parser.DemParser.StringTables.ForName(table_t.TableName);

            // if (table != null)
            // {
            //     for (var i = 0; i < table_t.Items.Count; i++)
            //     {
            //         var it = table_t.Items[i];
            //         Parser.DemParser.SetSingleEntry(table, 2, i, it.Str, it.Data);
            //     }
            // }
        }
    }
}