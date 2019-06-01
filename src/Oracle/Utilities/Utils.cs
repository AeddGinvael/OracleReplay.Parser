namespace Oracle.BitStream.Utilities
{
    public class Utils
    {
        public static int CalculateBitsNeededFor(long x)
        {
            if (x == 0) 
                return 0;

            int n = 32;

            if (x <= 0x0000FFFF) {
                n -= 16; x <<= 16;
            }
            if (x <= 0x00FFFFFF) {
                n -= 8; x <<= 8;
            }
            if (x <= 0x0FFFFFFF) {
                n -= 4; x <<= 4;
            }
            if (x <= 0x3FFFFFFF) {
                n -= 2; x <<= 2;
            }
            if (x <= 0x7FFFFFFF) {
                n -= 1;
            }
            return n;
        }

        public static string ArrayIdxToString(int idx)
        {
            return string.Format("%04d", idx);
        }
    }
}