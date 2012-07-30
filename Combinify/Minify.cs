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
    using System.Text.RegularExpressions;

    /// <summary>
    /// A class to handle the minification of CSS files.
    /// </summary>
    public static class Minify {
        /// <summary>
        /// Cleans/compresses several aspects of CSS code.
        /// </summary>
        /// <remarks>
        /// This should be everything...
        /// </remarks>
        /// <param name="contents">The string value of the file(s).</param>
        /// <returns>A minified version of the supplied CCS string.</returns>
        public static string MakeItUgly( string contents ) {
            string result = CleanCommWhitespace( contents );
            result = CleanSelectors( result );
            result = CleanBrackets( result );
            result = CleanQuotes( result );
            result = CleanUnnecessary( result );
            result = ConvertColors( result );
            result = CompressHex( result );

            // Return the string after trimming any leading or trailing spaces
            return result.Trim();
        }

        private static string CleanCommWhitespace( string s ) {
            string result = string.Empty;

            // Get rid of comment strings - need to check for base64 breakage
            result = Regex.Replace( s, "/\\*.+?\\*/", string.Empty, RegexOptions.Singleline );

            // Get rid of newline characters
            result = Regex.Replace( result, "([\\r\\n])*", string.Empty, RegexOptions.Singleline );

            return result;
        }

        private static string CleanSelectors( string s ) {
            string result = string.Empty;

            // Get rid of white spaces around punctuation ( : ; , )
            result = Regex.Replace( s, "\\s*(?<Punctuation>:|;|,)\\s*", "${Punctuation}", RegexOptions.Singleline );

            // Get rid of white spaces around attribute selectors ( ^= $= *= |= ~= = )
            result = Regex.Replace( result, "\\s*(?<AttrSelector>(\\^=)|(\\$=)|(\\*=)|(\\|=)|(~=)|=)\\s*", "${AttrSelector}", RegexOptions.Singleline );

            // Get rid of white spaces around combination selectors ( * > + ~ )
            result = Regex.Replace( result, "\\s*(?<CombSelector>\\*|>|\\+|~)\\s*", "${CombSelector}", RegexOptions.Singleline );

            return result;
        }

        private static string CleanBrackets( string s ) {
            string result = string.Empty;

            // Get rid of white spaces around parenthese and braces
            result = Regex.Replace( s, "\\s*(?<Brace>[\\(\\)\\{\\}])\\s*", "${Brace}", RegexOptions.Singleline );

            // Square brackets are a special case since you can have classes with descendant attribute selectors
            // Still kind of flawed ignores '.class [ attr' but I'd rather it be a few bytes bigger than broken...need to find a sure fix
            result = Regex.Replace( result, "\\s*(?<Right>(?<!\\w )\\[)\\s*", "${Right}", RegexOptions.Singleline );
            result = Regex.Replace( result, "\\s*(?<Left>\\])\\s*(?! \\w|\\.|#)", "${Left}", RegexOptions.Singleline );

            // Have to put the space back for media queries
            result = Regex.Replace( result, "and\\(", "and (", RegexOptions.Singleline );

            return result;
        }

        private static string CleanQuotes( string s ) {
            // Get rid of white spaces around quotation marks and apostrophes
            return Regex.Replace( s, "\\s*(?<Quotes>'|\\\")\\s*", "${Quotes}", RegexOptions.Singleline );
        }

        private static string CleanUnnecessary( string s ) {
            string result = string.Empty;

            // Get rid of last semi-colon before closing brace
            result = Regex.Replace( s, ";}\\s*", "}", RegexOptions.Singleline );

            // Get rid of leading zeros on decimals
            result = Regex.Replace( result, "(?<=(:|,))0+(?<Value>\\.\\d+)", "${Value}", RegexOptions.Singleline );

            // Get rid of white spaces infront of '!important'
            result = Regex.Replace( result, "\\s*!important", "!important", RegexOptions.Singleline );

            // Get rid of measurements on zero values
            result = Regex.Replace( result, "(?<=(:|,))0+(%|in|cm|mm|em|ex|pt|pc|px)", "0", RegexOptions.Singleline );

            // Get rid of zero'd out values (margin: 0 0 0 0)
            result = Regex.Replace( result, ":(0 0 0 0|0 0)(;|\\})", ":0", RegexOptions.Singleline );

            // Set various borders to zero if they were previously 'none'
            result = Regex.Replace( result, "(?<Borders>(border|border-top|border-right|border-bottom|border-left|outline|background)):none", "${Borders}:0", RegexOptions.Singleline );

            // Set various shadows to zero if they were previously 'none'
            result = Regex.Replace( result, "(?<Shadows>(box-shadow|text-shadow)):none", "${Shadows}:0", RegexOptions.Singleline );

            // Shorten MS filter
            result = Regex.Replace( result, "progid:DXImageTransform\\.Microsoft\\.Alpha\\(Opacity=", "alpha(opacity=", RegexOptions.Singleline );

            // Remove unused rule-sets
            result = Regex.Replace( result, "[^\\};\\{\\/]+\\{\\}", string.Empty, RegexOptions.Singleline );

            return result;
        }

        private static string ConvertColors( string str ) {
            string result = str;
            char[] parens = { '(', ')' };
            string[] nums;

            // Replace all rgb(x,x,x) and rgba(x,x,x,1) strings with their hex value
            MatchCollection rgbMatch = Regex.Matches( result, "(rgb\\(\\d{1,3},\\d{1,3},\\d{1,3}\\))|" +
                                                              "(rgba\\(\\d{1,3},\\d{1,3},\\d{1,3},1\\))" );
            foreach( Match rm in rgbMatch ) {
                nums = ( rm.Value ).Split( parens )[ 1 ].Split( ',' );
                int r, g, b;

                r = Int32.Parse( nums[ 0 ] );
                g = Int32.Parse( nums[ 1 ] );
                b = Int32.Parse( nums[ 2 ] );

                result = result.Replace( rm.Value, ConvertRgbToHex( r, g, b ) );
            }

            // Replace all hsl(x,x,x) and hsla(x,x,x,1) strings with their hex value
            // Have to '?' the '%' since they may have been removed in a previous step
            MatchCollection hslMatch = Regex.Matches( result, "(hsl\\(\\d{1,3},\\d{1,3}%?,\\d{1,3}%?\\))|" +
                                                              "(hsla\\(\\d{1,3},\\d{1,3}%?,\\d{1,3}%?,1\\))" );
            foreach( Match hm in hslMatch ) {
                nums = ( hm.Value ).Replace( "%", string.Empty ).Split( parens )[ 1 ].Split( ',' );
                double h, s, l;

                h = Double.Parse( nums[ 0 ] ) / 360;
                s = Double.Parse( nums[ 1 ] ) / 100;
                l = Double.Parse( nums[ 2 ] ) / 100;

                result = result.Replace( hm.Value, ConvertHslToHex( h, s, l ) );
            }

            return result;
        }

        private static string CompressHex( string s ) {
            string result = s;
            string tmp;

            // Match hex values that are 6 long, if they're 3 they're already compressed
            MatchCollection hexMatch = Regex.Matches( result, "#[0-9a-fA-F]{6}(?=,|;|\\})" );
            foreach( Match hm in hexMatch ) {
                tmp = hm.Value.Replace( "#", string.Empty );
                if( tmp[ 0 ] == tmp[ 1 ] && tmp[ 2 ] == tmp[ 3 ] && tmp[ 4 ] == tmp[ 5 ] ) {
                    result = result.Replace( hm.Value, "#" + tmp[ 0 ] + tmp[ 2 ] + tmp[ 4 ] );
                }
            }

            return result;
        }

        private static string ConvertRgbToHex( int r, int g, int b ) {
            return ( "#" + r.ToString( "X" ) + g.ToString( "X" ) + b.ToString( "X" ) ).ToLower();
        }

        /* Ported from http://mjijackson.com/2008/02/rgb-to-hsl-and-rgb-to-hsv-color-model-conversion-algorithms-in-javascript
         * Original comments.....
         * Converts an HSL color value to RGB. Conversion formula
         * adapted from http://en.wikipedia.org/wiki/HSL_color_space.
         * Assumes h, s, and l are contained in the set [0, 1] and
         * returns r, g, and b in the set [0, 255].
         */
        private static string ConvertHslToHex( double h, double s, double l ) {
            double r, g, b, q, p;

            if( s == 0 ) {
                r = g = b = l;
            }
            else {
                q = l < 0.5 ? l * ( 1 + s ) : l + s - l * s;
                p = 2 * l - q;
                r = HueToRgb( p, q, h + 1 / 3 );
                g = HueToRgb( p, q, h );
                b = HueToRgb( p, q, h - 1 / 3 );
            }

            return ConvertRgbToHex( ( int )Math.Round( r * 255 ), ( int )Math.Round( g * 255 ), ( int )Math.Round( b * 255 ) );
        }

        private static double HueToRgb( double p, double q, double t ) {
            if( t < 0 ) { t += 1; }
            if( t > 1 ) { t -= 1; }
            if( t < 1 / 6 ) { return p + ( q - p ) * 6 * t; }
            if( t < 1 / 2 ) { return q; }
            if( t < 2 / 3 ) { return p + ( q - p ) * ( 2 / 3 - t ) * 6; }

            return p;
        }
    }
}
