
namespace CombinifyWpf.Dialogs {
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using Cgum.Controls;
    using Cgum.Extensions;
    using CombinifyWpf.Adorners;
    using CombinifyWpf.Controls;
    using ModernUIDialogs;
    using CombinifyWpf.Utils;

    /// <summary>
    /// Interaction logic for FileSelectDialog.xaml
    /// </summary>
    public partial class FileSelectDialog : Window {

        private DirectoryInfo _di;
        private string[] _extensions;
        private DialogType _type;

        /// <summary>
        /// Initializes a new instance of the FileSelectDialog class.
        /// </summary>
        public FileSelectDialog( DialogType type ) 
            : this( type, string.Empty, null ) { }

        /// <summary>
        /// Initializes a new instance of the FileSelectDialog class.
        /// </summary>
        /// <param name="title">The title of the dialog window.</param>
        public FileSelectDialog( DialogType type, string title )
            : this( type, title, null ) { }

        /// <summary>
        /// Initializes a new instance of the FileSelectDialog class.
        /// </summary>
        /// <param name="title">The title of the dialog window.</param>
        /// <param name="extensions">The file extensions to show.</param>
        public FileSelectDialog( DialogType type, string title, params string[] extensions ) {
            InitializeComponent();
            if( extensions != null ) {
                this._extensions = extensions;
            }

            if( string.IsNullOrWhiteSpace( title ) ) {
                if( type == DialogType.Save ) {
                    this.Title = "Save Item";
                }
                else {
                    this.Title = "Open Item";
                }
            }
            else {
                this.Title = title;
            }

            DefaultDirectory = System.IO.Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
        }

        /// <summary>
        /// Gets or sets the user selected file path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the default directory for this dialog;
        /// </summary>
        public string DefaultDirectory {
            get {
                if( this._di != null ) {
                    return this._di.FullName;
                }
                else {
                    return string.Empty;
                }
            }
            set {
                if( Directory.Exists( value ) ) {
                    this._di = new DirectoryInfo( value );
                    breadCrumb.Content = this._di.FullName;
                    SetFiles();
                }
            }
        }

        private void FileWindow_MouseMove( object sender, MouseEventArgs e ) {
            if( sender != null && e.LeftButton == MouseButtonState.Pressed ) {
                this.DragMove();
            }
        }

        private void Window_Loaded( object sender, RoutedEventArgs e ) {
            //var dirs = FilterLocalDirectory( Directory.GetLogicalDrives() );
            //foreach( string s in dirs ) {
            //    TreeViewItem item = new TreeViewItem();
            //    item.Header = s;
            //    item.Tag = s;
            //    item.FontWeight = FontWeights.Normal;
            //    item.Items.Add( dummyNode );
            //    item.Expanded += new RoutedEventHandler( folder_Expanded );
            //    LocalTree.Items.Add( item );
            //}
            SetFiles();
        }

        private void FileWindow_ContentRendered( object sender, EventArgs e ) {

        }

        private void winStatesButton_CloseClick( object sender, RoutedEventArgs e ) {
            CloseDialog( false );
        }

        private void OkButton_Click( object sender, RoutedEventArgs e ) {
            CloseDialog( true );
        }

        private void CancelButton_Click( object sender, RoutedEventArgs e ) {
            CloseDialog( false );
        }

        private void LocalTree_SelectedItemChanged( object sender, RoutedPropertyChangedEventArgs<object> e ) {
            //TreeView tree = ( TreeView )sender;
            //TreeViewItem temp = ( ( TreeViewItem )tree.SelectedItem );
            //string tmp = GetFilePath( tree, temp, '\\' );
            //if( this._visibleFiles ) {
            //    if( this._extensions.Any( s => tmp.EndsWith( s ) ) ) {
            //        Path = tmp;
            //    }
            //}
            //else {
            //    Path = tmp;
            //}

            //selectedPath.Text = Path;
        }

        private void LocalTree_MouseDoubleClick( object sender, MouseButtonEventArgs e ) {
            //TreeView tree = ( TreeView )sender;
            //TreeViewItem temp = ( ( TreeViewItem )tree.SelectedItem );
            //string tmp = GetFilePath( tree, temp, '\\' );
            //if( this._visibleFiles ) {
            //    if( this._extensions.Any( s => tmp.EndsWith( s ) ) ) {
            //        Path = tmp;
            //        this.DialogResult = true;
            //        this.Close();
            //    }
            //}
        }

        private void Canvas_MouseEnter( object sender, MouseEventArgs e ) {
            var can = sender as Canvas;
            AnimateProperty.EaseOpacityIn( can, new Duration( TimeSpan.FromSeconds( .2 ) ) );
        }

        private void Canvas_MouseLeave( object sender, MouseEventArgs e ) {
            var can = sender as Canvas;
            AnimateProperty.EaseOpacity( can, .4, new Duration( TimeSpan.FromSeconds( .2 ) ) );
        }

        private void backSvg_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            if( this._di != null && this._di.Parent != null ) {
                DefaultDirectory = this._di.Parent.FullName;
            }
        }

        private void refreshSvg_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            SetFiles();
        }

        private void Icon_Click( object sender, MouseButtonEventArgs e ) {
            this.filePanel.Children
                          .OfType<FileIcon>()
                          .Do( f => {
                              var al = AdornerLayer.GetAdornerLayer( f );
                              var adorners = al.GetAdorners( f );
                              if( adorners != null && adorners.Length > 0 ) {
                                  al.Remove( adorners[ 0 ] );
                              }
                          } );

            var fi = e.Source as FileIcon;
            var adornLayer = AdornerLayer.GetAdornerLayer( fi );
            if( adornLayer != null ) {
                var selectAdorn = new SelectedAdorner( fi );
                adornLayer.Add( selectAdorn );
            }

            if( !Directory.Exists( fi.FilePath ) && this._extensions.Contains( fi.FileName.Substring( fi.FileName.LastIndexOf( '.' ) ) ) ) {
                selectedPath.Text = fi.FilePath;
            }
        }

        private void Icon_DoubleClick( object sender, MouseButtonEventArgs e ) {
            var fi = e.Source as FileIcon;
            if( File.Exists( fi.FilePath ) ) {
                var info = new FileInfo( fi.FilePath );
                if( this._extensions == null || this._extensions.Contains( info.Extension ) ) {
                    Path = fi.FilePath;
                    CloseDialog( true );
                }
            }
            else {
                DefaultDirectory = fi.FilePath;
            }
        }

        private void SetFiles() {
            this.filePanel.Children.Clear();

            foreach( var dir in Directory.GetDirectories( DefaultDirectory ) ) {
                var fi = new FileIcon( App.FileIcons );
                fi.FilePath = dir;
                fi.Margin = new Thickness( 10 );
                fi.MouseLeftButtonDown += new MouseButtonEventHandler( Icon_Click );
                fi.MouseDoubleClick += new MouseButtonEventHandler( Icon_DoubleClick );
                this.filePanel.Children.Add( fi );
            }

            foreach( var file in Directory.GetFiles( DefaultDirectory ) ) {
                if( this._extensions == null || this._extensions.Contains( file.Substring( file.LastIndexOf( '.' ) ) ) ) {
                    var fi = new FileIcon( App.FileIcons );
                    fi.FilePath = file;
                    fi.Margin = new Thickness( 10 );
                    fi.MouseLeftButtonDown += new MouseButtonEventHandler( Icon_Click );
                    fi.MouseDoubleClick += new MouseButtonEventHandler( Icon_DoubleClick );
                    this.filePanel.Children.Add( fi );
                }
            }
        }

        private void CloseDialog( bool isSuccess ) {
            if( isSuccess ) {
                if( !string.IsNullOrWhiteSpace( Path ) ) {
                    if( this._type == DialogType.Save && !IsConfirmedSave() ) {
                        return;
                    }

                    this.DialogResult = true;
                    this.Close();
                }
                else {
                    MetroMessageBox.ShowWarning( "You must select an item first" );
                }
            }
            else {
                this.DialogResult = false;
                this.Close();
            }
        }

        private bool IsConfirmedSave() {
            var fi = new FileInfo( Path );
            if( !this._extensions.Contains( fi.Extension ) ) {
                MetroMessageBox.ShowWarning( "Not a valid file type.", "Invalid File", MessageBoxButton.OK );
                return false;
            }

            if( fi.Exists ) {
                var res = MetroMessageBox.ShowQuestion( "This will overwrite the existing file. Is this O.K. ?", "File Overwrite", MessageBoxButton.YesNo );
                return res == MessageBoxResult.Yes;
            }

            return true;
        }
    }
}
