// -------------------------------------------------------------------------------
//    OptionsWindow.xaml.cs
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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Cgum.Encryption;
    using System;

    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window {

        /// <summary>
        /// Initializes a new instance of the OptionsWindow class.
        /// </summary>
        public OptionsWindow() {
            InitializeComponent();
        }

        private void lblImports_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            Flatten.IsChecked = !Flatten.IsChecked;
        }

        private void lblCombine_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            Combine.IsChecked = !Combine.IsChecked;
        }

        private void lblComb_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            Comb.IsChecked = !Comb.IsChecked;
        }

        private void lblMinify_MouseLeftButtonDown( object sender, MouseButtonEventArgs e ) {
            Minify.IsChecked = !Minify.IsChecked;
        }

        private void CloseButton_Click( object sender, RoutedEventArgs e ) {
            this.Close();
        }

        private void lblHeader_MouseMove( object sender, MouseEventArgs e ) {
            Control ctrl = sender as Control;
            if( ctrl != null && e.LeftButton == MouseButtonState.Pressed ) {
                this.DragMove();
            }
        }

        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e ) {
            if( Password.Text.Length > 0 ) {
                var cryptor = new Encrypt( EncryptType.Rijndael, Properties.Settings.Default.Guid.Substring( 0, 24 ) );
                Properties.Settings.Default.Word = cryptor.EncryptString( Password.Text );
                Properties.Settings.Default.Sign = Convert.ToBase64String( cryptor.Vector );
                Properties.Settings.Default.Save();
            }
        }

        private void Window_Loaded( object sender, RoutedEventArgs e ) {
            if( !string.IsNullOrWhiteSpace( Properties.Settings.Default.Word ) ) {
                byte[] vect = Convert.FromBase64String( Properties.Settings.Default.Sign );
                var cryptor = new Decrypt( EncryptType.Rijndael, vect, Properties.Settings.Default.Guid.Substring( 0, 24 ) );
                Password.Text = cryptor.DecryptString( Properties.Settings.Default.Word );
            }
        }
    }
}