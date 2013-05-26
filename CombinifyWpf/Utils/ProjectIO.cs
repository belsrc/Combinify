// -------------------------------------------------------------------------------
//    ProjectIO.cs
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
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using CombinifyWpf.Models;
    using ModernUIDialogs;

    /// <summary>
    /// Class for handling project input and output.
    /// </summary>
    public class ProjectIO {

        private List<string> _fileList = new List<string>();

        /// <summary>
        /// Initializes a new instance of the ProjectIO class.
        /// </summary>
        public ProjectIO() { }

        /// <summary>
        /// Writes the project file to the supllied path.
        /// </summary>
        /// <param name="projectPath">Path to save the project file.</param>
        public void SaveProject( string projectPath, Project project ) {
            try {
                var xdoc = new XDocument();

                // Create the Project root element with the 
                // LastDir child element and the
                // Destination child element
                var proj = new XElement( "Project",
                                new XElement( "LastDir", project.LastDirectory ),
                                new XElement( "Destination", project.DestinationFile )
                            );

                // Create the Files element that will contain the
                // project file list
                var files = new XElement( "Files" );

                // Add the Files children
                foreach( string s in project.WatchedFiles ) {
                    files.Add( new XElement( "Path", s ) );
                }

                // Add Files to the Project element
                proj.Add( files );

                // Add the whole tree to the document
                xdoc.Add( proj );

                // Save the document
                xdoc.Save( projectPath, SaveOptions.None );
            }
            catch( IOException e ) {
                MetroMessageBox.ShowError( e );
            }
        }

        /// <summary>
        /// Reads a project file from the disk.
        /// </summary>
        /// <param name="projectPath">The path of the project file.</param>
        /// <returns>A Project object.</returns>
        public Project LoadProject( string projectPath ) {
            Project project = new Project();
            if( File.Exists( projectPath ) ) {
                try {
                    var xdoc = XDocument.Load( projectPath );

                    // Get the LastDir value
                    project.LastDirectory = xdoc.Element( "Project" ).Element( "LastDir" ).Value;

                    // Get the Destination value if newer version else string.Empty
                    // This way it will maintain backward compatibility
                    if( xdoc.Element( "Project" ).Element( "Destination" ) != null ) {
                        project.DestinationFile = xdoc.Element( "Project" ).Element( "Destination" ).Value;
                    }
                    else {
                        project.DestinationFile = string.Empty;
                    }

                    // Get the Files descendants
                    var files = xdoc.Element( "Project" ).Descendants( "Files" );

                    // StringBuilder for broken file links
                    StringBuilder broken = new StringBuilder( string.Empty );

                    // Temp file list
                    List<string> temp = new List<string>();

                    // For each of the paths, check if it exists than
                    // add it to the list, otherwise add to the broken string
                    foreach( var f in files.Elements( "Path" ) ) {
                        if( File.Exists( f.Value ) ) {
                            temp.Add( f.Value );
                        }
                        else {
                            broken.Append( f.Value + "\n" );
                        }
                    }

                    // Set the watched files
                    project.WatchedFiles = temp;

                    // Notify the user that there was broken links
                    if( !string.IsNullOrEmpty( broken.ToString() ) ) {
                        MetroMessageBox.Show( "One or more files from the project could not be found\n\n" +
                                            broken.ToString() );
                    }
                }
                catch( XmlException e ) {
                    MetroMessageBox.ShowError( e );
                }
            }

            return project;
        }

        /// <summary>
        /// Writes the processed CSS file(s) to disk.
        /// </summary>
        /// <param name="destination">The destination file path.</param>
        /// <param name="content">The file content to write.</param>
        public void SaveProcessedFile( string destination, string content ) {
            using( var sw = new StreamWriter( destination, false ) ) {
                sw.Write( content );
                sw.Close();
            }
        }

        /// <summary>
        /// Retrieve the CSS files in the selected directory and all sub-directories.
        /// </summary>
        /// <param name="dir">Base directory.</param>
        /// <returns>A List&lt;&gt; of strings representing the CSS files in the directory/sub-directories</returns>
        public List<string> GetCssFiles( string dir ) {
            _fileList.Clear();
            return ParseDirectory( dir );
        }

        // Recursively check each directory, starting from specified, for .css files and add them to the list
        private List<string> ParseDirectory( string dir ) {
            try {
                if( !string.IsNullOrWhiteSpace( dir ) ) {
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
                MetroMessageBox.ShowError( e );
            }

            return _fileList;
        }
    }
}
