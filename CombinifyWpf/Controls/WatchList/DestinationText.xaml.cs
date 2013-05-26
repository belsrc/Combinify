// -------------------------------------------------------------------------------
//    DestinationText.xaml.cs
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
    using Cgum;
    using Cgum.Controls;
    using Cgum.Dialog;
    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for DestinationText.xaml
    /// </summary>
    /// <remarks>
    /// Public Control Properties:
    ///     ShownPath: string
    /// </remarks>
    public partial class DestinationText : UserControl {

        /// <summary>
        /// Initialize a new instance of the DestinationText class.
        /// </summary>
        public DestinationText() {
            InitializeComponent();
            this.DataContext = this;
            Project.Instance.DestinationChanged += new EventHandler( Destination_Changed );
        }

        /// <summary>
        /// Gets or sets the user controls ShownPath property.
        /// </summary>
        public string ShownPath {
            get { return ( string )GetValue( ShownPathProperty ); }
            set { SetValue( ShownPathProperty, value ); }
        }

        public static readonly DependencyProperty ShownPathProperty =
            DependencyProperty.Register( "ShownPath",
                                         typeof( string ),
                                         typeof( DestinationText )
                                       );

        // Event handler for the Destination Changed event. Sets the path label. Adjusts the visible controls.
        private void Destination_Changed( object sender, EventArgs e ) {
            // Set the label content
            lblDestPath.Content = Project.Instance.DestinationFile.TruncateStart( 53 );

            // If there's a path, show the textbox and the hover select button
            // else, show the destination button
            if( Project.Instance.DestinationFile != string.Empty ) {
                AnimateProperty.EaseOpacityIn( lblDestPath, new Duration( TimeSpan.FromSeconds( .4 ) ) );
            }
            else {
                if( lblDestPath.Opacity == 1 ) {
                    AnimateProperty.EaseOpacityOut( lblDestPath, new Duration( TimeSpan.FromSeconds( .4 ) ) );
                }
            }
        }

        // Event handler for the Mouse Enter event.
        private void Button_MouseEnter( object sender, MouseEventArgs e ) {
            ( sender as Control ).Foreground = AnimateProperty.EaseSolidBrush(
                                                   ( sender as Control ).Foreground as SolidColorBrush,
                                                   Color.FromArgb( 255, 30, 187, 238 ),
                                                   new Duration( TimeSpan.FromSeconds( .2 ) )
                                               );
        }

        // Event handler for the Mouse Leave event.
        private void Button_MouseLeave( object sender, MouseEventArgs e ) {
            ( sender as Control ).Foreground = AnimateProperty.EaseSolidBrush(
                                                   ( sender as Control ).Foreground as SolidColorBrush,
                                                   Color.FromArgb( 255, 255, 255, 255 ),
                                                   new Duration( TimeSpan.FromSeconds( .2 ) )
                                               );
        }

        // Event handler for the Header label Click event. 
        // Gets user path input and sets the Project destination file and the destination label content.
        private void destHead_ButtonClicked( object sender, EventArgs e ) {
            string path = string.Empty;

            if( OpenSaveDialogs.GetSavePath( out path, new SaveFileDialog(), "Cascading Style Sheet (*.css)|*.css", "Save Output To" ) ) {
                Project.Instance.DestinationFile = path;
            }
        }
    }
}