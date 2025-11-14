
using System.Linq;
using System.Text;

namespace UTF_8_TRUNCATE
{
    internal class Program
    {
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

                fsOut.Write(truncateChars.ToArray());
                fsOut.WriteByte(0x0A); // '\n' 
            }
        }

        private static byte[] Truncate(byte[] remainChars, byte numberToCut)
        {
            if ((int)numberToCut >= remainChars.Length)
            {
                return remainChars[..^1];
            }

            while (numberToCut > 0 && ((remainChars[numberToCut] & 0xC0) == 0x80)) // check remainChars[numberToCut] is continue byte
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
                if (b == 0x0A) // '\n'
                    break;
            }

            if (bytes.Count == 0)
                return null; // EOF

            return bytes.ToArray();
        }
    }
}
