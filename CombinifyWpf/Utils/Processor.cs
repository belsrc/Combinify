// -------------------------------------------------------------------------------
//    FileOp.cs
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Cgum.Dialog;
    using CombinifyWpf.Models;

    /// <summary>
    /// Class for processing the CSS files.
    /// </summary>
    public class Processor {

        private Minifier _min;
        private Flattener _flat;
        private SelectorCombiner _combine;
        private Comber _comb;

        /// <summary>
        /// Initializes a new instance of the Processor class.
        /// </summary>
        public Processor() {
            this._min = new Minifier();
            this._flat = new Flattener();
            this._combine = new SelectorCombiner();
            this._comb = new Comber();
        }

        /// <summary>
        /// Processes the CSS files using the supplied settings.
        /// </summary>
        /// <param name="files">A list of CSS file paths.</param>
        /// <param name="settings">A settings object to use to determine how the file are to be processed.</param>
        /// <returns>A processes CSS string value.</returns>
        public string ProcessFiles( IEnumerable<string> files ) {
            string css = string.Empty;

            if( Properties.Settings.Default.Flatten ) {
                var flat = FlattenFiles( files );
                css = CombineFiles( flat );
            }
            else {
                css = CombineFiles( files );
            }

            if( Properties.Settings.Default.Combine ) {
                css = this._combine.CombineSelectors( css );
            }

            if( Properties.Settings.Default.Comb ) {
                css = CombFile( css );
            }

            if( Properties.Settings.Default.Minify ) {
                css = MinifyFile( css );
            }

            return css;
        }

        /// <summary>
        /// Combines a collection of files.
        /// </summary>
        /// <param name="content">An array of the CSS file content.</param>
        /// <returns>A combined string version of the supplied CSS files.</returns>
        public string CombineFiles( params string[] content ) {
            content = content.Select( s => s.Trim() ).ToArray();
            return string.Join( "\n", content );
        }

        /// <summary>
        /// Combines a collection of files.
        /// </summary>
        /// <param name="paths">A string List&lt;&gt; of file paths.</param>
        /// <returns>A combined string version of the supplied CSS files.</returns>
        public string CombineFiles( IEnumerable<string> paths ) {
            var sb = new StringBuilder();

            try {
                foreach( string p in paths ) {
                    if( File.Exists( p ) ) {
                        using( var sr = new StreamReader( p ) ) {
                            string css = sr.ReadToEnd();
                            css.Trim();
                            sb.Append( css );
                            sb.Append( "\n" );
                        }
                    }
                }
            }
            catch( IOException e ) {
                Dialogs.ShowError( e );
            }

            return sb.ToString();
        }

        /// <summary>
        /// Minifies a single file.
        /// </summary>
        /// <param name="path">Path of the CSS file to be minified.</param>
        /// <returns>A minified string version of the supplied CSS file.</returns>
        public string MinifyFile( string content ) {
            return this._min.MakeItUgly( content );
        }

        /// <summary>
        /// Minifies a collection of files.
        /// </summary>
        /// <param name="paths">A string List&lt;&gt; of file paths.</param>
        /// <returns>A minified string version of the supplied CSS files.</returns>
        public string MinifyFiles( IEnumerable<string> paths ) {
            var sb = new StringBuilder();

            try {
                foreach( string s in paths ) {
                    if( File.Exists( s ) ) {
                        using( var sr = new StreamReader( s ) ) {
                            string css = sr.ReadToEnd();
                            sr.Close();
                            sb.Append( MinifyFile( css ) );
                        }
                    }
                }
            }
            catch( IOException e ) {
                Dialogs.ShowError( e );
            }

            return sb.ToString();
        }

        /// <summary>
        /// Flattens the import statements in the provided CSS file.
        /// </summary>
        /// <param name="path">The CSS file path.</param>
        /// <returns>A flattened string representation of the CSS file.</returns>
        public string FlattenFile( string path ) {
            string dir = Path.GetDirectoryName( path );
            if( File.Exists( path ) ) {
                string css = string.Empty;

                using( var sr = new StreamReader( path ) ) {
                    css = sr.ReadToEnd();
                    css.Trim();
                }

                return this._flat.FlattenImports( css, dir );
            }

            return string.Empty;
        }

        /// <summary>
        /// Flattens the import statements in the provided list of CSS files.
        /// </summary>
        /// <param name="paths">A list of CSS file paths.</param>
        /// <returns>An array of flattened string representations of the CSS files.</returns>
        public string[] FlattenFiles( IEnumerable<string> paths ) {
            var css = new List<string>();
            foreach( var file in paths ) {
                css.Add( FlattenFile( file ) );
            }

            return css.ToArray();
        }

        /// <summary>
        /// Combs the CSS attributes in the provided CSS file.
        /// </summary>
        /// <param name="path">The CSS file path.</param>
        /// <returns>A combed string representation of the CSS file.</returns>
        public string CombFile( string content ) {
            return this._comb.CombFile( content );
        }

        /// <summary>
        /// Combs the CSS attributes in the provided list of CSS files.
        /// </summary>
        /// <param name="paths">A list of CSS file paths.</param>
        /// <returns>An array of combed string representations of the CSS files.</returns>
        public string[] CombFiles( IEnumerable<string> paths ) {
            var css = new List<string>();
            foreach( var file in paths ) {
                if( File.Exists( file ) ) {
                    using( var sr = new StreamReader( file ) ) {
                        css.Add( CombFile( sr.ReadToEnd() ) );
                    }
                }
            }

            return css.ToArray();
        }

        /// <summary>
        /// Combines duplicate CSS selectors in the provided CSS file.
        /// </summary>
        /// <param name="path">The CSS file path.</param>
        /// <returns>A string representation of the CSS file with the duplicate selectors combined.</returns>
        public string CombineSelectors( string content ) {
            return this._combine.CombineSelectors( content );
        }

        /// <summary>
        /// Combines duplicate CSS selectors in the provided list of CSS files.
        /// </summary>
        /// <param name="paths">A list of CSS file paths.</param>
        /// <returns>An array of string representations of the CSS files with the duplicate selectors combined..</returns>
        public string[] CombineSelectors( IEnumerable<string> paths ) {
            var css = new List<string>();
            foreach( var file in paths ) {
                if( File.Exists( file ) ) {
                    using( var sr = new StreamReader( file ) ) {
                        css.Add( CombineSelectors( sr.ReadToEnd() ) );
                    }
                }
            }

            return css.ToArray();
        }
    }
}