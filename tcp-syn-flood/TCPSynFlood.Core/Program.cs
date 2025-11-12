
using System.Buffers.Binary;
using System.Net.Sockets;

namespace TCPSynFlood.Core;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var bytes = await File.ReadAllBytesAsync(@"C:\temp\synflood.pcap");
        Console.WriteLine($"{bytes.Length} bytes");

        //
        var headerBytes = bytes[0..24];
        PrintBytes(headerBytes);

        // parse magic_number, major, minor, _, _, _, llh_type
        /*
            d4 c3 b2 a1   # magic_number  = 0xa1b2c3d4 (đọc LE) (32 bit) ~~ 0 - 4
	            02 00         # version_major = 2                   (16 bit) ~~ 4 - 6
	            04 00         # version_minor = 4                            ~~ 6 - 8 
	            00 00 00 00   # thiszone      = 0                               8 - 12             
	            00 00 00 00   # sigfigs       = 0                               12 - 16
	            ff ff 00 00   # snaplen       = 0x0000ffff = 65535              16 - 20
	            01 00 00 00   # network       = 1 (Ethernet)                    20 - 24
         
         */
        Console.WriteLine();

        uint magicNumber = ParseBytes(headerBytes[0..4]); // 4 byte ~~ 32 bit ~~ int32 ~~ int 
        Console.WriteLine($"expected: {0xa1b2c3d4} - actual: {magicNumber}");

        uint major = ParseBytes(headerBytes[4..6]);
        uint minor = ParseBytes(headerBytes[6..8]);
        uint llh_type = ParseBytes(headerBytes[20..24]);
        Console.WriteLine($"major: {major} - minor: {minor} - llh_type: {llh_type}"); //expectation: 2 - 4 - 0

        // read how many packets, ipv4, ihl, src, dst, flags
        /*
         * ipv4: family = struct.unpack('<I', packet[:4])[0]
         * 
         */
        int count = 0;
        var packetBytes = bytes[24..];

        int offset = 0;
        while (true)
        {
            // offset move to at the end of the length
            if (offset + 16 > packetBytes.Length) break;

            // header
            var header = packetBytes[offset..(offset+16)];
            offset += header.Length;

            if (header == null || header.Length == 0) continue;

            // content
            var length = ParseBytes(header[8..12]);
            var content = packetBytes[offset..(int)(offset+length)];
            offset += content.Length;

            var ipv4 = ParseBytes(content[0..1]);
            Console.WriteLine($"ipv4: {ipv4}");

            var ihl = (content[4] & 0x0F) << 2;
            Console.WriteLine($"ihl: {ihl}");

            // src, dst, _, _, flags = "!HHIIH", packet[24:38]
            var result = UnpackBigEndian.Unpack("HHIIH", content[24..38]);

            ushort src = (ushort)result[0];
            ushort dst = (ushort)result[1];
            uint seq = (uint)result[2];
            uint ack = (uint)result[3];
            ushort flags = (ushort)result[4];
            Console.WriteLine($"src: {src} --> dst: {dst}");

            count++;
        }
        Console.WriteLine($"count: {count}");
    }

    static void PrintBytes(byte[] bytes)
    {
        foreach (byte b in bytes) 
        {
            Console.Write($"{b:X2} ");
        }
    }

    private static uint ParseBytes(byte[] bytes)
    {
        uint n = 0;
        for (int i = 0; i < bytes.Length; i++)
            n += (uint)bytes[i] << (i * 8);
        return n;
    }
}

public static class UnpackBigEndian
{
    public enum FormatCode
    {
        B,  // Unsigned byte (1B)
        H,  // Unsigned short (2B)
        I,  // Unsigned int (4B)
        Q,  // Unsigned long (8B)
        S,  // Raw bytes 
        X   // Skip byte
    }
    public static List<object> Unpack(string fmt, ReadOnlySpan<byte> data)
    {
        var result = new List<object>();
        int offset = 0;
        int i = 0;

        while (i < fmt.Length)
        {
            int count = 0;
            while (i < fmt.Length && char.IsDigit(fmt[i]))
            {
                count = count * 10 + (fmt[i] - '0');
                i++;
            }
            if (count == 0) count = 1;

            if (i >= fmt.Length) break;
            char c = fmt[i++];

            if (!Enum.TryParse<FormatCode>(c.ToString().ToUpper(), out var code))
                throw new NotSupportedException($"Unsupported format code: '{c}'");

            switch (code)
            {
                case FormatCode.X: // skip
                    offset += count;
                    break;

                case FormatCode.B: // unsigned byte
                    for (int k = 0; k < count; k++)
                    {
                        result.Add(data[offset]);
                        offset += 1;
                    }
                    break;

                case FormatCode.H: // unsigned short (2 bytes)
                    for (int k = 0; k < count; k++)
                    {
                        ushort val = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
                        result.Add(val);
                        offset += 2;
                    }
                    break;

                case FormatCode.I: // unsigned int (4 bytes)
                    for (int k = 0; k < count; k++)
                    {
                        uint val = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(offset, 4));
                        result.Add(val);
                        offset += 4;
                    }
                    break;

                case FormatCode.Q: // unsigned long (8 bytes)
                    for (int k = 0; k < count; k++)
                    {
                        ulong val = BinaryPrimitives.ReadUInt64BigEndian(data.Slice(offset, 8));
                        result.Add(val);
                        offset += 8;
                    }
                    break;

                case FormatCode.S: // raw bytes
                    result.Add(data.Slice(offset, count).ToArray());
                    offset += count;
                    break;
            }
        }
        return result;
    }
}
