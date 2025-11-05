using System;
using System.Collections.Generic;
using System.Drawing;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CssConvert;

public class CssHelper
{
    private static Dictionary<string, int> HexMap = new()
    {
        { "1", 1},
        { "2", 2},
        { "3", 3},
        { "4", 4},
        { "5", 5},
        { "6", 6},
        { "7", 7 },
        { "8", 8 },
        { "9", 9 },
        { "A", 10 },
        { "B", 11 },
        { "C", 12 },
        { "D", 13 },
        { "E", 14 },
        { "F", 15 }
    };

    private static int BASE_16 = 16;

    public static Color HexToRGB(string hexColor)
    {
        // TODO: validation hexColor, format should be "#XXXXXX", "X should be in the HexMap"
        Console.WriteLine($"{hexColor}");
        hexColor = hexColor.Replace("#", "");

        string redString = hexColor.Substring(0, 2);
        string greenString = hexColor.Substring(2, 2);
        string blueString = hexColor.Substring(4, 2);
        
        int red = ToInt(redString);
        int green = ToInt(greenString);
        int blue = ToInt(blueString);

        return Color.FromArgb(255, redHex, greenHex, blueHex);
    }

    private static int ToInt(string input)
    {
        // TODO: 
        // input length is 2, i.e "4A"
        // validation hex
        //Console.WriteLine($"left 4 bits for {HexMap[input[0].ToString()]} : {HexMap[input[0].ToString()] << 4}");
        //var movebit = HexMap[input[0].ToString()] << 4;
        //var bitwiseValue = (HexMap[input[0].ToString()] << 4) + HexMap[input[1].ToString()];
        //var normalCal = HexMap[input[0].ToString()] * BASE_16 + HexMap[input[1].ToString()];

        return (HexMap[input[0].ToString()] << 4) + HexMap[input[1].ToString()];
    }
}
