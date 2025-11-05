using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CssConvert;

public class MyCssConverter
{
    public Color HexToRGB(string hexColor)
    {
        return CssHelper.HexToRGB(hexColor);
    }
}
