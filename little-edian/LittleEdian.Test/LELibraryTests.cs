using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LittleEdian.Core;

namespace LittleEdian.Test
{
    public class LELibraryTests
    {
        [Fact]
        public void ToUInt_ReturnInt_WhenBytesAreProvided()
        {
            var bytes = new byte[] { };
            var expectedUint = 123;

            var result = LELibrary.ToUInt(bytes);

            //result.Should()
        }
    }
}
