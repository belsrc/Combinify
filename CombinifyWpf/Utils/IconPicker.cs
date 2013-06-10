// -------------------------------------------------------------------------------
//    FileIcon.cs
//    Copyright (c) 2013 Bryan Kizer
//    All rights reserved.
//
//    Redistribution and use in source and binary forms, with or without
//    modification, are permitted provided that the following conditions are
//    met:
//
//    Redistributions of source code must retain the above copyright notice,
//    this list of conditions and the following disclaimer.
//
//    Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation
//    and/or other materials provided with the distribution.
//
//    Neither the name of the Organization nor the names of its contributors
//    may be used to endorse or promote products derived from this software
//    without specific prior written permission.
//
//    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
//    IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
//    TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
//    PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
//    HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//    SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
//    TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
//    PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//    LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//    NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//    SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// -------------------------------------------------------------------------------
namespace CombinifyWpf.Utils {
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// A class to extract an Icon from a file.
    /// </summary>
    public class IconPicker {

        /// <summary>
        /// Initializes a new instance of the FileIcon class.
        /// </summary>
        public IconPicker() { }

        /// <summary>
        /// Gets an ImageSource of the default icon for a file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>An ImageSource representing the files icon.</returns>
        public ImageSource GetFileIcon( string path ) {
            var icon = System.Drawing.Icon.ExtractAssociatedIcon( path );
            var stream = GetIconStream( icon );

            return GetBitMapImage( stream );
        }

        private static Stream GetIconStream( Icon icon ) {
            var iconStream = new MemoryStream();

            var bitmap = icon.ToBitmap();
            bitmap.Save( iconStream, ImageFormat.Png );

            return iconStream;
        }

        private static ImageSource GetBitMapImage( Stream iconStream ) {
            var bi = new BitmapImage();

            bi.BeginInit();
            bi.StreamSource = iconStream;
            bi.EndInit();

            return bi;
        }
    }
}
