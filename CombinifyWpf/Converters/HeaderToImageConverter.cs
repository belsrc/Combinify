// -------------------------------------------------------------------------------
//    HeaderToImageConverter.cs
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
namespace CombinifyWpf.Converters {
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ValueConversion( typeof( string ), typeof( bool ) )]
    public class HeaderToImageConverter : BaseConverter {
        private static HeaderToImageConverter _converter = null;

        public HeaderToImageConverter() { }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue( IServiceProvider serviceProvider ) {
            return _converter ?? ( _converter = new HeaderToImageConverter() );
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public override object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {
            string val = value as string;

            if( val.ToLower().EndsWith( ".css" ) ) {
                Uri uri = new Uri( "pack://application:,,,/Images/css.png" );
                BitmapImage source = new BitmapImage( uri );
                return source;
            }
            else if( val.Contains( ".cpj" ) ) {
                Uri uri = new Uri( "pack://application:,,,/Images/project.png" );
                BitmapImage source = new BitmapImage( uri );
                return source;
            }
            else if( val.Contains( @"\" ) ) {
                Uri uri = new Uri( "pack://application:,,,/Images/drive.png" );
                BitmapImage source = new BitmapImage( uri );
                return source;
            }
            else {
                Uri uri = new Uri( "pack://application:,,,/Images/folder.png" );
                BitmapImage source = new BitmapImage( uri );
                return source;
            }
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public override object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
            throw new NotSupportedException( "Cannot convert back" );
        }
    }
}
