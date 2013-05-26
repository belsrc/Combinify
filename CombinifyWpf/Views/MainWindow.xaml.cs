// -------------------------------------------------------------------------------
//    MainWindow.xaml.cs
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
namespace CombinifyWpf {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;
    using Cgum;
    using Cgum.Controls;
    using Cgum.Dialog;
    using Microsoft.Win32;
    using ModernUIDialogs;
    using System.IO;
    using CombinifyWpf.Models;
    using CombinifyWpf.ViewModels;
    using CombinifyWpf.Utils;
    using System.Threading;
    using System.Windows.Threading;
    using System.Security.Cryptography;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private System.Windows.Forms.NotifyIcon trayIcon;
        private Timer _watchTimer;

        public MainWindow() {
            InitializeComponent();
            //this.Hide();
            ConfigureTrayIcon();
            if( string.IsNullOrWhiteSpace( Properties.Settings.Default.Guid ) ) {
                RandomNumberGenerator rng = new RNGCryptoServiceProvider();
                byte[] tokenData = new byte[ 32 ];
                rng.GetBytes( tokenData );
                Properties.Settings.Default.Guid = Convert.ToBase64String( tokenData );
                Properties.Settings.Default.Save();
            }

            string[] args = Environment.GetCommandLineArgs();
            
            // Since the first arg is always the assemply name we need to make sure there is more than 
            // that, otherwise we can simply ignore it
            if( args.Length > 1 ) {
                ParseCommandArgs( args );
            }


            // For testing
            //( this.DataContext as ProjectViewModel ).WatchedFiles = new List<string>() {
            //    @"E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombinifyWpf\bin\Debug\TestCss\style.css",
            //    @"E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombinifyWpf\bin\Debug\TestCss\core.css",
            //    @"E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombinifyWpf\bin\Debug\TestCss\reset.css"
            //};
            //( this.DataContext as ProjectViewModel ).Destination = @"E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombinifyWpf\bin\Debug\TestCss\destination.css";
            ( this.DataContext as ProjectViewModel ).WatchedFiles = new List<string>() {
                @"E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombinifyWpf\bin\Debug\TestCss\flattener.css"
            };
            ( this.DataContext as ProjectViewModel ).Destination = @"E:\Programming\!Csharp\Combinify Project\CombinifyWpf\CombinifyWpf\bin\Debug\TestCss\flat-result.css";
        }

        /* Event Handlers
           ---------------------------------------------------------------------------------------*/

        private void WindowMain_Closing( object sender, CancelEventArgs e ) {
            trayIcon.Dispose();
            trayIcon = null;
        }

        private void WindowMain_StateChanged( object sender, EventArgs e ) {
            if( this.WindowState == WindowState.Minimized ) {
                this.ShowInTaskbar = false;
                trayIcon.Visible = true;
                trayIcon.ShowBalloonTip( 100 );
            }
            else if( this.WindowState == WindowState.Normal ) {
                this.ShowInTaskbar = true;
                trayIcon.Visible = false;
            }
        }

        private void winStateBtn_MinimizeClick( object sender, RoutedEventArgs e ) {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void winStateBtn_CloseClick( object sender, RoutedEventArgs e ) {
            ExitApplication();
        }

        private void trayIcon_Click( object sender, System.Windows.Forms.MouseEventArgs e ) {
            this.Show();
            this.WindowState = WindowState.Normal;
        }

        private void ContextMenuStart_Click( object sender, EventArgs e ) {
            StartWatching();
        }

        private void ContextMenuStop_Click( object sender, EventArgs e ) {
            StopWatching();
        }

        private void ContextMenuExit_Click( object sender, EventArgs e ) {
            ExitApplication();
        }

        private void WindowMain_MouseMove( object sender, MouseEventArgs e ) {
            Window win = sender as Window;
            if( win != null && e.LeftButton == MouseButtonState.Pressed ) {
                this.DragMove();
            }
        }

        private void WindowMain_Drop( object sender, DragEventArgs e ) {
            // Extract the data from the DataObject-Container into a string list
            string[] files = ( string[] )e.Data.GetData( DataFormats.FileDrop, false );

            // If the dragged object is a css file add it to the list
            if( files != null && files.Length != 0 ) {
                foreach( string f in files ) {
                    if( f.ToLower().EndsWith( ".css" ) ) {
                        ( this.DataContext as ProjectViewModel ).AddFile( f );
                    }
                }
            }
        }

        private void WindowMain_DragOver( object sender, DragEventArgs e ) {
            // Check if the Dataformat of the data can be accepted
            // (we only accept file drops from Explorer, etc.)
            if( ( e.AllowedEffects & DragDropEffects.Copy ) == DragDropEffects.Copy ) {
                if( e.Data.GetDataPresent( DataFormats.FileDrop ) ) {
                    e.Effects = DragDropEffects.Copy;
                }
                else {
                    e.Effects = DragDropEffects.None;
                }
            }
        }

        private void stripMenu_StartClick( object sender, RoutedEventArgs e ) {
            StartWatching();
        }

        private void stripMenu_StopClick( object sender, RoutedEventArgs e ) {
            StopWatching();
        }

        private void stripMenu_ClearClick( object sender, RoutedEventArgs e ) {
            if( MetroMessageBox.Show( "Are you sure you want to clear the list of all watched files?",
                                 "Clear List?",
                                 MessageBoxButton.YesNo,
                                 MessageBoxImage.Question ) == MessageBoxResult.Yes ) {
                ( this.DataContext as ProjectViewModel ).ClearWatched();
            }
        }

        private void stripMenu_CloseProjectClick( object sender, RoutedEventArgs e ) {
            ResetProject();
        }

        private void stripMenu_FileAddClick( object sender, RoutedEventArgs e ) {
            string path = string.Empty;

            if( OpenSaveDialogs.GetOpenPath( out path, new OpenFileDialog(), "Cascading Style Sheet (*.css)|*.css" ) ) {
                ( this.DataContext as ProjectViewModel ).AddFile( path );
            }
        }

        private void stripMenu_DirectoryAddClick( object sender, RoutedEventArgs e ) {
            string path = string.Empty;

            if( OpenSaveDialogs.GetFolderPath( out path,
                new System.Windows.Forms.FolderBrowserDialog(),
                "Select Input Folder",
                true,
                ( this.DataContext as ProjectViewModel ).LastDirectory ) ) {
                ( this.DataContext as ProjectViewModel ).AddDirectory( path );
            }
        }

        private void stripMenu_SaveAsClick( object sender, RoutedEventArgs e ) {
            NewSave();
        }

        private void stripMenu_SaveProjectClick( object sender, RoutedEventArgs e ) {
            ExistingSave();
        }

        private void stripMenu_OpenProjectClick( object sender, RoutedEventArgs e ) {
            string path = string.Empty;

            if( OpenSaveDialogs.GetOpenPath( out path, new OpenFileDialog(), "Combinify Project (*.cpj)|*.cpj" ) ) {
                var pio = new ProjectIO();
                ( this.DataContext as ProjectViewModel ).Model = pio.LoadProject( path );
            }
        }

        private void stripMenu_NewFromDirectoryClick( object sender, RoutedEventArgs e ) {
            string path = string.Empty;
            var context = this.DataContext as ProjectViewModel;

            if( OpenSaveDialogs.GetFolderPath( out path,
                new System.Windows.Forms.FolderBrowserDialog(),
                "Select Input Folder",
                false,
                context.LastDirectory ) ) {
                ResetProject();
                context.AddDirectory( path );
            }
        }

        private void stripMenu_NewFromFileClick( object sender, RoutedEventArgs e ) {
            string path = string.Empty;

            if( OpenSaveDialogs.GetOpenPath( out path, new OpenFileDialog(), "Cascading Style Sheet (*.css)|*.css" ) ) {
                ResetProject();
                ( this.DataContext as ProjectViewModel ).AddFile( path );
            }
        }

        private void stripMenu_MenuClose( object sender, RoutedEventArgs e ) {
            AnimateProperty.AnimateWidth( watchList, 430, new Duration( TimeSpan.FromSeconds( .2 ) ) );
        }

        private void stripMenu_MenuOpen( object sender, RoutedEventArgs e ) {
            AnimateProperty.AnimateWidth( watchList, 300, new Duration( TimeSpan.FromSeconds( .2 ) ) );
        }

        private void stripMenu_ExitClick( object sender, RoutedEventArgs e ) {
            ExitApplication();
        }

        private void watchList_ClearClick( object sender, RoutedEventArgs e ) {
            if( MetroMessageBox.Show( "Are you sure you want to clear the list of all watched files?",
                                 "Clear List?",
                                 MessageBoxButton.YesNo,
                                 MessageBoxImage.Question ) == MessageBoxResult.Yes ) {
                ( this.DataContext as ProjectViewModel ).ClearWatched();
            }
        }

        private void watchList_SelectClick( object sender, RoutedEventArgs e ) {
            string path = string.Empty;

            if( OpenSaveDialogs.GetSavePath( out path, new SaveFileDialog(), "Cascading Style Sheet (*.css)|*.css", "Save Output To" ) ) {
                ( this.DataContext as ProjectViewModel ).Destination = path;
            }
        }

        private void WatchedItemRemove_Click( object sender, RoutedEventArgs e ) {
            var item = e.OriginalSource as WatchedItem;
            string name = new FileInfo( item.FullPath ).Name;

            if( MetroMessageBox.Show( "Are you sure you want to remove " + name + " from the watch list?",
                                 "Remove Item?",
                                 MessageBoxButton.YesNo,
                                 MessageBoxImage.Question ) == MessageBoxResult.Yes ) {
                string val = item.FullPath;
                ( this.DataContext as ProjectViewModel ).RemoveItem( val );
            }
        }

        private void WatchTimer_Elapsed( object state ) {
            Dispatcher.BeginInvoke( ( Action )delegate {
                var context = this.DataContext as ProjectViewModel;
                if( context.IsWatching && context.FilesNeedProcessed() ) {
                    context.ProcessFiles();
                }
            }, DispatcherPriority.Normal );
        }

        /* Methods
           ---------------------------------------------------------------------------------------*/

        private void ConfigureTrayIcon() {
            var contextMenu = new System.Windows.Forms.ContextMenuStrip();
            
            System.Windows.Forms.ToolStripMenuItem miOne = new System.Windows.Forms.ToolStripMenuItem();
            miOne.Text = "Start Montioring";
            miOne.Enabled = true;
            miOne.Click += new EventHandler( ContextMenuStart_Click );
            contextMenu.Items.Add( miOne );

            System.Windows.Forms.ToolStripMenuItem miTwo = new System.Windows.Forms.ToolStripMenuItem();
            miTwo.Text = "Stop Montioring";
            miTwo.Enabled = false;
            miTwo.Click += new EventHandler( ContextMenuStop_Click );
            contextMenu.Items.Add( miTwo );

            contextMenu.Items.Add( "-" );
            System.Windows.Forms.ToolStripMenuItem miThree = new System.Windows.Forms.ToolStripMenuItem();
            miThree.Text = "Exit Combinify";
            miThree.Click += new EventHandler( ContextMenuExit_Click );
            contextMenu.Items.Add( miThree );

            trayIcon = new System.Windows.Forms.NotifyIcon();
            trayIcon.Icon = new System.Drawing.Icon( "icon.ico" );
            trayIcon.BalloonTipTitle = "Combinify";
            trayIcon.BalloonTipText = "Lives in your tray, eats your goats";
            trayIcon.ContextMenuStrip = contextMenu;
            trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( trayIcon_Click );
        }

        private void StartWatching() {
            SwitchEnabledStates();
            this._watchTimer = new Timer( WatchTimer_Elapsed, null, TimeSpan.FromSeconds( 0 ), TimeSpan.FromSeconds( 1 ) );
        }

        private void StopWatching() {
            SwitchEnabledStates();
            if( this._watchTimer != null ) {
                this._watchTimer.Dispose();
            }
        }

        private void SwitchEnabledStates() {
            var context = this.DataContext as ProjectViewModel;
            var strip = trayIcon.ContextMenuStrip;
            context.IsWatching = !context.IsWatching;
            stripMenu.lblStart.IsEnabled = !stripMenu.lblStart.IsEnabled;
            stripMenu.lblStop.IsEnabled = !stripMenu.lblStop.IsEnabled;
            strip.Items[ 0 ].Enabled = !strip.Items[ 0 ].Enabled; // Start label
            strip.Items[ 1 ].Enabled = !strip.Items[ 1 ].Enabled; // Stop label
        }

        private void ExitApplication() {
            // TODO : Check for unsaved changes in current project
            Application.Current.MainWindow.Close();
        }

        private void NewSave() {
            string path = string.Empty;

            if( OpenSaveDialogs.GetSavePath( out path, new SaveFileDialog(), "Combinify Project (*.cpj)|*.cpj", "Save Project" ) ) {
                var pio = new ProjectIO();
                var context = this.DataContext as ProjectViewModel;
                context.ProjectSavePath = path;
                context.LastDirectory = path.Substring( 0, path.LastIndexOf( "\\" ) + 1 );
                pio.SaveProject( path, context.Model );
            }
        }

        private void ExistingSave() {
            var context = this.DataContext as ProjectViewModel;
            if( string.IsNullOrWhiteSpace( context.ProjectSavePath ) ) {
                NewSave();
            }
            else {
                var pio = new ProjectIO();
                pio.SaveProject( context.ProjectSavePath, context.Model );
            }
        }

        private void ResetProject() {
            // TODO : Check for unsaved changes in current project
            ( this.DataContext as ProjectViewModel ).Close();
        }

        private void IconSvg_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            var folder = new FolderSelectDialog();
            folder.ShowDialog();
        }

        private void ParseCommandArgs( string[] args ) {
            // Means its a simple file/project so just open the GUI
            if( args.Length == 2 && File.Exists( args[ 1 ] ) ) {
                var pio = new ProjectIO();
                ( this.DataContext as ProjectViewModel ).Model = pio.LoadProject( args[ 1 ] );
                return;
            }
        }
    }
}
