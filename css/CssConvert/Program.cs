using System.Drawing;

namespace CssConvert
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            MyCssConverter myCssConverter = new MyCssConverter();
            var color = myCssConverter.HexToRGB("#4A7FBC");
            Console.WriteLine($"{color.R}, {color.G}, {color.B}"); 
        }
    }
}
