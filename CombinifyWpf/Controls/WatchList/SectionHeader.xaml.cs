// -------------------------------------------------------------------------------
//    SectionHeader.xaml.cs
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
    /// Interaction logic for ListHeader.xaml
    /// </summary>
    public partial class SectionHeader : UserControl {
        /// <summary>
        /// Initializes a new instance of the ListHeader class.
        /// </summary>
        public SectionHeader() {
            InitializeComponent();
            this.DataContext = this; ;
        }

        /// <summary>
        /// Occurs when the action button is clicked.
        /// </summary>
        public event RoutedEventHandler ButtonClick {
            add { this.AddHandler( ButtonClickEvent, value ); }
            remove { this.RemoveHandler( ButtonClickEvent, value ); }
        }

        /// <summary>
        /// Routed event register for the RemoveClick event.
        /// </summary>
        public static readonly RoutedEvent ButtonClickEvent =
            EventManager.RegisterRoutedEvent( "ButtonClick",
                                              RoutingStrategy.Bubble,
                                              typeof( RoutedEventHandler ),
                                              typeof( SectionHeader )
                                            );
        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        public string HeaderText {
            get { return ( string )GetValue( HeaderTextProperty ); }
            set { SetValue( HeaderTextProperty, value ); }
        }

        /// <summary>
        /// Dependency Property for HeaderText.
        /// </summary>
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register( "HeaderText",
                                         typeof( string ),
                                         typeof( SectionHeader ),
                                         new PropertyMetadata( "SectionHeader" )
                                       );

        /// <summary>
        /// Gets or sets the button text property.
        /// </summary>
        public string ButtonText {
            get { return ( string )GetValue( ButtonTextProperty ); }
            set { SetValue( ButtonTextProperty, value ); }
        }

        /// <summary>
        /// Dependency Property for ButtonText.
        /// </summary>
        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register( "ButtonText",
                                         typeof( string ),
                                         typeof( SectionHeader ),
                                         new PropertyMetadata( "Button" )
                                       );

        private void UserControl_MouseEnter( object sender, MouseEventArgs e ) {
            AnimateProperty.EaseOpacityIn( lblButton, new Duration( TimeSpan.FromSeconds( .4 ) ) );
        }

        private void UserControl_MouseLeave( object sender, MouseEventArgs e ) {
            AnimateProperty.EaseOpacityOut( lblButton, new Duration( TimeSpan.FromSeconds( .4 ) ) );
        }

        private void lblButton_MouseEnter( object sender, MouseEventArgs e ) {
            lblButton.Foreground = AnimateProperty.EaseSolidBrush(
                                        ( sender as Control ).Foreground as SolidColorBrush,
                                        Color.FromArgb( 255, 30, 187, 238 ),
                                        new Duration( TimeSpan.FromSeconds( .2 ) )
                                   );
        }

        private void lblButton_MouseLeave( object sender, MouseEventArgs e ) {
            lblButton.Foreground = AnimateProperty.EaseSolidBrush(
                                        ( sender as Control ).Foreground as SolidColorBrush,
                                        Color.FromArgb( 255, 119, 119, 119 ),
                                        new Duration( TimeSpan.FromSeconds( .2 ) )
                                   );
        }

        private void lblButton_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            // Since doing things like opening a modal window wrecks mouse enter/leave events 
            // (label stays visible until you re-enter/leave) I'll just go a head and reset it
            lblButton.Foreground = new SolidColorBrush( Color.FromArgb( 255, 119, 119, 119 ) );
            AnimateProperty.EaseOpacityOut( lblButton, new Duration( TimeSpan.FromSeconds( .4 ) ) );

            this.RaiseEvent( new RoutedEventArgs( ButtonClickEvent, this ) );
        }
    }
}
