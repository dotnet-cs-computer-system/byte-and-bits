using System.Text;

namespace SneakyNAN.Core;

public static class BitHelper
{
        public static string ToBinary64(ulong value)
        {
            char[] bits = new char[64];

            for (int i = 63; i >= 0; i--)
            {
                bits[i] = (value & 1) == 1 ? '1' : '0';
                value >>= 1;
            }

            return new string(bits);
        }

        public static string PrintBits(ulong value)
        {
            string result = "";
            string bits = ToBinary64(value);
            int count = 0;
            foreach (char c in bits)
            {
                result += c;
                count++;

                if (count == 4)
                {
                    result += " ";
                    count = 0;
                }
            }

            return result;
        }

        public static string PrintBytes(byte[] messageBytes)
        {
            string result8Bit = "";

            foreach (byte c in messageBytes)
            {
                result8Bit += Convert.ToString(c, 2).PadLeft(8, '0');
            }

            string result = "";
            int count = 0;
            foreach (char c in result8Bit)
            {
                result += c;
                count++;

                if (count == 4)
                {
                    result += " ";
                    count = 0;
                }
            }
            
            return result;
        }
}