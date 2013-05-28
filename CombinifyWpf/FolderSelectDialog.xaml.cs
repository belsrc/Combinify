// -------------------------------------------------------------------------------
//    FolderSelectDialog.xaml.cs
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
namespace CombinifyWpf {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Cgum.Encryption;
    using CombinifyWpf.Utils;
    using ModernUIDialogs;
    using FtpLib;

    /// <summary>
    /// Interaction logic for FolderSelectDialog.xaml
    /// </summary>
    public partial class FolderSelectDialog : Window {
        private object dummyNode = null;
        private FtpConnection _con;
        private bool _visibleFiles;
        private string[] _extensions;

        /// <summary>
        /// Initializes a new instance of the FolderSelectDialog class.
        /// </summary>
        public FolderSelectDialog()
            : this( "Select Item", null ) { }

        /// <summary>
        /// Initializes a new instance of the FolderSelectDialog class.
        /// </summary>
        /// <param name="title">The displayed title of the dialog.</param>
        public FolderSelectDialog( string title )
            : this( title, null ) { }

        /// <summary>
        /// Initializes a new instance of the FolderSelectDialog class.
        /// </summary>
        /// <param name="title">The displayed title of the dialog.</param>
        /// <param name="fileExts">The files, with the proveided extensions, to show in the dialog.</param>
        public FolderSelectDialog( string title, params string[] fileExts ) {
            InitializeComponent();
            lblHeader.Content = title;
            if( fileExts != null ) {
                this._visibleFiles = true;
                this._extensions = fileExts;
            }
            else {
                this._visibleFiles = false;
            }
        }

        /// <summary>
        /// Gets or sets the user selected file/folder path.
        /// </summary>
        public string Path { get; set; }

        private void FileWindow_MouseMove( object sender, MouseEventArgs e ) {
            if( sender != null && e.LeftButton == MouseButtonState.Pressed ) {
                this.DragMove();
            }
        }

        private void winStatesButton1_CloseClick( object sender, RoutedEventArgs e ) {
            this.DialogResult = false;
            this.Close();
        }

        private void OkButton_Click( object sender, RoutedEventArgs e ) {
            if( !string.IsNullOrWhiteSpace( Path ) ) {
                this.DialogResult = true;
                this.Close();
            }
            else {
                MetroMessageBox.ShowWarning( "You must select an item first" );
            }
        }

        private void CancelButton_Click( object sender, RoutedEventArgs e ) {
            this.DialogResult = false;
            this.Close();
        }

        private void Window_Loaded( object sender, RoutedEventArgs e ) {
            var dirs = FilterLocalDirectory( Directory.GetLogicalDrives() );
            foreach( string s in dirs ) {
                TreeViewItem item = new TreeViewItem();
                item.Header = s;
                item.Tag = s;
                item.FontWeight = FontWeights.Normal;
                item.Items.Add( dummyNode );
                item.Expanded += new RoutedEventHandler( folder_Expanded );
                LocalTree.Items.Add( item );
            }
        }

        private void FileWindow_ContentRendered( object sender, EventArgs e ) {
            if( hasRemoteInfo() ) {
                this._con = GetConnection();
                var dirs = FilterRemoteDirectory( this._con.SimpleDirectoryList( "" ) );
                if( this._visibleFiles ) {
                    var files = FilterFileList( this._con.SimpleFileList( "" ) );
                }

                TreeViewItem item = new TreeViewItem();
                item.Header = "/";
                item.Tag = "/";
                item.FontWeight = FontWeights.Normal;
                item.Expanded += new RoutedEventHandler( remoteFolder_Expanded );
                RemoteTree.Items.Add( item );

                if( dirs.Count() > 1 ) {
                    CreateSubNodes( dirs, new string[] { "" }, item, remoteFolder_Expanded );
                }
                else {
                    item.Items.Add( dummyNode );
                }
            }
        }

        private void folder_Expanded( object sender, RoutedEventArgs e ) {
            TreeViewItem item = ( TreeViewItem )sender;
            if( item.Items.Count == 1 && item.Items[ 0 ] == dummyNode ) {
                item.Items.Clear();
                try {
                    var files = new List<string>();
                    var dirs = FilterLocalDirectory( Directory.GetDirectories( item.Tag.ToString() ) );
                    if( this._visibleFiles ) {
                        files.AddRange( files = FilterFileList( Directory.GetFiles( item.Tag.ToString() ) ) );
                    }

                    CreateSubNodes( dirs, files, item, folder_Expanded );
                }
                catch( Exception ) { }
            }
        }

        private void remoteFolder_Expanded( object sender, RoutedEventArgs e ) {
            TreeViewItem item = ( TreeViewItem )sender;
            if( item.Items.Count == 1 && item.Items[ 0 ] == dummyNode ) {
                item.Items.Clear();
                try {
                    var files = new List<string>();
                    var path = GetFilePath( RemoteTree, item, '/' ).Substring( 1 );
                    var dirs = FilterRemoteDirectory( this._con.SimpleDirectoryList( path + "/" ) );
                    if( this._visibleFiles ) {
                        files.AddRange( FilterFileList( this._con.SimpleFileList( path + "/" ) ) );
                    }

                    CreateSubNodes( dirs, files, item, remoteFolder_Expanded );
                }
                catch( Exception ) { }
            }
        }

        private void LocalTree_SelectedItemChanged( object sender, RoutedPropertyChangedEventArgs<object> e ) {
            TreeView tree = ( TreeView )sender;
            TreeViewItem temp = ( ( TreeViewItem )tree.SelectedItem );
            string tmp = GetFilePath( tree, temp, '\\' );
            if( this._visibleFiles ) {
                if( this._extensions.Any( s => tmp.EndsWith( s ) ) ) {
                    Path = tmp;
                }
            }
            else {
                Path = tmp;
            }

            selectedPath.Text = Path;
        }

        private void LocalTree_MouseDoubleClick( object sender, MouseButtonEventArgs e ) {
            TreeView tree = ( TreeView )sender;
            TreeViewItem temp = ( ( TreeViewItem )tree.SelectedItem );
            string tmp = GetFilePath( tree, temp, '\\' );
            if( this._visibleFiles ) {
                if( this._extensions.Any( s => tmp.EndsWith( s ) ) ) {
                    Path = tmp;
                    this.DialogResult = true;
                    this.Close();
                }
            }
        }

        private void RemoteTree_SelectedItemChanged( object sender, RoutedPropertyChangedEventArgs<object> e ) {
            TreeView tree = ( TreeView )sender;
            TreeViewItem temp = ( ( TreeViewItem )tree.SelectedItem );
            string tmp = this._con.Host + GetFilePath( tree, temp, '/' ).Substring( 1 );
            if( this._visibleFiles ) {
                if( this._extensions.Any( s => tmp.EndsWith( s ) ) ) {
                    Path = tmp;
                }
            }
            else {
                Path = tmp;
            }

            selectedPath.Text = Path;
        }

        private void RemoteTree_MouseDoubleClick( object sender, MouseButtonEventArgs e ) {
            TreeView tree = ( TreeView )sender;
            TreeViewItem temp = ( ( TreeViewItem )tree.SelectedItem );
            string tmp = this._con.Host + GetFilePath( tree, temp, '/' ).Substring( 1 );
            if( this._visibleFiles ) {
                if( this._extensions.Any( s => tmp.EndsWith( s ) ) ) {
                    Path = tmp;
                    this.DialogResult = true;
                    this.Close();
                }
            }
        }

        private void selectedPath_TextChanged( object sender, TextChangedEventArgs e ) {
            if( Path != selectedPath.Text ) {
                Path = selectedPath.Text;
            }
        }
        
        private void CreateSubNodes( IEnumerable<string> directories, IEnumerable<string> files, TreeViewItem item, RoutedEventHandler callback ) {
            foreach( string d in directories ) {
                if( !string.IsNullOrWhiteSpace( d ) ) {
                    TreeViewItem subitem = new TreeViewItem();
                    subitem.Header = d.Substring( d.LastIndexOf( "\\" ) + 1 );
                    subitem.Tag = d;
                    subitem.FontWeight = FontWeights.Normal;
                    subitem.Items.Add( dummyNode );
                    subitem.Expanded += callback;
                    item.Items.Add( subitem );
                }
            }

            foreach( string f in files ) {
                if( !string.IsNullOrWhiteSpace( f ) ) {
                    TreeViewItem subitem = new TreeViewItem();
                    subitem.Header = f.Substring( f.LastIndexOf( "\\" ) + 1 ); ;
                    subitem.Tag = f;
                    subitem.FontWeight = FontWeights.Normal;
                    item.Items.Add( subitem );
                }
            }
        }
        
        private string GetFilePath( TreeView tree, TreeViewItem temp, char separator ) {
            if( temp == null ) {
                return string.Empty;
            }

            string path = "";
            string temp1 = "";
            string temp2 = "";
            while( true ) {
                temp1 = temp.Header.ToString();
                if( temp1.Contains( separator ) ) {
                    temp2 = "";
                }
                path = temp1 + temp2 + path;
                if( temp.Parent.GetType().Equals( typeof( TreeView ) ) ) {
                    break;
                }
                temp = ( ( TreeViewItem )temp.Parent );
                temp2 = separator.ToString();
            }

            return path;
        }

        private List<string> FilterLocalDirectory( IEnumerable<string> directories ) {
            return directories.Where( d => {
                try {
                    return Directory.GetDirectories( d ).Length > 0 || Directory.GetFiles( d ).Length > 0;
                }
                catch( Exception trash ) {
                    return false;
                }
            } ).ToList();
        }

        private List<string> FilterRemoteDirectory( IEnumerable<string> directories ) {
            return directories.Where( d => {
                try {
                    return d != "." && d != "..";
                }
                catch( Exception trash ) { return false; }
            } ).ToList();
        }

        private List<string> FilterFileList( IEnumerable<string> files ) {
            return files.Where( c => {
                try {
                    return this._extensions.Contains( c.Substring( c.LastIndexOf( '.' ) ).ToLower() );
                }
                catch( Exception trash ) { return false; }
            } ).ToList();
        }

        private bool hasRemoteInfo() {
            return !string.IsNullOrWhiteSpace( Properties.Settings.Default.Nick ) &&
                !string.IsNullOrWhiteSpace( Properties.Settings.Default.Path ) &&
                !string.IsNullOrWhiteSpace( Properties.Settings.Default.Port.ToString() ) &&
                !string.IsNullOrWhiteSpace( Properties.Settings.Default.Word );
        }

        private FtpConnection GetConnection() {
            byte[] vect = Convert.FromBase64String( Properties.Settings.Default.Sign );
            var cryptor = new Decrypt( EncryptType.Rijndael, vect, Properties.Settings.Default.Guid.Substring( 0, 24 ) );
            string p = cryptor.DecryptString( Properties.Settings.Default.Word );
            return new FtpConnection( Properties.Settings.Default.Path,
                               Properties.Settings.Default.Port,
                               Properties.Settings.Default.Nick,
                               p,
                               Properties.Settings.Default.Ssl
                             );
        }
    }
}
