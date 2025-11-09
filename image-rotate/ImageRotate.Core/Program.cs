using System.IO;

namespace ImageRotate.Core;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        string path = @"C:\temp\image-rotate\image-rotate\teapot.bmp";
        byte[] bytes = File.ReadAllBytes(path);

        // first pixel 
        int offset = ParseBytes(bytes[10..14]);
        Console.WriteLine(offset);

        // get the height and width of the file 
        // height: 18-22(order of bytes)
        // width: 22-26(order of bytes)
        int height = ParseBytes(bytes[18..22]);
        int width = ParseBytes(bytes[22..26]);
        Console.WriteLine($"height: {height}, width: {width}"); // expectation: 420 , 420 

        // rorate pixels
        var pixels = new List<byte>();
        for (int ty = 0; ty < width; ty++) // TODO: What should we do for non-squares?
        {
            for (int tx = 0; tx < width; tx++)
            {
                int sy = tx;
                int sx = width - ty - 1;
                int n = (int)offset + 3 * (sy * width + sx);

                pixels.Add(bytes[n]);     // B
                pixels.Add(bytes[n + 1]); // G
                pixels.Add(bytes[n + 2]); // R
            }
        }

        // create a new image 
        using (var fs = File.Create("C:\\temp\\image-rotate\\image-rotate\\out.bmp"))
        {
            fs.Write(bytes, 0, (int)offset);

            fs.Write(pixels.ToArray(), 0, pixels.Count);
        }

        Console.WriteLine("Done, wrote out.bmp");
    }

    private static int ParseBytes(byte[] bytes)
    {
        int n = 0;
        for (int i = 0; i < bytes.Length; i++)
            n += (int)bytes[i] << (i * 8);
        return n;
    }
}

