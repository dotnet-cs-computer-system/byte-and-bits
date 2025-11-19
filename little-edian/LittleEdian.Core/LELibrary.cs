using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleEdian.Core;

public static class LELibrary
{
    public static uint ToUInt(Byte[] bytes)
    {
        if (bytes.Length < 4)
            throw new ArgumentException("At least 4 bytes are required.");

        uint n = 0;

        /* phep cong */

        //n = (uint)(
        //    bytes[0] * Math.Pow(256, 0) +
        //    bytes[1] * Math.Pow(256, 1) +
        //    bytes[2] * Math.Pow(256, 2) +
        //    bytes[3] * Math.Pow(256, 3));

        /* phep dich bit */
        n = (uint)(
            bytes[0] +
            (bytes[1] << 8) + // dich trai 8 bit ~~ Math.Pow(256, 1)
            (bytes[2] << 16) +
            (bytes[3] << 24));

        return n;
    }
}
