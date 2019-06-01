using System;
using Google.Protobuf;

namespace Oracle.Packets.Model
{
    public class StringTableModel
    {
        public string Name { get; }
        public int MaxEntries { get;  }
        public bool UserDataFixedSize { get; }
        public int UserDataSize { get; }
        public int UserDataSizeBits { get; }
        public int Flags { get; }

        private string[][] names;
        private ByteString[][] values;

        private int initialEntryCount;
        private int entryCount;


        public StringTableModel(string name, int maxEntries, bool userDataFixedSize, int userDataSize, int userDataSizeBits, int flags)
        {
            this.Name = name;
            this.MaxEntries = maxEntries;
            this.UserDataFixedSize = userDataFixedSize;
            this.UserDataSize = userDataSize;
            this.UserDataSizeBits = userDataSizeBits;
            this.Flags = flags;

            names = new string[maxEntries][];
            values = new ByteString[maxEntries][];
            Array.Fill(names, new string[2]);
            Array.Fill(values, new ByteString[2]);

            initialEntryCount = 0;
            entryCount = 0;

        }

        public void EnsureSize(int minCap)
        {
            if (names.Length < minCap)
            {
                int oldCap = names.Length;
                int newCap = oldCap + (oldCap >> 1);
                if (newCap - minCap < 0)
                {
                    newCap = minCap;
                }
                Array.Resize(ref names, newCap);
                Array.Resize(ref values, newCap);
                Array.Fill(names, new string[2], oldCap, newCap - oldCap);
                Array.Fill(values, new ByteString[2], oldCap, newCap - oldCap);
            }
        }

        public void Set(int tbl, int index, string name, ByteString val)
        {
            EnsureSize(index + 1);

            if ((tbl & 1) != 0)
            {
                initialEntryCount = Math.Max(initialEntryCount, index + 1);
                var test = names[index][0];
                this.names[index][0] = name;
                this.values[index][0] = val;
            }

            if ((tbl & 2) != 0)
            {
                entryCount = Math.Max(entryCount, index + 1);
                this.names[index][1] = name;
                this.values[index][1] = val;
            }
        }

        public bool HasIndex(int index) => index >= 0 && names.Length > index;
        public ByteString GetValueByIndex(int index) => values[index][1];
        public string GetNameByIndex(int index) => names[index][1];

        public void Reset()
        {
            for (int i = 0; i < names.Length; i++)
            {
                names[i][1] = names[i][0];
                values[i][1] = values[i][0];
            }

            entryCount = initialEntryCount;
        }
    }
}