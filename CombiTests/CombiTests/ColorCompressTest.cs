
namespace CombiTests {
    using System.Collections.Generic;
    using System.Linq;
    using CombinifyWpf.Utils;
    using NUnit.Framework;

    [TestFixture]
    public class ColorCompressTest {
        ColorCompressor _compressor;

        [SetUp]
        public void Init() {
            this._compressor = new ColorCompressor();
        }

        [TearDown]
        public void Cleanup() {
            this._compressor = null;
        }

        // string CompressHex( string hex )

        // string SwapOutHex( string hex )

        // string SwapOutNames( string css )
    }
}
