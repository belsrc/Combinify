// -------------------------------------------------------------------------------
//    Minify.cs
//    Copyright (c) 2012 Bryan Kizer
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
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using ModernUIDialogs;

    /// <summary>
    /// A class to handle the minification of CSS files.
    /// </summary>
    /// <remarks>
    /// Some RE's were pulled from Isaac Schlueter's rules list as is, 
    /// others (most) were shortened or modified in some manner and 
    /// some are new (where I thought I could go a little more in depth)
    /// 
    /// Isaac's list can me found on github - 
    /// github.com/isaacs/cssmin/blob/master/rules.txt
    /// </remarks>
    public class Minifier {
        // Placeholders for values we don't want to run through the minifier.
        private const string PSEUDO_REPLACE = "___PSEUDO_REPLACE___";
        private const string URL_REPLACE = "___URL_REPLACE___";
        private const string DATA_REPLACE = "___DATA_REPLACE___";
        private const string CONTENT_REPLACE = "___CONTENT_REPLACE___";
        private const string IMPORTANT_COM = "___IMPORTANT_COM___";

        // Lists for holding the replaced values.
        private List<string> _urls = new List<string>();
        private List<string> _data = new List<string>();
        private List<string> _contents = new List<string>();
        private List<string> _comments = new List<string>();

        /// <summary>
        /// Cleans/compresses several aspects of CSS code.
        /// </summary>
        /// <remarks>
        /// This should be everything...
        /// </remarks>
        /// <param name="css">The string value of the file(s).</param>
        /// <returns>A minified version of the supplied CSS string.</returns>
        public string MakeItUgly( string css ) {
            var colors = new ColorCompressor();

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

        // Replace some characters/strings that we dont want to run through the general minify methods.
        private void SwapForPlaceholders( ref string source ) {
            // data url
            source = Regex.Replace( source,
                                    "(?<=[:;\\s]+)url\\(\\s?[\\'\\\\\"]?data:(.*?)[\\'\\\\\"]?\\s?\\)(?=[;\\}\\s]+)",
                                    m => {
                                        _data.Add( CleanDataUrls( m.Value ) );
                                        return DATA_REPLACE;
                                    } );

            // url
            source = Regex.Replace( source,
                                    "(?<=[:;\\s]+)url\\(\\s?[\\'\\\\\"]?(?!\\w+:)(.*?)[\\'\\\\\"]?\\s?\\)(?=[;\\}\\s]+)",
                                    m => {
                                        _urls.Add( CleanNormUrls( m.Value ) );
                                        return URL_REPLACE;
                                    } );

            // pseudo-class colon
            source = Regex.Replace( source,
                                    "(^|\\})(([^\\{:])+:)+([^\\{]*\\{)",
                                    m => {
                                        return m.Value.Replace( ":", PSEUDO_REPLACE );
                                    } );

            // content
            source = Regex.Replace( source,
                                    "(?<=content\\s*:).*?(?=;)",
                                    m => {
                                        _contents.Add( CleanContents( m.Value ) );
                                        return CONTENT_REPLACE;
                                    } );

            // important comment /*!  */
            source = Regex.Replace( source,
                                    "/\\*!.+?\\*/",
                                    m => {
                                        _comments.Add( Regex.Replace( m.Value, "\\s+", " " ) );
                                        return IMPORTANT_COM;
                                    },
                                    RegexOptions.Singleline );
        }

        // Replace the characters/strings that we took out at the start
        private void ReplacePlaceholders( ref string source ) {
            // url(data)
            for( int i = 0; i < _data.Count; i++ ) {
                source = ReplaceFirst( source, DATA_REPLACE, _data[ i ] );
            }

            // url()
            for( int i = 0; i < _urls.Count; i++ ) {
                source = ReplaceFirst( source, URL_REPLACE, _urls[ i ] );
            }

            // pseudo-class colons
            source = source.Replace( PSEUDO_REPLACE, ":" );

            // content:
            for( int i = 0; i < _contents.Count; i++ ) {
                source = ReplaceFirst( source, CONTENT_REPLACE, _contents[ i ] );
            }

            // important comments
            for( int i = 0; i < _comments.Count; i++ ) {
                source = ReplaceFirst( source, IMPORTANT_COM, _comments[ i ] );
            }
        }

        // Get rid of comment strings, Get rid of newline characters, Normalize remainig whitespace.
        private void InitialCleaning( ref string source ) {
            source = Regex.Replace( source, "/\\*.+?\\*/", string.Empty, RegexOptions.Singleline );
            source = Regex.Replace( source, "([\\r\\n])*", string.Empty );
            source = Regex.Replace( source, "\\s+", " " );
        }

        // Get rid of whitespaces around operators and separators. ( : ; ,  * > + ~ = ^= $= *= |= ~= ! )
        private void CleanSelectors( ref string source ) {
            source = Regex.Replace( source,
                                    "\\s*(?<Selector>(:|;|,|\\*|>|\\+|=|~|/|(\\^=)|(\\$=)|(\\*=)|(\\|=)|(~=)|!))\\s*",
                                    "${Selector}"
                                  );
        }

        // Get rid of white spaces around curly braces, parens and brackets.
        private void CleanBraces( ref string source ) {
            // Since theres no special cases with them just grab all the spaces around them
            source = Regex.Replace( source, "\\s*(?<Brace>[\\{\\}])\\s*", "${Brace}" );
            source = Regex.Replace( source, "(?<Open>[\\(\\[])\\s+", "${Open}" );
            source = Regex.Replace( source, "\\s+(?<Close>[\\)\\]])", "${Close}" );
        }

        // Get rid of last semi-colon before closing brace, leading zeros on decimals, measurements on zero values, 
        // shorten zero'd out values (margin: 0 0 0 0), set various borders to zero if they were previously 'none', 
        // shorten MS filter, remove unused rule-sets.
        private void CleanUnnecessary( ref string source ) {
            source = Regex.Replace( source, ";}\\s*", "}" );
            source = Regex.Replace( source, "(?<=[:,\\s\\(])0+(?<Value>\\.\\d+)", "${Value}" );
            source = Regex.Replace( source, "(?<=[:,\\s\\(])0+(%|in|[cme]m|ex|p[tcx]|rem)", "0" );
            source = Regex.Replace( source, ":(0 0 0 0|0 0)(?=;|\\})", ":0" );
            source = Regex.Replace( source,
                                    "(?<Borders>(border(-top|-right|-bottom|-left)?|outline|background)):none",
                                    "${Borders}:0"
                                  );

            // Removed the box|text shadow replacement since, apparently, its illegal to set those to zero, 
            // which would be nice. Especially since you can do it with borders and a number of other props.
            // Hopefully it eventually changes so I can put it back in and save more space.

            source = Regex.Replace( source, "progid:DXImageTransform\\.Microsoft\\.Alpha\\(Opacity=", "alpha(opacity=" );
            source = Regex.Replace( source, "[^\\};\\{\\/]+\\{\\}", string.Empty );
        }

        // Replace all rgb|hsl(#,#,#) and rgba|hsla(#,#,#,1) strings with their hex value.
        private void ConvertColors( ref string source ) {
            var colors = new ColorCompressor();
            char[] parens = { '(', ')' };
            string[] nums;

            source = Regex.Replace( source,
                                    "rgba?\\(\\d{1,3},\\d{1,3},\\d{1,3}(,1)?\\)",
                                    m => {
                                        nums = m.Value.Split( parens )[ 1 ].Split( ',' );
                                        try {
                                            byte[] rgb = { 
                                                            Byte.Parse( nums[ 0 ] ),
                                                            Byte.Parse( nums[ 1 ] ),
                                                            Byte.Parse( nums[ 2 ] )
                                                        };

                                            return m.Value.Replace( m.Value, colors.CompressRgb( rgb ) );
                                        }
                                        catch( Exception e ) {
                                            if( e is ArgumentNullException ||
                                                e is FormatException ||
                                                e is OverflowException ) {
                                                MetroMessageBox.Show( "Failed to parse " + m.Value, "Parse Error" );
                                            }
                                            else {
                                                throw;
                                            }
                                        }

                                        return m.Value;
                                    } );

            // Have to '{0,1}' the '%' since they may have been removed in a previous step
            source = Regex.Replace( source,
                                    "hsla?\\(\\d{1,3},\\d{1,3}%?,\\d{1,3}%?(,1)?\\)",
                                    m => {
                                        nums = m.Value.Replace( "%", string.Empty ).Split( parens )[ 1 ].Split( ',' );
                                        float h, s, l;

                                        try {
                                            h = float.Parse( nums[ 0 ] );
                                            s = float.Parse( nums[ 1 ] );
                                            l = float.Parse( nums[ 2 ] );

                                            return m.Value.Replace( m.Value, colors.CompressHsl( h, s, l ) );
                                        }
                                        catch( Exception ex ) {
                                            if( ex is ArgumentNullException ||
                                                ex is FormatException ||
                                                ex is OverflowException ) {
                                                MetroMessageBox.Show( "Failed to parse " + m.Value, "Parse Error" );
                                            }
                                            else {
                                                throw;
                                            }
                                        }

                                        return m.Value;
                                    } );
        }

        // Match hex values that are 6 long and compress them down to 3 if possible, 
        // Grab all the hex strings and replace with the literal color name if is shorter than the hex value.
        private void CompressHexValues( ref string source ) {
            var colors = new ColorCompressor();
            string tmp = string.Empty;

            source = Regex.Replace( source,
                                    "#[0-9a-fA-F]{6}(?=,|;|\\}\\)\\s)",
                                    m => {
                                        tmp = colors.CompressHex( m.Value.Replace( "#", string.Empty ) );
                                        return m.Value.Replace( m.Value, tmp );
                                    } );

            source = Regex.Replace( source,
                                    "#([0-9a-fA-F]{3}){1,2}(?=,|;|\\}\\)\\s)",
                                    m => {
                                        tmp = colors.SwapOutHex( m.Value );
                                        return m.Value.Replace( m.Value, tmp );
                                    } );
        }

        // Since I kind of...butcher hsla colors with 0 values I need to fix them
        // Find all hsl/hsla colors with no '%' on the S and L values
        // Split the match on the ','
        // Since one may be good and the other not, strip '%'
        // Put it back together with the '%'
        private void FixIllFormedHsl( ref string source ) {
            source = Regex.Replace( source,
                                    "(?<=hsla?\\(\\d{1,3},)\\d{1,3}%?,\\d{1,3}%?(?=(,(0|1)?(\\.\\d+)?)?\\))",
                                    m => {
                                        string[] tmp = m.Value.Split( ',' );
                                        return tmp[ 0 ].Replace( "%", string.Empty ) + "%," + tmp[ 1 ]
                                                       .Replace( "%", string.Empty ) + "%";
                                        // Kind of hack-ish I admit
                                    } );
        }

        // Get rid of any spaces and quotes in the type declaration
        // Since theres a possible charset (for fonts) Ill jump to the base64 and leave it at that,
        // Get rid of any space and quotes at the end
        private string CleanDataUrls( string input ) {
            input = Regex.Replace( input, "^url\\s*\\(\\s*[\\\"']?data\\s*:\\s*(?<Type>.*?)\\s*;\\s*", "url(data:${Type};" );
            input = Regex.Replace( input, "\\s*base64\\s*,\\s*", "base64," );
            input = Regex.Replace( input, "[\\\"']?\\s*\\)$", ")" );

            return input;
        }

        // Remove the quotes since they arent needed in url()'s
        // Get rid of any space at the beginning or end
        private string CleanNormUrls( string input ) {
            input = Regex.Replace( input, "[\"']", string.Empty );
            input = Regex.Replace( input, "^url\\s*\\(\\s*", "url(" );
            input = Regex.Replace( input, "\\s*\\)$", ")" );

            return input;
        }

        // Remove quoted strings, remaining spaces, put quoted strings back
        private string CleanContents( string input ) {
            List<string> quoted = new List<string>();
            string tmp = "_quote_";

            input = Regex.Replace( input,
                                    "\".*?\"",
                                    m => {
                                        quoted.Add( m.Value );
                                        return tmp;
                                    } );

            input = Regex.Replace( input, "\\s*", string.Empty );

            for( int i = 0; i < quoted.Count; i++ ) {
                input = ReplaceFirst( input, tmp, quoted[ i ] );
            }

            return input;
        }

        // Replace the first occurrence of a specified string  
        // This method performs an ordinal (case-sensitive and culture-insensitive) 
        // search to find oldValue. Original code from DotNetPerls - 
        // www.dotnetperls.com/replace-extension
        private string ReplaceFirst( string source, string oldValue, string newValue ) {
            int index = source.IndexOf( oldValue );

            if( index == -1 ) {
                return source;
            }

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