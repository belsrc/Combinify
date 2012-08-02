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
            var colors = new ColorConversions();

            string result = CleanCommWhitespace( contents );
            result = CleanSelectors( result );
            result = CleanBrackets( result );
            result = CleanUnnecessary( result );
            result = ConvertColors( result );
            result = CompressHexValues( result );

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
            // Had a big ugly regex for matching parens but apparently its just fine if there is no space
            // after the closing parent in the background shorthand...yay more savings?
            result = Regex.Replace( s, "\\s*(?<Brace>[\\(\\)\\{\\}])\\s*", "${Brace}", RegexOptions.Singleline );

            // Square brackets are a special case since you can have classes with descendant attribute selectors
            // Still kind of flawed ignores '.class [ attr' but I'd rather it be a few bytes bigger than broken...need to find a sure fix
            result = Regex.Replace( result, "\\s*(?<Right>(?<!\\w )\\[)\\s*", "${Right}", RegexOptions.Singleline );
            result = Regex.Replace( result, "\\s*(?<Left>\\])\\s*(?! \\w|\\.|#)", "${Left}", RegexOptions.Singleline );

            // Have to put the space back for media queries
            result = Regex.Replace( result, "and\\(", "and (", RegexOptions.Singleline );

            return result;
        }

        private static string CleanUnnecessary( string s ) {
            string result = string.Empty;

            // Get rid of last semi-colon before closing brace
            result = Regex.Replace( s, ";}\\s*", "}", RegexOptions.Singleline );

            // Get rid of leading zeros on decimals
            result = Regex.Replace( result, "(?<=(:|,| ))0+(?<Value>\\.\\d+)", "${Value}", RegexOptions.Singleline );

            // Get rid of white spaces infront of '!important'
            result = Regex.Replace( result, "\\s*!important", "!important", RegexOptions.Singleline );

            // Get rid of measurements on zero values
            result = Regex.Replace( result, "(?<=(:|,))0+(%|in|cm|mm|em|ex|pt|pc|px)", "0", RegexOptions.Singleline );

            // Get rid of zero'd out values (margin: 0 0 0 0)
            result = Regex.Replace( result, ":(0 0 0 0|0 0)(;|\\})", ":0", RegexOptions.Singleline );

            // Set various borders to zero if they were previously 'none'
            result = Regex.Replace( result, "(?<Borders>(border|border-top|border-right|border-bottom|border-left|outline|background)):none", "${Borders}:0", RegexOptions.Singleline );

            // Removed the box|text shadow replacement since, apparently, you cant set those to zero, which would be nice. Especially since you can do
            // it with borders and a number of other props. Hopefully it eventually gets approved so I can put it back and get more space back.

            // Shorten MS filter
            result = Regex.Replace( result, "progid:DXImageTransform\\.Microsoft\\.Alpha\\(Opacity=", "alpha(opacity=", RegexOptions.Singleline );

            // Remove unused rule-sets
            result = Regex.Replace( result, "[^\\};\\{\\/]+\\{\\}", string.Empty, RegexOptions.Singleline );

            return result;
        }

        private static string ConvertColors( string str ) {
            var colors = new ColorConversions();
            string result = str;
            char[] parens = { '(', ')' };
            string[] nums;

            // Replace all rgb(x,x,x) and rgba(x,x,x,1) strings with their hex value
            MatchCollection rgbMatch = Regex.Matches( result, "(rgb\\(\\d{1,3},\\d{1,3},\\d{1,3}\\))|" +
                                                              "(rgba\\(\\d{1,3},\\d{1,3},\\d{1,3},1\\))" );
            foreach( Match rm in rgbMatch ) {
                nums = ( rm.Value ).Split( parens )[ 1 ].Split( ',' );
                
                byte[] rgb = { Byte.Parse( nums[ 0 ] ),
                               Byte.Parse( nums[ 1 ] ),
                               Byte.Parse( nums[ 2 ] ) };

                result = result.Replace( rm.Value, colors.ConvertRgbToHex( rgb ) );
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

                result = result.Replace( hm.Value, colors.ConvertHslToHex( h, s, l ) );
            }

            return result;
        }

        private static string CompressHexValues( string str ) {
            var colors = new ColorConversions();
            string result = str;
            string tmp = string.Empty;

            // Match hex values that are 6 long, if they're 3 they're already compressed
            MatchCollection hexMatch = Regex.Matches( result, "#[0-9a-fA-F]{6}(?=,|;|\\})" );
            foreach( Match hm in hexMatch ) {
                tmp = colors.CompressHex( hm.Value.Replace( "#", string.Empty ) );
                tmp = colors.SwapOutHex( tmp );

                result = result.Replace( hm.Value, tmp );
            }

            return result;
        }
    }
}
