// -------------------------------------------------------------------------------
//    FileIcon.xaml.cs
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
namespace CombinifyWpf.Controls {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using CombinifyWpf.Utils;

    /// <summary>
    /// Interaction logic for FileIcon.xaml
    /// </summary>
    public partial class FileIcon : UserControl {

        private string _base = @"pack://application:,,,/Images/";
        private string _suffix = "-64x64.png";
        private Dictionary<string, string> _custIcons;

        /// <summary>
        /// Initializes a new instance of the FileIcon class.
        /// </summary>
        public FileIcon()
            : this( null ) { }

        /// <summary>
        /// Initializes a new instance of the FileIcon class.
        /// </summary>
        /// <param name="icons">A list of custom file type icons.</param>
        public FileIcon( Dictionary<string, string> icons ) {
            InitializeComponent();
            this._custIcons = icons;
        }

        /// <summary>
        /// Gets or sets the path of the file that this represents.
        /// </summary>
        public string FilePath {
            get { return ( string )GetValue( FilePathProperty ); }
            set { SetValue( FilePathProperty, value ); }
        }

        /// <summary>
        /// Dependency property for FileName.
        /// </summary>
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register( "FilePath",
                                         typeof( string ),
                                         typeof( FileIcon ),
                                         new PropertyMetadata(
                                             new PropertyChangedCallback( FilePath_Changed ) )
                                       );

        /// <summary>
        /// Gets or sets the name of the file that this represents.
        /// </summary>
        public string FileName {
            get { return ( string )GetValue( FileNameProperty ); }
            set { SetValue( FileNameProperty, value ); }
        }

        /// <summary>
        /// Dependency property for FileName.
        /// </summary>
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register( "FileName",
                                         typeof( string ),
                                         typeof( FileIcon )
                                       );

        private static void FilePath_Changed( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            var fi = ( FileIcon )d;

            if( fi._custIcons == null ) {
                var ico = new IconPicker();
                fi.fileImage.Source = ico.GetFileIcon( fi.FilePath );
                return;
            }

            string relPath = string.Empty;

            if( Directory.Exists( fi.FilePath ) ) {
                var dir = new DirectoryInfo( fi.FilePath );
                fi.FileName = dir.Name;
                if( dir.Parent == null ) {
                    relPath = fi._base + "drive" + fi._suffix;
                }
                else {
                    relPath = fi._base + "folder" + fi._suffix;
                }
            }
            else if( File.Exists( fi.FilePath ) ) {
                var file = new FileInfo( fi.FilePath );
                string ext = file.Extension;
                fi.FileName = file.Name;
                if( fi._custIcons != null && fi._custIcons.ContainsKey( ext ) ) {
                    relPath = fi._custIcons[ ext ];
                }
            }

            if( !string.IsNullOrWhiteSpace( relPath ) ) {
                Uri uri = new Uri( relPath, UriKind.RelativeOrAbsolute );
                BitmapImage source = new BitmapImage( uri );
                fi.fileImage.Source = source;
            }
            else {
                throw new FileNotFoundException();
            }
        }
    }
}
