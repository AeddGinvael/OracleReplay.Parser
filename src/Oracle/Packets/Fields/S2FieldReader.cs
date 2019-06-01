using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Sources;
using Newtonsoft.Json.Bson;
using Oracle.BitStream;
using Oracle.Packets.Model;

namespace Oracle.Packets.Fields
{
    class S2FieldReader : FieldReader<S2DTClass>
    {
        public override int ReadDeletions(BitArrayStream stream, int num, int[] nums)
        {
            int n = (int)stream.ReadUBitVar();
            int c = 0;
            int idx = -1;
            while (c < n)
            {
                idx += (int)stream.ReadUBitVar();
                nums[c++] = idx;
            }

            return n;
        }

        public override int ReadFields(BitArrayStream stream, S2DTClass cls, object[] state, bool debug)
        {
            dynamic lastUnpacker = null;
            FieldPath lastFp = new FieldPath();
            int lr = 0;

            FieldOpType lastOp = null;
            int ln = 0;
            try
            {
                var n = 0;
                var fp = new FieldPath();
                while (true)
                {
                    int offsBefore = stream.Position;
                    var op = stream.ReadFieldOpType();
                    lastOp = op;
                    op.Execute(fp, stream);

                    if (op is FieldPathEncodeFinish)
                    {
                        break;
                    }

                    FieldPaths[n++] = fp;
                    ln = n;
                    fp = new FieldPath(fp);
                }

                for (int r = 0; r < n; r++)
                {
                    lr = r;
                    fp = FieldPaths[r];
                    lastFp = new FieldPath(fp);

                    var unpacker = cls.GetUnpackerForFieldPath(fp);
                    lastUnpacker = unpacker;
                    if (unpacker == null)
                    {
                        var f = cls.GetFieldForFieldPath(fp).Properties;
                        throw new Exception($"No unpacker for field {f.Name} with type {f.Type}. {fp}");
                    }

                    var offsBefore = stream.Position;
                    object data = unpacker(stream);
                    cls.SetValuesForFieldPath(fp, state, data);
                }

                return n;
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Last OP = {lastOp}, last n = {ln}");
                Console.WriteLine($"Class = {cls.GetSerializer().Id}, Stream Pos = {stream.Position}.\n Last Unpacker = {lastUnpacker}, last r = {lr}, last fp = {lastFp}");
                Console.WriteLine(Ex.Message + "\n" + Ex.InnerException + "\n" + Ex.Source);
                throw;
            }
        }
    }
}
