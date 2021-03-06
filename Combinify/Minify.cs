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
/*
    Some RE's were pulled from Isaac Schlueter's rules list as is, 
    others (most) were shortened or modified in some manner and 
    some are new (where I thought I could go a little more in depth)
    
    Isaac's list can me found on github
    github.com/isaacs/cssmin/blob/master/rules.txt
*/
namespace QuickMinCombine {
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    /// <summary>
    /// A class to handle the minification of CSS files.
    /// </summary>
    public static class Minify {
        /// <summary>
        /// Arbitrary placeholder (thats not in the css spec) for the pre-pseudo class colons.
        /// </summary>
        private const string PSEUDO_REPLACE = "¬";

        /// <summary>
        /// Arbitrary placholder (thats not in the css spec) for the url() strings.
        /// </summary>
        private const string URL_REPLACE = "δ";

        /// <summary>
        /// Arbitrary placholder (thats not in the css spec) for the url(data:) strings.
        /// </summary>
        private const string DATA_REPLACE = "λ";

        /// <summary>
        /// Arbitrary placholder (thats not in the css spec) for the content: strings.
        /// </summary>
        private const string CONTENT_REPLACE = "ƒ";

        /// <summary>
        /// A string List to hold the url()'s from the css
        /// </summary>
        private static List<string> _urls = new List<string>();

        /// <summary>
        /// A string List to hold the url(data)'s from the css
        /// </summary>
        private static List<string> _data = new List<string>();

        /// <summary>
        /// A string List to hold the contents from the css
        /// </summary>
        private static List<string> _contents = new List<string>();

        /// <summary>
        /// Cleans/compresses several aspects of CSS code.
        /// </summary>
        /// <remarks>
        /// This should be everything...
        /// </remarks>
        /// <param name="css">The string value of the file(s).</param>
        /// <returns>A minified version of the supplied CSS string.</returns>
        public static string MakeItUgly( string css ) {
            var colors = new ColorConversions();

            // Clear the lists
            _urls.Clear();
            _data.Clear();
            _contents.Clear();

            // Insert placeholders
            SwapForPlaceholders( ref css );

            // Run through the rest of the minify methods
            InitialCleaning( ref css );
            CleanSelectors( ref css );
            CleanBraces( ref css );
            CleanUnnecessary( ref css );
            ConvertColors( ref css );
            CompressHexValues( ref css );
            css = colors.SwapOutNames( css );
            FixIllFormedHsl( ref css );

            // 'transparent' == rgba(0,0,0,0) == hsla(0,0%,0%,0)
            // Should be fine, if it supports Alpha should support the 'transparent' color literal
            // ...right?
            css = css.Replace( "rgba(0,0,0,0)", "transparent" )
                     .Replace( "hsla(0,0%,0%,0)", "transparent" );

            // Replace placeholders
            ReplacePlaceholders( ref css );

            // Return the string after trimming any leading or trailing spaces
            return css.Trim();
        }

        /// <summary>
        /// Replace some characters/strings that we dont want to run through the general minify methods.
        /// </summary>
        private static void SwapForPlaceholders( ref string source ) {
            // Pull out each css data url, do the needed minification, 
            // store in a list and put in a placeholder
            source = Regex.Replace( source, "(?<=[:;\\s]+)url\\(\\s?[\\'\\\\\"]?data:(.*?)[\\'\\\\\"]?" +
                                    "\\s?\\)(?=[;\\}\\s]+)", m => {
                                        _data.Add( CleanDataUrls( m.Value ) );
                                        return DATA_REPLACE;
                                    } );

            // Pull out each css url, do the needed minification, 
            // store in a list and put in a placeholder
            source = Regex.Replace( source,
                                    "(?<=[:;\\s]+)url\\(\\s?[\\'\\\\\"]?(?!\\w+:)(.*?)[\\'\\\\\"]?" +
                                    "\\s?\\)(?=[;\\}\\s]+)", m => {
                                        _urls.Add( CleanNormUrls( m.Value ) );
                                        return URL_REPLACE;
                                    } );

            // Replace pseudo-class colons with an arbitrary character
            source = Regex.Replace( source, "(^|\\})(([^\\{:])+:)+([^\\{]*\\{)", m => {
                return m.Value.Replace( ":", PSEUDO_REPLACE );
            } );

            // Pull out each css content property, do the needed minification, 
            // store in a list and put in a placeholder
            source = Regex.Replace( source, "(?<=content\\s*:).*?(?=;)", m => {
                _contents.Add( CleanContents( m.Value ) );
                return CONTENT_REPLACE;
            } );
        }

        /// <summary>
        /// Replace the characters/strings that we took out at the start
        /// </summary>
        private static void ReplacePlaceholders( ref string source ) {
            // Put url(data)'s back how they were
            for( int i = 0; i < _data.Count; i++ ) {
                source = ReplaceFirst( source, DATA_REPLACE, _data[ i ] );
            }

            // Put url()'s back how they were
            for( int i = 0; i < _urls.Count; i++ ) {
                source = ReplaceFirst( source, URL_REPLACE, _urls[ i ] );
            }

            // Put pseudo-class colons back how they were
            source = source.Replace( PSEUDO_REPLACE, ":" );

            // Put content:'s back how they were
            for( int i = 0; i < _contents.Count; i++ ) {
                source = ReplaceFirst( source, CONTENT_REPLACE, _contents[ i ] );
            }
        }

        /// <summary>
        /// Get rid of comment strings, 
        /// Get rid of newline characters, 
        /// Normalize remainig whitespace.
        /// </summary>
        private static void InitialCleaning( ref string source ) {
            source = Regex.Replace( source, "/\\*.+?\\*/", string.Empty, RegexOptions.Singleline );
            source = Regex.Replace( source, "([\\r\\n])*", string.Empty );
            source = Regex.Replace( source, "\\s+", " " );
        }

        /// <summary>
        /// Get rid of whitespaces around different punctuation. 
        /// ( : ; ,  * > + ~ = ^= $= *= |= ~= ! )
        /// </summary>
        private static void CleanSelectors( ref string source ) {
            source = Regex.Replace( source, "\\s*(?<Selector>(:|;|,|\\*|>|\\+|=|~|/|(\\^=)|" +
                                            "(\\$=)|(\\*=)|(\\|=)|(~=)|!))\\s*", "${Selector}" );
        }

        /// <summary>
        /// Get rid of white spaces around curly braces, 
        /// Clean up space immediately after opening, or before closing, parens and brackets.
        /// </summary>
        private static void CleanBraces( ref string source ) {
            // Since theres no special cases with them just grab all the spaces around them
            source = Regex.Replace( source, "\\s*(?<Brace>[\\{\\}])\\s*", "${Brace}" );
            source = Regex.Replace( source, "(?<Open>[\\(\\[])\\s+", "${Open}" );
            source = Regex.Replace( source, "\\s+(?<Close>[\\)\\]])", "${Close}" );
        }

        /// <summary>
        /// Get rid of last semi-colon before closing brace, 
        /// Get rid of leading zeros on decimals, 
        /// Get rid of measurements on zero values, 
        /// Shorten zero'd out values (margin: 0 0 0 0), 
        /// Set various borders to zero if they were previously 'none', 
        /// Shorten MS filter, 
        /// Remove unused rule-sets.
        /// </summary>
        private static void CleanUnnecessary( ref string source ) {
            source = Regex.Replace( source, ";}\\s*", "}" );
            source = Regex.Replace( source, "(?<=[:,\\s\\(])0+(?<Value>\\.\\d+)", "${Value}" );
            source = Regex.Replace( source, "(?<=[:,\\s\\(])0+(%|in|[cme]m|ex|p[tcx]|rem)", "0" );
            source = Regex.Replace( source, ":(0 0 0 0|0 0)(?=;|\\})", ":0" );
            source = Regex.Replace( source, "(?<Borders>(border(-top|-right|-bottom|-left)?" +
                                            "|outline|background)):none", "${Borders}:0" );

            // Removed the box|text shadow replacement since, apparently, its illegal set those to zero, 
            // which would be nice. Especially since you can do it with borders and a number of other props.
            // Hopefully it eventually changes so I can put it back in and save more space.

            source = Regex.Replace( source, "progid:DXImageTransform\\.Microsoft\\.Alpha\\(Opacity=", "alpha(opacity=" );
            source = Regex.Replace( source, "[^\\};\\{\\/]+\\{\\}", string.Empty );
        }

        /// <summary>
        /// Replace all rgb(#,#,#) and rgba(#,#,#,1) strings with their hex value, 
        /// Replace all hsl(#,#,#) and hsla(#,#,#,1) strings with their hex value.
        /// </summary>
        private static void ConvertColors( ref string source ) {
            var colors = new ColorConversions();
            char[] parens = { '(', ')' };
            string[] nums;

            source = Regex.Replace( source, "rgba?\\(\\d{1,3},\\d{1,3},\\d{1,3}(,1)?\\)", m => {
                nums = m.Value.Split( parens )[ 1 ].Split( ',' );
                try {
                    byte[] rgb = { 
                                 Byte.Parse( nums[ 0 ] ),
                                 Byte.Parse( nums[ 1 ] ),
                                 Byte.Parse( nums[ 2 ] )
                             };

                    return m.Value.Replace( m.Value, colors.ConvertRgbToHex( rgb ) );
                }
                catch( Exception e ) {
                    if( e is ArgumentNullException ||
                        e is FormatException ||
                        e is OverflowException ) {
                        MessageBox.Show( "Failed to parse " + m.Value, "Parse Error" );
                    }
                    else {
                        throw;
                    }
                }

                return m.Value;
            } );

            // Have to '{0,1}' the '%' since they may have been removed in a previous step
            source = Regex.Replace( source, "hsla?\\(\\d{1,3},\\d{1,3}%?,\\d{1,3}%?(,1)?\\)", m => {
                nums = m.Value.Replace( "%", string.Empty ).Split( parens )[ 1 ].Split( ',' );
                double h, s, l;

                try {
                    h = Double.Parse( nums[ 0 ] ) / 360;
                    s = Double.Parse( nums[ 1 ] ) / 100;
                    l = Double.Parse( nums[ 2 ] ) / 100;

                    return m.Value.Replace( m.Value, colors.ConvertHslToHex( h, s, l ) );
                }
                catch( Exception ex ) {
                    if( ex is ArgumentNullException ||
                        ex is FormatException ||
                        ex is OverflowException ) {
                        MessageBox.Show( "Failed to parse " + m.Value, "Parse Error" );
                    }
                    else {
                        throw;
                    }
                }

                return m.Value;
            } );
        }

        /// <summary>
        /// Match hex values that are 6 long, can ignore length 3 for now, and compress them down to 3 if possible, 
        /// Grab all the hex strings and see if the literal color name is shorter than the hex value, if so, replace it.
        /// </summary>
        private static void CompressHexValues( ref string source ) {
            var colors = new ColorConversions();
            string tmp = string.Empty;

            source = Regex.Replace( source, "#[0-9a-fA-F]{6}(?=,|;|\\}\\)\\s)", m => {
                tmp = colors.CompressHex( m.Value.Replace( "#", string.Empty ) );
                return m.Value.Replace( m.Value, tmp );
            } );

            source = Regex.Replace( source, "#([0-9a-fA-F]{3}){1,2}(?=,|;|\\}\\)\\s)", m => {
                tmp = colors.SwapOutHex( m.Value );
                return m.Value.Replace( m.Value, tmp );
            } );
        }

        /// <summary>
        /// Since I kind of...butcher hsla colors with 0 values I need to fix them
        /// Find all hsl/hsla colors with no '%' on the S and L values
        /// Split the match on the ','
        /// Since one may be good and the other not, strip '%'
        /// Put it back together with the '%'
        /// </summary>
        private static void FixIllFormedHsl( ref string source ) {
            source = Regex.Replace( source, "(?<=hsla?\\(\\d{1,3},)\\d{1,3}%?,\\d{1,3}%?" +
                                    "(?=(,(0|1)?(\\.\\d+)?)?\\))", m => {
                                        string[] tmp = m.Value.Split( ',' );
                                        return tmp[ 0 ].Replace( "%", string.Empty ) + "%," + tmp[ 1 ]
                                                       .Replace( "%", string.Empty ) + "%";
                                        // Kind of hack-ish I admit
                                    } );
        }

        /// <summary>
        /// Get rid of any spaces and quotes in the type declaration
        /// Since theres a possible charset (for fonts) Ill jump to the base64 and leave it at that
        /// Get rid of any space and quotes at the end
        /// </summary>
        private static string CleanDataUrls( string input ) {
            input = Regex.Replace( input, "^url\\s*\\(\\s*[\\\"']?data\\s*:\\s*(?<Type>.*?)\\s*;\\s*", "url(data:${Type};" );
            input = Regex.Replace( input, "\\s*base64\\s*,\\s*", "base64," );
            input = Regex.Replace( input, "[\\\"']?\\s*\\)$", ")" );

            return input;
        }

        /// <summary>
        /// Remove the quotes since they arent needed in url()'s
        /// Get rid of any space at the beginning
        /// Get rid of any space at the end
        /// </summary>
        private static string CleanNormUrls( string input ) {
            input = Regex.Replace( input, "[\"']", string.Empty );
            input = Regex.Replace( input, "^url\\s*\\(\\s*", "url(" );
            input = Regex.Replace( input, "\\s*\\)$", ")" );

            return input;
        }

        /// <summary>
        /// Remove quoted strings
        /// Remove remaining spaces
        /// Put quoted strings back
        /// </summary>
        private static string CleanContents( string input ) {
            List<string> quoted = new List<string>();
            string tmp = "_quote_";

            input = Regex.Replace( input, "\".*?\"", m => {
                quoted.Add( m.Value );
                return tmp;
            } );

            input = Regex.Replace( input, "\\s*", string.Empty );

            for( int i = 0; i < quoted.Count; i++ ) {
                input = ReplaceFirst( input, tmp, quoted[ i ] );
            }

            return input;
        }

        /// <summary>
        /// Returns a new string in which only the first occurrence of a specified string in 
        /// the current instance is replaced with another specified string.
        /// </summary>
        /// <param name="source">String source</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string to replace all occurrences of oldValue.</param>
        /// <returns>
        /// A string that is equivalent to the current string except that the first 
        /// instance of oldValue is replaced with newValue.
        /// </returns>
        /// <remarks>
        /// This method performs an ordinal (case-sensitive and culture-insensitive) 
        /// search to find oldValue. Original code from DotNetPerls - 
        /// www.dotnetperls.com/replace-extension
        /// </remarks>
        private static string ReplaceFirst( string source, string oldValue, string newValue ) {
            int index = source.IndexOf( oldValue );

            if( index == -1 )
                return source;

            int replacementLength = newValue.Length;
            int patternLength = oldValue.Length;
            int valueLength = source.Length;

            char[] array = new char[ valueLength + replacementLength - patternLength ];
            source.CopyTo( 0, array, 0, index );
            newValue.CopyTo( 0, array, index, replacementLength );
            source.CopyTo( index + patternLength, array, index + replacementLength, valueLength - ( index + patternLength ) );

            return new string( array );
        }
    }
}