
using System.Collections;
using System.Text;

namespace SneakyNAN.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            string message = "xinch";
            double nanNumber = Conceal(message);

            string extractedMessage = Extractor(nanNumber);

            if (message != extractedMessage)
            {
                Console.WriteLine($"fail - message: {message} - extractedMessage: {extractedMessage}");
            }
            else
            {
                Console.WriteLine($"sucess: {message}");
            }
        }

        private static double Conceal(string message)
        {
            /*
                resultBits = 
		            sign (1 bit)						~~ 0
	              + exponentBits (11 bits)				~~ 11111111111	
	              + fixed1 (1 bit, always 1)			~~ 1
	              + length3 (3 bits for msg size)		~~ message size 
	              + message48 (6-byte payload)			~~ message payload 
            */
            ulong rawNumber = 0; //64bit

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            if (messageBytes.Length > 6)
            {
                throw new InvalidDataException(message);
            }

            //sign
            rawNumber = rawNumber | 0b0;
            Console.WriteLine($"after sign: {PrintBits(rawNumber)}");

            // exponentBits
            Console.WriteLine($" 0x7FF0_0000_0000_0000: {PrintBits(0x7FF0_0000_0000_0000)}");
            rawNumber = rawNumber | 0x7FF0_0000_0000_0000;
            Console.WriteLine($"after exponentBits: {PrintBits(rawNumber)}");

            // fixedBit1
            Console.WriteLine($" 0x7FF8_0000_0000_0000: {PrintBits(0x7FF8_0000_0000_0000)}");
            rawNumber = rawNumber | 0x7FF8_0000_0000_0000;
            Console.WriteLine($"after fixedBit1: {PrintBits(rawNumber)}");

            // length3
            ulong lengthSize = (ulong) messageBytes.Length;
            lengthSize = lengthSize << 48;
            Console.WriteLine($" lengthSize: {PrintBits(lengthSize)}");
            rawNumber = rawNumber | lengthSize;
            Console.WriteLine($"after length3: {PrintBits(rawNumber)}");

            // message48(6 - byte payload)          ~~message payload
            //Console.WriteLine($"value: " {BytesToUlongBigEndian()})
            //messageBytes = Allocate(messageBytes);
            //Console.WriteLine($" messageBytes: {PrintBytes(messageBytes)}");
            ulong payload = BytesToUlongBigEndian(messageBytes);
            //Console.WriteLine($" payload: {PrintBits(payload)}");
            //int fixedBit1 = 0b1;
            //int lengthBit3 = messageBytes.Length;
            //messageBytes = Allocate(messageBytes);

            return 123;
        }

        public static ulong MoveBit(byte[] bytes)
        {
            if (bytes.Length > 8)
                throw new ArgumentException("Max 8 bytes");
            ulong n = 0;
            int bitIndex = 0;
            for (int i = 0; i < 64; i++)
            {
                // push 0 to n
                // if i == 15, push vale from bytes to n
                if (i <= 15)
                {
                    // do notthing
                }
                else
                {
                    // get bit from bytes 
                    // append bit to n
                    int bit = GetBit(bytes, bitIndex);
                    bitIndex++;
                    AppendBit(n, bit);
                }
            }
            return n;
        }

        public static int GetBit(byte[] bytes, int bitIndex)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            int totalBits = bytes.Length * 8;
            if (bitIndex < 0 || bitIndex >= totalBits)
                throw new ArgumentOutOfRangeException(nameof(bitIndex));

            int byteIndex = bitIndex / 8;
            int bitInByte = 7 - (bitIndex % 8); // MSB trước

            int b = bytes[byteIndex];
            int bit = (b >> bitInByte) & 1;
            return bit;
        }

        public static ulong AppendBit(ulong n, int bit)
        {
            n <<= 1;                // chừa chỗ cho bit mới
            if ((bit & 1) != 0)     // nếu là 1 thì set LSB
            {
                n |= 1UL;
            }
            return n;
        }

        public static ulong BytesToUlongBigEndian(byte[] bytes)
        {
            if (bytes.Length > 8)
                throw new ArgumentException("Max 8 bytes");
            Console.WriteLine("BytesToUlongBigEndian");
            ulong value = 0;
            //foreach (var b in bytes)
            //{
            //    Console.WriteLine($"value: {PrintBits(value)}");
            //    ulong move8ToRight = value << 8;
            //    value = move8ToRight | b;
            //    Console.WriteLine($"move8ToRight: {PrintBits(move8ToRight)}");
            //    Console.WriteLine($"new: {PrintBits(value)}");
            //    Console.WriteLine();
            //}
            int index = 0;
            while (true)
            {
                if (index <= bytes.Length)
                byte b = bytes[index];

                index++;
            }

            return value;
        }

        public static ulong MakeUlongHas16BitOnTheLeft(ulong l)
        {
            ulong value = l;
            ulong count = 0;
            while (true)
            {

                count++;
            }

            return value;
        }

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
            string result = "";

            foreach (byte c in messageBytes)
            {
                result += Convert.ToString(c, 2).PadLeft(8, '0'); ;
                result += " ";
            }

            return result;
        }

        private static byte[] Allocate(byte[] messageBytes)
        {
            var bytes = messageBytes.ToList();

            while (bytes.Count <= 6)
            {
                bytes.Add(0b0);
            }
            return bytes.ToArray();
        }

        private static string Extractor(double nanNumber)
        {
            return "";   
        }
    }
}
