
namespace CombiTests {
    using System.Collections.Generic;
    using System.Linq;
    using CombinifyWpf.Utils;
    using NUnit.Framework;

    [TestFixture]
    public class ColorConvertTest {
        ColorConverter _converter;

        [SetUp]
        public void Init() {
            this._converter = new ColorConverter();
        }

        [TearDown]
        public void Cleanup() {
            this._converter = null;
        }

        [Test]
        public void RgbToHexTest() {
            List<byte[]> rgbs = new List<byte[]>() {
                new byte[] { 78, 166, 234 },
                new byte[] { 145, 0, 145 },
                new byte[] { 235, 60, 0 }
            };
            List<string> expecteds = new List<string>() {
                "4ea6ea",
                "910091",
                "eb3c00"
            };

            for( int i = 0; i < rgbs.Count(); i++ ) {
                string actual = this._converter.ConvertRgbToHex( rgbs[ i ] );
                Assert.AreEqual( expecteds[ i ], actual, "Original was " + string.Join( ", ", rgbs[ i ] ) );
            }
        }

        [Test]
        public void HslToHexTest() {
            List<float[]> hslas = new List<float[]>() {
                new float[] { 206F, 79F, 61F },
                new float[] { 300F, 100F, 28F },
                new float[] { 15F, 100F, 46F }
            };
            List<string> expecteds = new List<string>() {
                "4da6ea",
                "8f008f",
                "eb3b00"
            };

            for( int i = 0; i < hslas.Count(); i++ ) {
                string actual = this._converter.ConvertHslToHex( hslas[ i ][ 0 ], hslas[ i ][ 1 ], hslas[ i ][ 2 ] );
                Assert.AreEqual( expecteds[ i ], actual, "Original was " + string.Join( ", ", hslas[ i ] ) );
            }
        }
    }
}
