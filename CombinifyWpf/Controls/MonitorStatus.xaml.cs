// -------------------------------------------------------------------------------
//    MonitorStatus.xaml.cs
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
    using System.Windows;
    using System.Windows.Controls;
    using Cgum.Controls;

    /// <summary>
    /// Interaction logic for MonitorStatus.xaml.
    /// </summary>
    public partial class MonitorStatus : UserControl {

        /// <summary>
        /// Initializes a new instance of the MonitorStatus class.
        /// </summary>
        public MonitorStatus() {
            InitializeComponent();
        }
        
        /* Properties
           ---------------------------------------------------------------------------------------*/

        /// <summary>
        /// Gets or sets a value indicating whether the app is running or not.
        /// </summary>
        public bool IsRunning {
            get { return ( bool )GetValue( IsRunningProperty ); }
            set { SetValue( IsRunningProperty, value ); }
        }

        

        /// <summary>
        /// Dependency property for IsRunning.
        /// </summary>
        public static readonly DependencyProperty IsRunningProperty =
            DependencyProperty.Register( "IsRunning",
                                         typeof( bool ),
                                         typeof( MonitorStatus ),
                                         new PropertyMetadata(
                                             false,
                                             new PropertyChangedCallback( Running_Changed ) )
                                       );

        /* Event Handlers
           ---------------------------------------------------------------------------------------*/

        // Sets the visible label based on the property value. I could have used a BoolToVisibility converter
        // but I wanted to maintain the ease animation.
        private static void Running_Changed( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
            MonitorStatus ms = ( MonitorStatus )d;
            if( ms.IsRunning ) {
                ms.TransitionOn();
            }
            else {
                ms.TransitionOff();
            }
        }

        // Animates the transition to the 'On' state. If the Off label is visible, fades it out. 
        // Fades the On label in.
        private void TransitionOn() {
            if( lblOff.Opacity == 1 ) {
                AnimateProperty.EaseOpacityOut( lblOff, new Duration( TimeSpan.FromSeconds( .5 ) ) );
            }

            AnimateProperty.EaseOpacityIn( lblOn, new Duration( TimeSpan.FromSeconds( .5 ) ) );
        }

        // Animates the transition to the 'Off' state. If the On label is visible, fades it out. 
        // Fades the Off label in.
        private void TransitionOff() {
            if( lblOn.Opacity == 1 ) {
                AnimateProperty.EaseOpacityOut( lblOn, new Duration( TimeSpan.FromSeconds( .5 ) ) );
            }

            AnimateProperty.EaseOpacityIn( lblOff, new Duration( TimeSpan.FromSeconds( .5 ) ) );
        }
    }
}
