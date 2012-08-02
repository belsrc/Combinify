/*
    Copyright (c) 2012, Bryan Kizer
    All rights reserved. 

    Redistribution and use in source and binary forms, with or without 
    modification, are permitted provided that the following conditions are 
    met: 

    * Redistributions of source code must retain the above copyright notice, 
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of the Organization nor the names of its contributors 
      may be used to endorse or promote products derived from this software 
      without specific prior written permission. 
  
    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS 
    IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED 
    TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
    PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT 
    HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
    SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
    TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
    PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
    LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
    NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS 
    SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
namespace QuickMinCombine {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ColorConversions {
        private Dictionary<string, string> _hexSmallerList;
        private Dictionary<string, string> _nameSmallerList;

        public ColorConversions() {
            _nameSmallerList = new Dictionary<string, string>() {
                { "#f0ffff", "azure" },  { "#f5f5dc", "beige" },  { "#ffe4c4", "bisque" },
                { "#a52a2a", "brown" },  { "#ff7f50", "coral" },  { "#ffd700", "gold" },
                { "#808080", "gray" },   { "#008000", "green" },  { "#4b0082", "indigo" },
                { "#fffff0", "ivory" },  { "#f0e68c", "khaki" },  { "#faf0e6", "linen" },
                { "#800000", "maroon" }, { "#000080", "navy" },   { "#808000", "olive" },
                { "#ffa500", "orange" }, { "#da70d6", "orchid" }, { "#cd853f", "peru" },
                { "#ffc0cb", "pink" },   { "#dda0dd", "plum" },   { "#800080", "purple" },
                { "#f00", "red" },       { "#ff0000", "red" },    { "#fa8072", "salmon" },
                { "#a0522d", "sienna" }, { "#c0c0c0", "silver" }, { "#fffafa", "snow" },
                { "#d2b48c", "tan" },    { "#008080", "teal" },   { "#ff6347", "tomato" },
                { "#ee82ee", "violet" }, { "#f5deb3", "wheat" },  { "#ffff00", "yellow" }
            };
        }

        /// <summary>
        /// Converts an RGB color value to Hexadecimal.
        /// </summary>
        /// <remarks>
        /// <para>Apparently if one of the values is 0 instead of getting 00 back you just get 0
        /// so I guess there goes my liner. *cry*</para>
        /// <para>return ( "#" + r.ToString( "X" ) + g.ToString( "X" ) + b.ToString( "X" ) ).ToLower();</para>
        /// </remarks>
        /// <param name="r">Red value contained in the set [0, 255].</param>
        /// <param name="g">Green value contained in the set [0, 255].</param>
        /// <param name="b">Blue value contained in the set [0, 255].</param>
        /// <returns>A hexadecimal value representing the supplied RGB values.</returns>
        public string ConvertRgbToHex( byte[] rgb ) {
            string s = "#";
            s += BitConverter.ToString( rgb ).Replace( "-", string.Empty );

            return s.ToLower();
        }

        /// <summary>
        /// Converts an HSL color value to Hexadecimal.
        /// </summary>
        /// <remarks>
        /// <para>Ported from mjijackson.com/2008/02/rgb-to-hsl-and-rgb-to-hsv-color-model-conversion-algorithms-in-javascript.</para>
        /// <para>Conversion formula adapted from en.wikipedia.org/wiki/HSL_color_space.</para>
        /// </remarks>
        /// <param name="h">Hue value contained in the set [0, 1].</param>
        /// <param name="s">Saturation value contained in the set [0, 1].</param>
        /// <param name="l">Lightness value contained in the set [0, 1].</param>
        /// <returns>A hexadecimal value representing the supplied HSL values.</returns>
        public string ConvertHslToHex( double h, double s, double l ) {
            double r, g, b, q, p;

            if( s == 0 ) {
                r = g = b = l;
            }
            else {
                q = l < 0.5 ? l * ( 1 + s ) : l + s - ( l * s );
                p = 2 * l - q;
                r = HueToRgb( p, q, h + ( 1.0 / 3.0 ) );
                g = HueToRgb( p, q, h );
                b = HueToRgb( p, q, h - ( 1.0 / 3.0 ) );
            }

            byte[] rgb = { ( byte )Math.Round( r * 255 ),
                           ( byte )Math.Round( g * 255 ),
                           ( byte )Math.Round( b * 255 ) };

            return ConvertRgbToHex( rgb );
        }

        private double HueToRgb( double p, double q, double t ) {
            if( t < 0 ) { t += 1; }
            if( t > 1 ) { t -= 1; }

            if( t * 6.0 < 1.0 ) { return p + ( ( q - p ) * 6 * t ); }
            if( t * 2.0 < 1.0 ) { return q; }
            if( t * 3.0 < 2.0 ) { return p + ( ( q - p ) * 6 * ( ( 2.0 / 3.0 ) - t ) ); }

            return p;
        }

        /// <summary>
        /// Compresses a six character hexadecimal value to its three character representation if possible.
        /// </summary>
        /// <param name="hex">A hexadecimal value.</param>
        /// <returns>If possible, returns the compressed hex value. Otherwise, returns the original hex.</returns>
        public string CompressHex( string hex ) {
            if( hex[ 0 ] == hex[ 1 ] && hex[ 2 ] == hex[ 3 ] && hex[ 4 ] == hex[ 5 ] ) {
                return ( "#" + hex[ 0 ] + hex[ 2 ] + hex[ 4 ] ).ToLower();
            }
            else {
                return ( "#" + hex ).ToLower();
            }
        }

        /// <summary>
        /// Swaps out a hexadecimal color value for its smaller literal color name.
        /// </summary>
        /// <param name="hex">A compressed hexadecimal value.</param>
        /// <returns>Literal color name, it a smaller one exists. Otherwise, returns the hex value.</returns>
        public string SwapOutHex( string hex ) {
            if( _nameSmallerList.Keys.Contains( hex ) ) {
                return _nameSmallerList[ hex ];
            }
            else {
                return hex;
            }
        }
    }
}
