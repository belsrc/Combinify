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
    using System.IO;
    using System.Text;

    /// <summary>
    /// Static class for some simple file operations.
    /// </summary>
    public static class FileOp {

        private static List<string> fileList = new List<string>();

        /// <summary>
        /// Retrieve the CSS files in the selected directory and all sub-directories.
        /// </summary>
        /// <param name="dir">Base directory.</param>
        /// <returns>A List&lt;&gt; of strings representing the CSS files in the directory/sub-directories</returns>
        public static List<string> GetCssFiles( string dir ) {
            fileList.Clear();
            return ParseDirectory( dir );
        }

        // Recursively check each directory for .css files and add them to the list
        private static List<string> ParseDirectory( string dir ) {
            if( !String.IsNullOrEmpty( dir ) ) {
                string[] files = Directory.GetFiles( dir );
                foreach( var f in files ) {
                    if( new FileInfo( f ).Extension == ".css" ) {
                        fileList.Add( f );
                    }
                }

                string[] subDir = Directory.GetDirectories( dir );
                foreach( string d in subDir ) {
                    if( ( File.GetAttributes( d ) & FileAttributes.ReparsePoint ) != FileAttributes.ReparsePoint )
                        ParseDirectory( d );
                }
            }

            return fileList;
        }

        /// <summary>
        /// Minifies a single file.
        /// </summary>
        /// <param name="path">Path of the CSS file to be minified.</param>
        /// <returns>A minified string version of the supplied CSS file.</returns>
        public static string MinifyFile( string path ) {
            string mini = string.Empty;

            if( File.Exists( path ) ) {
                using( StreamReader sr = new StreamReader( path ) ) {
                    string css = sr.ReadToEnd();
                    sr.Close();
                    mini = Minify.MakeItUgly( css );
                }
            }

            return mini;
        }

        /// <summary>
        /// Minifies a collection of files.
        /// </summary>
        /// <param name="paths">A string List&lt;&gt; of file paths.</param>
        /// <returns>A minified string version of the supplied CSS files.</returns>
        public static string MinifyFile( List<string> paths ) {
            var sb = new StringBuilder();

            foreach( string s in paths ) {
                sb.Append( MinifyFile( s ) );
            }

            return sb.ToString();
        }

        /// <summary>
        /// Combines a collection of files.
        /// </summary>
        /// <param name="paths">A string List&lt;&gt; of file paths.</param>
        /// <returns>A combined string version of the supplied CSS files.</returns>
        public static string CombineFile( List<string> paths ) {
            var sb = new StringBuilder();

            foreach( string p in paths ) {
                if( File.Exists( p ) ) {
                    using( StreamReader sr = new StreamReader( p ) ) {
                        string css = sr.ReadToEnd();
                        css.Trim();
                        sb.Append( "/* ==========================================================================\n" );
                        sb.Append( "   From " + p.Substring( ( p.LastIndexOf( "\\" ) + 1 ) ) + "\n" );
                        sb.Append( "   ========================================================================== */\n\n" );
                        sb.Append( css );
                        sb.Append( "\n\n" );
                    }
                }
            }

            return sb.ToString();
        }
    }
}