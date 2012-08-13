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
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Static class for some simple file operations.
    /// </summary>
    public static class FileOp {
        /// <summary>
        /// List to hold all of the files found by the ParseDirectory method.
        /// </summary>
        private static List<string> _fileList = new List<string>();

        /// <summary>
        /// Minifies a single file.
        /// </summary>
        /// <param name="path">Path of the CSS file to be minified.</param>
        /// <returns>A minified string version of the supplied CSS file.</returns>
        public static string MinifyFile( string path ) {
            string mini = string.Empty;

            if( File.Exists( path ) ) {
                try {
                    using( var sr = new StreamReader( path ) ) {
                        string css = sr.ReadToEnd();
                        sr.Close();
                        mini = Minify.MakeItUgly( css );
                    }
                }
                catch( IOException e ) {
                    Dialogs.ErrorDialog( e );
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

            try {
                foreach( string p in paths ) {
                    if( File.Exists( p ) ) {
                        using( var sr = new StreamReader( p ) ) {
                            string css = sr.ReadToEnd();
                            css.Trim();
                            sb.Append( "/*\n" );
                            sb.Append( " From " + new FileInfo( p ).Name + "\n" );
                            sb.Append( " ========================================================================== */\n\n" );
                            sb.Append( css );
                            sb.Append( "\n\n" );
                        }
                    }
                }
            }
            catch( IOException e ) {
                Dialogs.ErrorDialog( e );
            }

            return sb.ToString();
        }

        /// <summary>
        /// Retrieve the CSS files in the selected directory and all sub-directories.
        /// </summary>
        /// <param name="dir">Base directory.</param>
        /// <returns>A List&lt;&gt; of strings representing the CSS files in the directory/sub-directories</returns>
        public static List<string> GetCssFiles( string dir ) {
            _fileList.Clear();
            return ParseDirectory( dir );
        }

        /// <summary>
        /// Writes a project file to the supllied path using the dir and 
        /// list to populate the project file.
        /// </summary>
        /// <param name="path">Path to save the project file.</param>
        /// <param name="dir">String representing the last directory open in the project.</param>
        /// <param name="list">A list of files that were being watched.</param>
        public static void WriteProject( string path, string dir, List<string> list ) {
            try {
                // Create a new XDoc
                var xdoc = new XDocument();

                // Create the Project root element with the 
                // LastDir child element
                var proj = new XElement( "Project",
                        new XElement( "LastDir", dir )
                    );

                // Create the Files element that will contain the
                // project file list
                var files = new XElement( "Files" );

                // Add the Files children
                foreach( string s in list ) {
                    files.Add( new XElement( "Path", s ) );
                }

                // Add Files to the Project element
                proj.Add( files );

                // Add the whole tree to the document
                xdoc.Add( proj );

                // Save the document
                xdoc.Save( path );
            }
            catch( IOException e ) {
                Dialogs.ErrorDialog( e );
            }
        }

        /// <summary>
        /// Reads a project file from disks and returns the files to watch.
        /// </summary>
        /// <param name="dir">String representing the last directory open in the project.</param>
        /// <param name="path">The path of the project file.</param>
        /// <returns>An array of files to start watching.</returns>
        public static string[] ReadProject( out string dir, string path ) {
            if( File.Exists( path ) ) {
                try {
                    // Create the XDoc from the file
                    var xdoc = XDocument.Load( path );

                    // Get the LastDir value
                    dir = xdoc.Element( "Project" ).Element( "LastDir" ).Value;

                    // Get the Files descendants
                    var files = xdoc.Element( "Project" ).Descendants( "Files" );
                    var list = new List<string>();
                    var broken = new StringBuilder( "" );

                    // For each of the paths, check if it exists than
                    // add it to the list, otherwise add to the broken string
                    foreach( var f in files.Elements( "Path" ) ) {
                        if( File.Exists( f.Value ) ) {
                            list.Add( f.Value );
                        }
                        else {
                            broken.Append( f.Value + "\n" );
                        }
                    }

                    // Notify the user that there was broken links
                    if( broken.ToString() != string.Empty ) {
                        MessageBox.Show( "One or more files from the project could not be found\n\n" +
                                            broken.ToString() );
                    }

                    return list.ToArray();
                }
                catch( XmlException e ) {
                    Dialogs.ErrorDialog( e );
                }
            }

            // File doesnt exist
            dir = string.Empty;
            return null;
        }

        /// <summary>
        /// Recursively check each directory for .css files and add them to the list
        /// </summary>
        /// <param name="dir">The initial directory to start checking.</param>
        /// <returns>A list of .css file paths</returns>
        private static List<string> ParseDirectory( string dir ) {
            try {
                if( !String.IsNullOrEmpty( dir ) ) {
                    string[] files = Directory.GetFiles( dir );
                    foreach( var f in files ) {
                        if( new FileInfo( f ).Extension == ".css" ) {
                            _fileList.Add( f );
                        }
                    }

                    string[] subDir = Directory.GetDirectories( dir );
                    foreach( string d in subDir ) {
                        if( ( File.GetAttributes( d ) & FileAttributes.ReparsePoint ) != FileAttributes.ReparsePoint ) {
                            ParseDirectory( d );
                        }
                    }
                }
            }
            catch( Exception e ) {
                Dialogs.ErrorDialog( e );
            }

            return _fileList;
        }
    }
}