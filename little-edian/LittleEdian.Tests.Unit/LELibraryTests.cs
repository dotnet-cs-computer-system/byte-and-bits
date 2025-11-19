using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LittleEdian.Core;

namespace LittleEdian.Tests.Unit
{
    public class LELibraryTests
    {
        [Theory]
        [InlineData(new byte[] { 0x0A, 0x00, 0x00, 0x00 }, 10u)]                // 0x0A = 10
        [InlineData(new byte[] { 0x01, 0x00, 0x00, 0x00 }, 1u)]           // 0x0001
        [InlineData(new byte[] { 0xFF, 0x00, 0x00, 0x00 }, 255u)]         // 0x00FF
        [InlineData(new byte[] { 0x34, 0x12, 0x00, 0x00 }, 0x1234u)]      // 0x1234 = 4660
        [InlineData(new byte[] { 0x78, 0x56, 0x34, 0x12 }, 0x12345678u)] // 0x12345678 = 305419896
        public void ToUInt_ReturnsCorrectValue_WhenBytesAreProvided(byte[] bytes, uint expected)
        {
            // act
            var actual = LELibrary.ToUInt(bytes);

            // assert
            actual.Should().Be(expected);
        }
    }
}
