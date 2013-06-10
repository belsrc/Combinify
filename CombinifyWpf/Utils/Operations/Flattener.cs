// -------------------------------------------------------------------------------
//    Flattener.cs
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
    using System;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Text;

    /// <summary>
    /// A class to handle (recursively) flattening all import statements of a CSS files.
    /// </summary>
    public class Flattener {

        /// <summary>
        /// Initializes a new instance of the Flattener class.
        /// </summary>
        public Flattener() { }

        /// <summary>
        /// Flattens all @import statements in the CSS file.
        /// </summary>
        /// <param name="css">The content of the CSS file.</param>
        /// <param name="dirPath">The local directory path of the CSS file to use as a relative base.</param>
        /// <returns>A flattened css fiel string.</returns>
        public string FlattenImports( string css, string dirPath ) {
            css = Regex.Replace( css, "@import\\s*?.*?;", m => {
                string path;
                string content;

                if( m.Value.Contains( "http://" ) || m.Value.Contains( "https://" ) ) {
                    content = ImportFromWeb( m.Value, dirPath );
                }
                else {
                    path = GetFilePath( m.Value, dirPath );
                    content = ImportFromFile( path );
                }

                if( string.IsNullOrEmpty( content ) ) {
                    return m.Value;
                }

                return content;
            } );

            return css;
        }

        // Break down the import statement to get the path to the included file.
        private string StripImport( string import ) {
            string tmp = Regex.Replace( import, "@import\\s*?'|\"", string.Empty );
            tmp = Regex.Replace( tmp, "['|\"]\\s*?;", string.Empty );
            return tmp.Trim();
        }

        // Builds the local file directory path.
        private string GetFilePath( string import, string relativeBase ) {
            string tmp = StripImport( import );
            tmp = tmp.Replace( '/', '\\' );

            return Path.Combine( relativeBase, tmp );
        }

        // Get the content of the included file from the local file system.
        private string ImportFromFile( string path ) {
            string content = string.Empty;
            FileInfo fi = new FileInfo( path );

            if( fi.Exists ) {
                using( var reader = new StreamReader( fi.FullName, Encoding.UTF8 ) ) {
                    content = reader.ReadToEnd();
                }
            }

            // Since there is the possibility that the imported css file may
            // also have import statements, we need to keep calling FlattenImports
            // on the files until we import them all
            return FlattenImports( content, fi.DirectoryName );
        }

        // Get the content of the included file from the web address.
        // The localDir arg is merely used to not break recursion at this time.
        private string ImportFromWeb( string url, string localDir ) {
            string content = string.Empty;
            Uri uri = new Uri( StripImport( url ) );

            if( WebFileExists( uri ) ) {
                using( WebClient client = new WebClient() ) {
                    using( StreamReader reader = new StreamReader( client.OpenRead( uri ), Encoding.UTF8 ) ) {
                        content = reader.ReadToEnd();
                    }
                }
            }

            return FlattenImports( content, localDir );
        }

        // Checks to see if a file exists on the web.
        private bool WebFileExists( Uri url ) {
            try {
                HttpWebRequest request = WebRequest.Create( url ) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                return ( response.StatusCode == HttpStatusCode.OK );
            }
            catch {
                return false;
            }
        }
    }
}