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
            Console.WriteLine(nanNumber);

            string extractedMessage = Extractor(nanNumber);

            if (message != extractedMessage)
            {
                Console.WriteLine($"fail - message: {message} - extractedMessage: {extractedMessage}");
                Console.WriteLine($"length of message: {message.Length} - length of extractedMessage: {extractedMessage.Length}");
            }
            else
            {
                Console.WriteLine($"success: {message}");
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

            // exponentBits
            rawNumber = rawNumber | 0b0111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL;

            // fixedBit1
            rawNumber = rawNumber | 0b0111_1111_1111_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL;

            // length3
            ulong lengthSize = (ulong) messageBytes.Length;
            lengthSize = lengthSize << 48;
            rawNumber = rawNumber | lengthSize;

            // message48(6 bytes) ~~ payload  
            // rawNumber = 0b0111_1111_1111_1101_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL
            //rawNumber = BitHelper.AppendToUlong(rawNumber, messageBytes);
            for (int i = 0; i < messageBytes.Length; i++)
            {
                // rawNumber = 0b0111_1111_1111_1101_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL
                // byteLong = 0b0000_0000_0000_0000_[0000_0000]_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL
                // 1. move to right 40 bits
                // 2. move to right 32 bits
                ulong byteLong = messageBytes[i];
                byteLong = byteLong << (40 - (i*8));
                rawNumber = rawNumber | byteLong;
            }
            
            return BitConverter.Int64BitsToDouble((long)rawNumber);
        }

        private static string Extractor(double nanNumber)
        {
            // 1) Extract raw bits
            long raw = BitConverter.DoubleToInt64Bits(nanNumber);
            ulong bits = unchecked((ulong)raw);
            
            // 1.5) Extract size of message 
            ulong length = bits & 0b0000_0000_0000_0111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL;
            length = length >> 48;
            
            // 2) Extract bit from 48 to 0 
            bits = bits & 0b00000000_00000000_11111111_11111111_11111111_11111111_11111111_11111111UL;
            
            // 3) parse bits to bytes
            var bytes = new byte[length];
            for (int i = 0; i < bytes.Length; i++)
            {
               ulong temp2 = ((bits << (16 + (8*i))) >> 56);
               bytes[i] = (byte)(temp2 & 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_11111111UL);
            }
            
            // 4. Parse to string 
            string message = Encoding.UTF8.GetString(bytes);
            
            return message;   
        }
    }
}
