﻿/*
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
    using System.Text.RegularExpressions;

    /// <summary>
    /// Class to do the various color minifications on css files.
    /// </summary>
    public class ColorConversions {
        /// <summary>
        /// Dictionary where the hex value [value] is smaller than the literal name [key].
        /// </summary>
        private Dictionary<string, string> _hexSmallerList;

        /// <summary>
        /// Dictionary where the literal name [value] is smaller than the hex value [key].
        /// </summary>
        private Dictionary<string, string> _nameSmallerList;

        /// <summary>
        /// Initializes a new instance of the ColorConversions class.
        /// </summary>
        public ColorConversions() {
            // Names and values taken from Chris Coyier's post
            // @ css-tricks.com/snippets/css/named-colors-and-hex-equivalents/
            this._nameSmallerList = new Dictionary<string, string>() {
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

            this._hexSmallerList = new Dictionary<string, string>() {
                { "aliceblue", "#f0f8ff" },      { "antiquewhite", "#faebd7" },         { "aqua", "#0ff" },
                { "aquamarine", "#7fffd4" },     { "black", "#000" },                   { "blanchedalmond", "#ffebcd" },
                { "blue", "#00f" },              { "blueviolet", "#8a2be2" },           { "burlywood", "#deb887" },
                { "cadetblue", "#5f9ea0" },      { "chartreuse", "#7fff00" },           { "chocolate", "#d2691e" },
                { "cornflowerblue", "#6495ed" }, { "cornsilk", "#fff8dc" },             { "crimson", "#dc143c" },
                { "cyan", "#0ff" },              { "darkblue", "#00008b" },             { "darkcyan", "#008b8b" },
                { "darkgoldenrod", "#b8860b" },  { "darkgray", "#a9a9a9" },             { "darkgrey", "#a9a9a9" },
                { "darkgreen", "#006400" },      { "darkkhaki", "#bdb76b" },            { "darkmagenta", "#8b008b" },
                { "darkolivegreen", "#556b2f" }, { "darkorange", "#ff8c00" },           { "darkorchid", "#9932cc" },
                { "darkred", "#8b0000" },        { "darksalmon", "#e9967a" },           { "darkseagreen", "#8fbc8f" },
                { "darkslateblue", "#483d8b" },  { "darkslategray", "#2f4f4f" },        { "darkslategrey", "#2f4f4f" },
                { "darkturquoise", "#00ced1" },  { "darkviolet", "#9400d3" },           { "deeppink", "#ff1493" },
                { "deepskyblue", "#00bfff" },    { "dimgray", "#696969" },              { "dimgrey", "#696969" },
                { "dodgerblue", "#1e90ff" },     { "firebrick", "#b22222" },            { "floralwhite", "#fffaf0" },
                { "forestgreen", "#228b22" },    { "fuchsia", "#f0f" },                 { "gainsboro", "#dcdcdc" },
                { "ghostwhite", "#f8f8ff" },     { "goldenrod", "#daa520" },            { "greenyellow", "#adff2f" },
                { "honeydew", "#f0fff0" },       { "hotpink", "#ff69b4" },              { "indianred", "#cd5c5c" },
                { "lavender", "#e6e6fa" },       { "lavenderblush", "#fff0f5" },        { "lawngreen", "#7cfc00" },
                { "lemonchiffon", "#fffacd" },   { "lightblue", "#add8e6" },            { "lightcoral", "#f08080" },
                { "lightcyan", "#e0ffff" },      { "lightgoldenrodyellow", "#fafad2" }, { "lightgray", "#d3d3d3" },
                { "lightgrey", "#d3d3d3" },      { "lightgreen", "#90ee90" },           { "lightpink", "#ffb6c1" },
                { "lightsalmon", "#ffa07a" },    { "lightseagreen", "#20b2aa" },        { "lightskyblue", "#87cefa" },
                { "lightslategray", "#789" },    { "lightslategrey", "#789" },          { "lightsteelblue", "#b0c4de" },
                { "lightyellow", "#ffffe0" },    { "lime", "#0f0" },                    { "limegreen", "#32cd32" },
                { "magenta", "#f0f" },           { "mediumaquamarine", "#66cdaa" },     { "mediumblue", "#0000cd" },
                { "mediumorchid", "#ba55d3" },   { "mediumpurple", "#9370d8" },         { "mediumseagreen", "#3cb371" },
                { "mediumslateblue", "#7b68ee" },{ "mediumspringgreen", "#00fa9a" },    { "mediumturquoise", "#48d1cc" },
                { "mediumvioletred", "#c71585" },{ "midnightblue", "#191970" },         { "mintcream", "#f5fffa" },
                { "mistyrose", "#ffe4e1" },      { "moccasin", "#ffe4b5" },             { "navajowhite", "#ffdead" },
                { "oldlace", "#fdf5e6" },        { "olivedrab", "#6b8e23" },            { "orangered", "#ff4500" },
                { "palegoldenrod", "#eee8aa" },  { "palegreen", "#98fb98" },            { "paleturquoise", "#afeeee" },
                { "palevioletred", "#d87093" },  { "papayawhip", "#ffefd5" },           { "peachpuff", "#ffdab9" },
                { "powderblue", "#b0e0e6" },     { "rosybrown", "#bc8f8f" },            { "royalblue", "#4169e1" },
                { "saddlebrown", "#8b4513" },    { "sandybrown", "#f4a460" },           { "seagreen", "#2e8b57" },
                { "seashell", "#fff5ee" },       { "skyblue", "#87ceeb" },              { "slateblue", "#6a5acd" },
                { "slategray", "#708090" },      { "slategrey", "#708090" },            { "springgreen", "#00ff7f" },
                { "steelblue", "#4682b4" },      { "thistle", "#d8bfd8" },              { "turquoise", "#40e0d0" },
                { "white", "#fff" },             { "whitesmoke", "#f5f5f5" },           { "yellowgreen", "#9acd32" }
            };
        }

        /// <summary>
        /// Compresses a six character hexadecimal value to its 
        /// three character representation if possible.
        /// </summary>
        /// <param name="hex">A hexadecimal value.</param>
        /// <returns>
        /// If possible, returns the compressed hex value. 
        /// Otherwise, returns the original hex.
        /// </returns>
        public string CompressHex( string hex ) {
            if( hex.Length == 6 ) {
                if( hex[ 0 ] == hex[ 1 ] && hex[ 2 ] == hex[ 3 ] && hex[ 4 ] == hex[ 5 ] ) {
                    return ( "#" + hex[ 0 ] + hex[ 2 ] + hex[ 4 ] ).ToLower();
                }
            }

            return ( "#" + hex ).ToLower();
        }

        /// <summary>
        /// Swaps out a hexadecimal color value for its smaller literal color name.
        /// </summary>
        /// <param name="hex">A compressed hexadecimal value.</param>
        /// <returns>
        /// Literal color name, it a smaller one exists. Otherwise, returns the hex value.
        /// </returns>
        public string SwapOutHex( string hex ) {
            if( this._nameSmallerList.Keys.Contains( hex ) ) {
                return this._nameSmallerList[ hex ];
            }
            else {
                return hex;
            }
        }

        /// <summary>
        /// Swaps out a literal color name for its smaller hexadecimal color value.
        /// </summary>
        /// <param name="css">The string of the css file to parse.</param>
        /// <returns>
        /// The supplied string with the literal names converted to hex if possible.
        /// </returns>
        public string SwapOutNames( string css ) {
            foreach( var kvp in this._hexSmallerList ) {
                css = Regex.Replace( css, "(?<=[: ,])" + kvp.Key + "(?=[;,\\} \\)])", kvp.Value );
            }

            return css;
        }

        /// <summary>
        /// Converts an RGB color value to Hexadecimal.
        /// </summary>
        /// <param name="rgb">A byte array containing the R, G, B values</param>
        /// <returns>A hexadecimal value representing the supplied RGB values.</returns>
        public string ConvertRgbToHex( byte[] rgb ) {
            string s = BitConverter.ToString( rgb ).Replace( "-", string.Empty );

            return CompressHex( s.ToLower() );
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
                p = ( 2 * l ) - q;
                r = this.HueToRgb( p, q, h + ( 1.0 / 3.0 ) );
                g = this.HueToRgb( p, q, h );
                b = this.HueToRgb( p, q, h - ( 1.0 / 3.0 ) );
            }

            byte[] rgb = {
                             ( byte )Math.Round( r * 255 ),
                             ( byte )Math.Round( g * 255 ),
                             ( byte )Math.Round( b * 255 )
                         };

            return this.ConvertRgbToHex( rgb );
        }

        /// <summary>
        /// Factored out section of the ConvertHslToHex method.
        /// </summary>
        private double HueToRgb( double p, double q, double t ) {
            if( t < 0 ) { t += 1; }
            if( t > 1 ) { t -= 1; }

            if( t * 6.0 < 1.0 ) { return p + ( ( q - p ) * 6 * t ); }
            if( t * 2.0 < 1.0 ) { return q; }
            if( t * 3.0 < 2.0 ) { return p + ( ( q - p ) * 6 * ( ( 2.0 / 3.0 ) - t ) ); }

            return p;
        }
    }
}
