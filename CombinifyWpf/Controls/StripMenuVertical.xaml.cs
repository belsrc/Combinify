// -------------------------------------------------------------------------------
//    StripMenuVertical.xaml.cs
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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using Cgum.Controls;

    /// <summary>
    /// Interaction logic for StripMenuVertical.xaml
    /// </summary>
    public partial class StripMenuVertical : UserControl {

        /// <summary>
        /// Initializes a new instance of the StripMenuVertical class.
        /// </summary>
        public StripMenuVertical() {
            InitializeComponent();
        }

        /* Events
           ---------------------------------------------------------------------------------------*/

        public event RoutedEventHandler MenuOpen {
            add { this.AddHandler( MenuOpenEvent, value ); }
            remove { this.RemoveHandler( MenuOpenEvent, value ); }
        }

        public static readonly RoutedEvent MenuOpenEvent =
            EventManager.RegisterRoutedEvent( "MenuOpen",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( StripMenuVertical )
                                            );

        public event RoutedEventHandler MenuClose {
            add { this.AddHandler( MenuCloseEvent, value ); }
            remove { this.RemoveHandler( MenuCloseEvent, value ); }
        }

        public static readonly RoutedEvent MenuCloseEvent =
            EventManager.RegisterRoutedEvent( "MenuClose",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( StripMenuVertical )
                                            );

        public event RoutedEventHandler NewFromFileClick {
            add { this.AddHandler( NewFromFileClickEvent, value ); }
            remove { this.RemoveHandler( NewFromFileClickEvent, value ); }
        }

        public static readonly RoutedEvent NewFromFileClickEvent =
            EventManager.RegisterRoutedEvent( "NewFromFileClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( StripMenuVertical )
                                            );

        public event RoutedEventHandler NewFromDirectoryClick {
            add { this.AddHandler( NewFromDirectoryClickEvent, value ); }
            remove { this.RemoveHandler( NewFromDirectoryClickEvent, value ); }
        }

        public static readonly RoutedEvent NewFromDirectoryClickEvent =
            EventManager.RegisterRoutedEvent( "NewFromDirectoryClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( StripMenuVertical )
                                            );

        public event RoutedEventHandler OpenProjectClick {
            add { this.AddHandler( OpenProjectClickEvent, value ); }
            remove { this.RemoveHandler( OpenProjectClickEvent, value ); }
        }

        public static readonly RoutedEvent OpenProjectClickEvent =
            EventManager.RegisterRoutedEvent( "OpenProjectClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( StripMenuVertical )
                                            );

        public event RoutedEventHandler SaveProjectClick {
            add { this.AddHandler( SaveProjectClickEvent, value ); }
            remove { this.RemoveHandler( SaveProjectClickEvent, value ); }
        }

        public static readonly RoutedEvent SaveProjectClickEvent =
            EventManager.RegisterRoutedEvent( "SaveProjectClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( StripMenuVertical )
                                            );

        public event RoutedEventHandler SaveAsClick {
            add { this.AddHandler( SaveAsClickEvent, value ); }
            remove { this.RemoveHandler( SaveAsClickEvent, value ); }
        }

        public static readonly RoutedEvent SaveAsClickEvent =
            EventManager.RegisterRoutedEvent( "SaveAsClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( StripMenuVertical )
                                            );

        public event RoutedEventHandler CloseProjectClick {
            add { this.AddHandler( CloseProjectClickEvent, value ); }
            remove { this.RemoveHandler( CloseProjectClickEvent, value ); }
        }

        public static readonly RoutedEvent CloseProjectClickEvent =
            EventManager.RegisterRoutedEvent( "CloseProjectClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( StripMenuVertical )
                                            );

        public event RoutedEventHandler DirectoryAddClick {
            add { this.AddHandler( DirectoryAddClickEvent, value ); }
            remove { this.RemoveHandler( DirectoryAddClickEvent, value ); }
        }

        public static readonly RoutedEvent DirectoryAddClickEvent =
            EventManager.RegisterRoutedEvent( "DirectoryAddClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( StripMenuVertical )
                                            );

        public event RoutedEventHandler FileAddClick {
            add { this.AddHandler( FileAddClickEvent, value ); }
            remove { this.RemoveHandler( FileAddClickEvent, value ); }
        }

        public static readonly RoutedEvent FileAddClickEvent =
            EventManager.RegisterRoutedEvent( "FileAddClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( StripMenuVertical )
                                            );

        public event RoutedEventHandler ClearClick {
            add { this.AddHandler( ClearClickEvent, value ); }
            remove { this.RemoveHandler( ClearClickEvent, value ); }
        }

        public static readonly RoutedEvent ClearClickEvent =
            EventManager.RegisterRoutedEvent( "ClearClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( StripMenuVertical )
                                            );

        public event RoutedEventHandler ExitClick {
            add { this.AddHandler( ExitClickEvent, value ); }
            remove { this.RemoveHandler( ExitClickEvent, value ); }
        }

        public static readonly RoutedEvent ExitClickEvent =
            EventManager.RegisterRoutedEvent( "ExitClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( StripMenuVertical )
                                            );

        public event RoutedEventHandler StartClick {
            add { this.AddHandler( StartClickEvent, value ); }
            remove { this.RemoveHandler( StartClickEvent, value ); }
        }

        public static readonly RoutedEvent StartClickEvent =
            EventManager.RegisterRoutedEvent( "StartClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( StripMenuVertical )
                                            );

        public event RoutedEventHandler StopClick {
            add { this.AddHandler( StopClickEvent, value ); }
            remove { this.RemoveHandler( StopClickEvent, value ); }
        }

        public static readonly RoutedEvent StopClickEvent =
            EventManager.RegisterRoutedEvent( "StopClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( StripMenuVertical )
                                            );

        /* Event Handlers
           ---------------------------------------------------------------------------------------*/

        private void MainMenu_MouseEnter( object sender, MouseEventArgs e ) {
            ( sender as Control ).Foreground = AnimateProperty.EaseSolidBrush(
                                                   ( sender as Control ).Foreground as SolidColorBrush,
                                                   Color.FromArgb( 255, 255, 255, 255 ),
                                                   new Duration( TimeSpan.FromSeconds( .2 ) )
                                               );

            ( sender as Control ).Background = AnimateProperty.EaseSolidBrush(
                                                   ( sender as Control ).Background as SolidColorBrush,
                                                   Color.FromArgb( 255, 51, 51, 51 ),
                                                   new Duration( TimeSpan.FromSeconds( .2 ) )
                                               );
        }

        private void MainMenu_MouseLeave( object sender, MouseEventArgs e ) {
            FadeMainOut( ( sender as Control ) );
        }

        private void MainMenu_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            // Close any other open menus
            CloseMenus();

            if( ( sender as Control ).Name.Equals( "lblOption" ) ) {
                this.RaiseEvent( new RoutedEventArgs( MenuCloseEvent, this ) );
                OptionsWindow win = new OptionsWindow();
                win.Owner = Application.Current.MainWindow;
                win.ShowDialog();
                return;
            }

            // Remove the mouse events
            ( sender as Control ).MouseLeave -= MainMenu_MouseLeave;
            ( sender as Control ).MouseEnter -= MainMenu_MouseEnter;
            ( sender as Control ).MouseLeftButtonDown -= MainMenu_MouseLeftButtonDown;

            // Get the panel by the control that fired the event
            StackPanel panel = GetPanelByElement( ( sender as Control ) );

            // Start the panel animation
            AnimateProperty.AnimateMargin( panel,
                                       new Thickness( 95, 0, 0, 0 ),
                                       new Duration( TimeSpan.FromSeconds( .2 ) )
                                     );

            this.RaiseEvent( new RoutedEventArgs( MenuOpenEvent, this ) );
        }

        private void SubMenu_MouseEnter( object sender, MouseEventArgs e ) {
            if( ( ( sender as Control ).Foreground as SolidColorBrush ).Color != Color.FromArgb( 255, 30, 187, 238 ) ) {
                ( sender as Control ).Background = AnimateProperty.EaseSolidBrush(
                                                       ( sender as Control ).Background as SolidColorBrush,
                                                       Color.FromArgb( 255, 100, 100, 100 ), //125, 255, 255, 255
                                                       new Duration( TimeSpan.FromSeconds( .2 ) )
                                                   );
            }
        }

        private void SubMenu_MouseLeave( object sender, MouseEventArgs e ) {
            if( ( ( sender as Control ).Foreground as SolidColorBrush ).Color != Color.FromArgb( 255, 30, 187, 238 ) ) {
                ( sender as Control ).Background = AnimateProperty.EaseSolidBrush(
                                                       ( sender as Control ).Background as SolidColorBrush,
                                                       Color.FromArgb( 255, 51, 51, 51 ),
                                                       new Duration( TimeSpan.FromSeconds( .2 ) )
                                                   );
            }
        }

        private void BackArrow_MouseEnter( object sender, MouseEventArgs e ) {
            AnimateProperty.EaseOpacity( sender as FrameworkElement, 1, new Duration( TimeSpan.FromSeconds( .2 ) ) );
        }

        private void BackArrow_MouseLeave( object sender, MouseEventArgs e ) {
            AnimateProperty.EaseOpacity( sender as FrameworkElement, .6, new Duration( TimeSpan.FromSeconds( .2 ) ) );
        }

        private void BackArrow_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            // Get the current StackPanel and related menu label
            StackPanel panel = GetPanelByElement( ( sender as FrameworkElement ) );
            Label lbl = GetLabelByElement( ( sender as FrameworkElement ) );

            // Reset the labels hover state
            FadeMainOut( lbl );

            // Add the event listeners back
            lbl.MouseLeave += MainMenu_MouseLeave;
            lbl.MouseEnter += MainMenu_MouseEnter;
            lbl.MouseLeftButtonDown += MainMenu_MouseLeftButtonDown;

            // Start the panel animation
            AnimateProperty.AnimateMargin( panel,
                                       new Thickness( -45, 1, 0, 0 ),
                                       new Duration( TimeSpan.FromSeconds( .2 ) )
                                     );

            this.RaiseEvent( new RoutedEventArgs( MenuCloseEvent, this ) );
        }

        private void lblNew_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            if( lblNewDirectory.Visibility != Visibility.Visible ) {
                lblNewDirectory.Visibility = Visibility.Visible;
                lblNewFile.Visibility = Visibility.Visible;
            }
            else {
                lblNewDirectory.Visibility = Visibility.Collapsed;
                lblNewFile.Visibility = Visibility.Collapsed;
            }
        }

        private void lblNewFile_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            this.RaiseEvent( new RoutedEventArgs( NewFromFileClickEvent, this ) );
        }

        private void lblNewDirectory_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            this.RaiseEvent( new RoutedEventArgs( NewFromDirectoryClickEvent, this ) );
        }

        private void lblOpen_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            this.RaiseEvent( new RoutedEventArgs( OpenProjectClickEvent, this ) );
        }

        private void lblSave_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            this.RaiseEvent( new RoutedEventArgs( SaveProjectClickEvent, this ) );
        }

        private void lblSaveAs_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            this.RaiseEvent( new RoutedEventArgs( SaveAsClickEvent, this ) );
        }

        private void lblClose_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            this.RaiseEvent( new RoutedEventArgs( CloseProjectClickEvent, this ) );
        }

        private void lblExit_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            this.RaiseEvent( new RoutedEventArgs( ExitClickEvent, this ) );
        }

        private void lblAddDir_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            this.RaiseEvent( new RoutedEventArgs( DirectoryAddClickEvent, this ) );
        }

        private void lblAddFile_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            this.RaiseEvent( new RoutedEventArgs( FileAddClickEvent, this ) );
        }

        private void lblClear_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            this.RaiseEvent( new RoutedEventArgs( ClearClickEvent, this ) );
        }

        private void lblStart_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            this.RaiseEvent( new RoutedEventArgs( StartClickEvent, this ) );
        }

        private void lblStop_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            this.RaiseEvent( new RoutedEventArgs( StopClickEvent, this ) );
        }

        private void Label_IsEnabledChanged( object sender, DependencyPropertyChangedEventArgs e ) {
            var lbl = sender as Label;
            if( lbl.IsEnabled ) {
                lbl.Foreground = Brushes.White;
            }
            else {
                lbl.Foreground = Brushes.Gray;
            }
        }

        /* Methods
           ---------------------------------------------------------------------------------------*/

        private void CloseMenus() {
            Thickness extended = new Thickness( 95, 0, 0, 0 );

            // Close any other open menus
            foreach( StackPanel s in Root.Children ) {
                if( s.Margin == extended ) {

                    // Start the panel animation
                    AnimateProperty.AnimateMargin( s,
                                              new Thickness( -45, 1, 0, 0 ),
                                              new Duration( TimeSpan.FromSeconds( .2 ) )
                                             );

                    // Get related label
                    Label lbl = GetLabelByElement( s );

                    // Hook events back up
                    lbl.MouseEnter += MainMenu_MouseEnter;
                    lbl.MouseLeave += MainMenu_MouseLeave;
                    lbl.MouseLeftButtonDown += MainMenu_MouseLeftButtonDown;

                    // Reset hover state
                    FadeMainOut( lbl );
                }
            }
        }

        private void FadeMainOut( Control ctrl ) {
            ctrl.Foreground = AnimateProperty.EaseSolidBrush(
                                    ctrl.Foreground as SolidColorBrush,
                                    Color.FromArgb( 255, 187, 187, 187 ),
                                    new Duration( TimeSpan.FromSeconds( .2 ) )
                              );

            ctrl.Background = AnimateProperty.EaseSolidBrush(
                                    ctrl.Background as SolidColorBrush,
                                    Color.FromArgb( 255, 255, 255, 255 ),
                                    new Duration( TimeSpan.FromSeconds( .2 ) )
                              );
        }

        private Label GetLabelByElement( FrameworkElement element ) {
            switch( element.Name ) {
                case "FilePanel":
                case "fileSvg":
                    return lblFile;

                case "ListPanel":
                case "listSvg":
                    return lblWatch;

                default: // MonitorPanel || monitorSvg
                    return lblMonitor;
            }
        }

        private StackPanel GetPanelByElement( FrameworkElement element ) {
            switch( element.Name ) {
                case "lblFile":
                case "fileSvg":
                    return FilePanel;

                case "lblWatch":
                case "listSvg":
                    return ListPanel;

                default: // lblMonitor || monitorSvg
                    return MonitorPanel;
            }
        }
    }
}