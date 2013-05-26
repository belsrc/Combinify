// -------------------------------------------------------------------------------
//    WatchedItem.xaml.cs
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
    using Cgum.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for WatchedItem.xaml.
    /// </summary>
    public partial class WatchedItem : UserControl {
        
        /// <summary>
        /// Initializes a new instance of the WatchedItem class.
        /// </summary>
        public WatchedItem() {
            InitializeComponent();
            this.DataContext = this;
        }

        /* Event
           ---------------------------------------------------------------------------------------*/

        /// <summary>
        /// Occurs when the Clear list button is pressed.
        /// </summary>
        public event RoutedEventHandler RemoveClick {
            add { this.AddHandler( RemoveClickEvent, value ); }
            remove { this.RemoveHandler( RemoveClickEvent, value ); }
        }

        /// <summary>
        /// Routed event register for the RemoveClick event.
        /// </summary>
        public static readonly RoutedEvent RemoveClickEvent =
            EventManager.RegisterRoutedEvent( "RemoveClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( WatchedItem )
                                            );

        /* Properties
           ---------------------------------------------------------------------------------------*/

        /// <summary>
        /// Gets or sets the watched file path.
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Gets or sets the the file path.
        /// </summary>
        public string ShownPath {
            get { return ( string )GetValue( ShownPathProperty ); }
            set { SetValue( ShownPathProperty, value ); }
        }

        /// <summary>
        /// Dependency Property for HeaderText.
        /// </summary>
        public static readonly DependencyProperty ShownPathProperty =
            DependencyProperty.Register( "ShownPath",
                                         typeof( string ),
                                         typeof( WatchedItem )
                                       );

        /* Event Handlers
           ---------------------------------------------------------------------------------------*/

        private void GridRoot_MouseEnter( object sender, MouseEventArgs e ) {
            AnimateProperty.AnimateMargin( removeControl,
                                       new Thickness( 0, 0, 0, 0 ),
                                       new Duration( TimeSpan.FromSeconds( .2 ) )
                                     );
        }

        private void GridRoot_MouseLeave( object sender, MouseEventArgs e ) {
            AnimateProperty.AnimateMargin( removeControl,
                                       new Thickness( 0, 0, -73, 0 ),
                                       new Duration( TimeSpan.FromSeconds( .2 ) )
                                     );
        }

        private void Button_MouseEnter( object sender, MouseEventArgs e ) {
            lblButton.Foreground = AnimateProperty.EaseSolidBrush(
                                        lblButton.Foreground as SolidColorBrush,
                                        Color.FromArgb( 255, 30, 187, 238 ),
                                        new Duration( TimeSpan.FromSeconds( .2 ) )
                                   );
        }

        private void Button_MouseLeave( object sender, MouseEventArgs e ) {
            lblButton.Foreground = AnimateProperty.EaseSolidBrush(
                                        lblButton.Foreground as SolidColorBrush,
                                        Color.FromArgb( 255, 255, 255, 255 ),
                                        new Duration( TimeSpan.FromSeconds( .2 ) )
                                   );
        }

        private void btnRemove_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            this.RaiseEvent( new RoutedEventArgs( RemoveClickEvent, this ) );
        }

        private void WatchedItemMain_MouseMove( object sender, MouseEventArgs e ) {
            if( ( sender as Control ) != null && e.LeftButton == MouseButtonState.Pressed ) {
                DataObject dob = new DataObject( this.GetType(), this );
                DragDrop.DoDragDrop( WatchedItemMain, dob, DragDropEffects.Move );
            }
        }
    }
}
