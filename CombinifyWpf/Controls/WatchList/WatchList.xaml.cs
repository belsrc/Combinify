// -------------------------------------------------------------------------------
//    WatchList.xaml.cs
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
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using Cgum.Extensions;
    using ModernUIDialogs;
    using Cgum.Dialog;
    using Microsoft.Win32;
    using Cgum.Controls;
    using System.Collections.Generic;
    using CombinifyWpf.Models;
    //using CombinifyWpf.Adorners;
    using System.Windows.Documents;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for WatchList.xaml.
    /// </summary>
    public partial class WatchList : UserControl {

        private bool _isDown;
        private bool _isDragging;
        private UIElement _originalElement;
        //private SimpleAdorner _overlayElement;
        private Point _startPoint;

        
        /// <summary>
        /// Initializes a new instance of the WatchList class.
        /// </summary>
        public WatchList() {
            InitializeComponent();
        }

        /* Event
           ---------------------------------------------------------------------------------------*/

        /// <summary>
        /// Occurs when the Clear list button is pressed.
        /// </summary>
        public event RoutedEventHandler ClearClick {
            add { this.AddHandler( ClearClickEvent, value ); }
            remove { this.RemoveHandler( ClearClickEvent, value ); }
        }

        /// <summary>
        /// Routed event register for the ClearClick event.
        /// </summary>
        public static readonly RoutedEvent ClearClickEvent =
            EventManager.RegisterRoutedEvent( "ClearClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( WatchList )
                                            );

        /// <summary>
        /// Occurs when the Select list button is pressed.
        /// </summary>
        public event RoutedEventHandler SelectClick {
            add { this.AddHandler( SelectClickEvent, value ); }
            remove { this.RemoveHandler( SelectClickEvent, value ); }
        }

        /// <summary>
        /// Routed event register for the SelectClick event.
        /// </summary>
        public static readonly RoutedEvent SelectClickEvent =
            EventManager.RegisterRoutedEvent( "SelectClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( WatchList )
                                            );

        /* Properties
           ---------------------------------------------------------------------------------------*/

        /// <summary>
        /// Gets or sets the user controls ShownPath property.
        /// </summary>
        public string ShownPath {
            get { return ( string )GetValue( ShownPathProperty ); }
            set { SetValue( ShownPathProperty, value ); }
        }

        /// <summary>
        /// Dependency Property for ShownPath.
        /// </summary>
        public static readonly DependencyProperty ShownPathProperty =
            DependencyProperty.Register( "ShownPath",
                                         typeof( string ),
                                         typeof( WatchList ),
                                         new PropertyMetadata(
                                             string.Empty,
                                             new PropertyChangedCallback( PathChanged )
                                         )
                                       );

        /// <summary>
        /// Gets or sets the list of file paths to watch.
        /// </summary>
        public IEnumerable<string> ItemsSource {
            get { return ( IEnumerable<string> )GetValue( ItemsSourceProperty ); }
            set { SetValue( ItemsSourceProperty, value ); }
        }

        /// <summary>
        /// Dependency Property for ItemsSource.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register( "ItemsSource",
                                     typeof( IEnumerable<string> ),
                                     typeof( WatchList ),
                                     new PropertyMetadata(
                                         new PropertyChangedCallback( ItemsSourceChange ) )
                                   );

        /* Event Handlers
           ---------------------------------------------------------------------------------------*/

        private static void ItemsSourceChange( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            WatchList wl = ( WatchList )d;
            wl.FileStack.Items.Clear();

            // And re-add them
            foreach( string s in wl.ItemsSource ) {
                if( File.Exists( s ) ) {
                    // Get the files name less the extension
                    string name = new FileInfo( s ).Name;
                    name = wl.StripSpecial( name.Substring( 0, name.IndexOf( "." ) ) );
                    wl.FileStack.Items.Add( new WatchedItem() );
                    int index = wl.FileStack.Items.Count - 1;

                    // Set the items Name, Content and RemoveItem event
                    WatchedItem thisItem = wl.FileStack.Items[ index ] as WatchedItem;
                    thisItem.Name = name;
                    thisItem.FullPath = s;
                }
            }

            wl.SetListItemText();
        }

        private static void PathChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            WatchList wl = ( WatchList )d;
            if( wl.ShownPath == string.Empty || wl.ShownPath == null ) {
                wl.lblDestPath.Opacity = 0;
            }
            else {
                wl.lblDestPath.Opacity = 1;
                wl.SetShownText();
            }
        }

        private void listHead_ButtonClick( object sender, RoutedEventArgs e ) {
            this.RaiseEvent( new RoutedEventArgs( ClearClickEvent, this ) );
        }

        private void destHead_ButtonClick( object sender, RoutedEventArgs e ) {
            this.RaiseEvent( new RoutedEventArgs( SelectClickEvent, this ) );
        }

        private void FileStack_Drop( object sender, DragEventArgs e ) {

        }

        private void FileStack_DragEnter( object sender, DragEventArgs e ) {

        }

        private void FileStack_DragLeave( object sender, DragEventArgs e ) {

        }

        private void FileStack_DragOver( object sender, DragEventArgs e ) {
            
        }

        private void me_SizeChanged( object sender, SizeChangedEventArgs e ) {
            SetShownText();
            SetListItemText();
        }

        /* Methods
           ---------------------------------------------------------------------------------------*/

        // Since Windows allows - & spaces as file names, but aren't
        // allowed in valid control names, we need to remove them
        private string StripSpecial( string original ) {
            return original.Replace( "-", string.Empty ).Replace( " ", string.Empty );
        }

        private int GetStringSize() {
            this.Measure( new Size( Double.PositiveInfinity, Double.PositiveInfinity ) );
            double width = Math.Round( this.DesiredSize.Width );
            double max = 400;
            if( width > max ) {
                return 53;
            }
            else {
                return 35;
            }
        }

        private void SetShownText() {
            int trunc = GetStringSize();
            lblDestPath.Content = ShownPath.TruncateStart( trunc );
        }

        private void SetListItemText() {
            int trunc = GetStringSize();
            foreach( FrameworkElement el in FileStack.Items ) {
                ( el as WatchedItem ).ShownPath = ( el as WatchedItem ).FullPath.TruncateStart( trunc );
            }
        }

        //private void FileStack_PreviewMouseLeftButtonDown( object sender, System.Windows.Input.MouseButtonEventArgs e ) {
        //    if( e.Source.GetType() == typeof( WatchedItem ) ) {
        //        _isDown = true;
        //        _originalElement = e.Source as UIElement;
        //        _startPoint = e.GetPosition( RootGrid );
        //    }
        //}

        //private void FileStack_PreviewMouseLeftButtonUp( object sender, MouseButtonEventArgs e ) {
        //    if( _isDown ) {
        //        DragFinished( false );
        //        //e.Handled = true;
        //    }
        //}

        //private void FileStack_PreviewMouseMove( object sender, System.Windows.Input.MouseEventArgs e ) {
        //    if( _isDown ) {
        //        if( !this._isDragging ) {
        //            DragStarted();
        //        }

        //        if( _isDragging ) {
        //            DragMoved();
        //        }
        //    }
        //}

        //private void DragStarted() {
        //    _isDragging = true;
        //    AdornerLayer layer = AdornerLayer.GetAdornerLayer( _originalElement );
        //    //_overlayElement = new InsertionAdorner( true, true, _originalElement, layer );
        //    _overlayElement = new SimpleAdorner( _originalElement );
        //    layer.Add( _overlayElement );
        //}

        //private void DragMoved() {
        //    Point CurrentPosition = Mouse.GetPosition( RootGrid );

        //    _overlayElement.LeftOffset = CurrentPosition.X - _startPoint.X;
        //    _overlayElement.TopOffset = CurrentPosition.Y - _startPoint.Y;
        //}

        //private void DragFinished( bool cancelled ) {
        //    Mouse.Capture( null );
        //    if( _isDragging ) {
        //        AdornerLayer.GetAdornerLayer( _overlayElement.AdornedElement ).Remove( _overlayElement );
        //        _overlayElement = null;
        //    }

        //    _isDragging = false;
        //    _isDown = false;
        //}
    }
}
