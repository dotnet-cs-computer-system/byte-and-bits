using System.IO;

namespace ImageRotate.Core;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        string path = @"C:\temp\image-rotate\image-rotate\teapot.bmp";
        byte[] bytes = File.ReadAllBytes(path);

        //// Chuyển mảng byte → chuỗi hex kiểu “AA BB CC DD …”
        

        //HexViewer.HexDump(path);
    }

    private string ToHex(byte input)
    {
        return input.ToString("X2");
    }
}

