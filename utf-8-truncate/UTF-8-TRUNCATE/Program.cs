
using System.Linq;
using System.Text;

namespace UTF_8_TRUNCATE
{
    internal class Program
    {
        private static byte LineFeed = 0x0A; // '\n'
        private static byte Utf8LeadingMask = 0xC0; // 1100_0000
        private static byte Utf8Continuation = 0x80; // 1000_0000

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            string path = @"C:\temp\cases";
            string outputPath = @"C:\temp\cases_truncate";

            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var fsOut = new FileStream(outputPath, FileMode.Create, FileAccess.Write);

            while (true)
            {
                var line = ReadBinaryLine(fs);
                if (line == null) break;

                var truncateChars = Truncate(line[1..], line[0]).ToList();
                truncateChars.Add(0x0A);

                fsOut.Write(truncateChars.ToArray());
            }
        }

        private static byte[] Truncate(byte[] remainChars, byte numberToCut)
        {
            if ((int)numberToCut >= remainChars.Length)
            {
                return remainChars[..^1];
            }

            while (numberToCut > 0 && ((remainChars[numberToCut] & Utf8LeadingMask) == Utf8Continuation)) // check remainChars[numberToCut] is continue byte
            {
                numberToCut = (byte)(numberToCut - 1);
            }

            return remainChars[..numberToCut];
        }

        static byte[] ReadBinaryLine(FileStream fs)
        {
            var bytes = new List<byte>();
            int b;

            while ((b = fs.ReadByte()) != -1)
            {
                bytes.Add((byte)b);
                if (b == LineFeed) // '\n'
                    break;
            }

            if (bytes.Count == 0)
                return null; // EOF

            return bytes.ToArray();
        }
    }
}
