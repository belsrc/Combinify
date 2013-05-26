// -------------------------------------------------------------------------------
//    StatusBar.xaml.cs
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
    using System.Windows.Controls;
    using System.Windows;

    /// <summary>
    /// Interaction logic for StatusBar.xaml.
    /// </summary>
    public partial class StatusBar : UserControl {
        
        /// <summary>
        /// Initializes a new instance of the StatusBar class.
        /// </summary>
        public StatusBar() {
            InitializeComponent();
        }

        /* Properties
           ---------------------------------------------------------------------------------------*/

        /// <summary>
        /// Gets or sets the original size of the watched file[s].
        /// </summary>
        public long OriginalSize {
            get { return ( long )GetValue( OriginalSizeProperty ); }
            set { SetValue( OriginalSizeProperty, value ); }
        }

        /// <summary>
        /// Dependency Property for OriginalSize.
        /// </summary>
        public static readonly DependencyProperty OriginalSizeProperty =
            DependencyProperty.Register( "OriginalSize",
                                         typeof( long ),
                                         typeof( StatusBar ),
                                         new PropertyMetadata( 0L )
                                       );

        /// <summary>
        /// Gets or sets the post execution size of the watched file[s].
        /// </summary>
        public long PostSize {
            get { return ( long )GetValue( PostSizeProperty ); }
            set { SetValue( PostSizeProperty, value ); }
        }

        /// <summary>
        /// Dependency Property for PostSize.
        /// </summary>
        public static readonly DependencyProperty PostSizeProperty =
            DependencyProperty.Register( "PostSize",
                                         typeof( long ),
                                         typeof( StatusBar ),
                                         new PropertyMetadata( 0L )
                                       );

        /// <summary>
        /// Gets or sets the size change after the operation.
        /// </summary>
        public double ChangeAmount {
            get { return ( long )GetValue( ChangeAmountProperty ); }
            set { SetValue( ChangeAmountProperty, value ); }
        }

        /// <summary>
        /// Dependency Property for ChangeAmount.
        /// </summary>
        public static readonly DependencyProperty ChangeAmountProperty =
            DependencyProperty.Register( "ChangeAmount",
                                         typeof( double ),
                                         typeof( StatusBar ),
                                         new PropertyMetadata( 0D )
                                       );

        /// <summary>
        /// Gets or sets the last execution time.
        /// </summary>
        public DateTime LastExecuted {
            get { return ( DateTime )GetValue( LastExecutedProperty ); }
            set { SetValue( LastExecutedProperty, value ); }
        }

        /// <summary>
        /// Dependency Property for LastExecuted.
        /// </summary>
        public static readonly DependencyProperty LastExecutedProperty =
            DependencyProperty.Register( "LastExecuted",
                                         typeof( DateTime ),
                                         typeof( StatusBar ),
                                         new PropertyMetadata( DateTime.Now )
                                       );
    }
}
