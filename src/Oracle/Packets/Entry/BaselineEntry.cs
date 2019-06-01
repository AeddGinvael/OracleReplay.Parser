using System;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf;

namespace Oracle.Packets.Entry
{
    public class BaselineEntry
    {
        public ByteString RawBaseLine {get; set;}
        public object[] BaseLine { get; set; }

        public BaselineEntry(ByteString raw)
        {
            this.RawBaseLine = raw;
            this.BaseLine = null;
        }
    }
}
