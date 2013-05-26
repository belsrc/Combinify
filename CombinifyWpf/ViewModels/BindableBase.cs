// -------------------------------------------------------------------------------
//    BindableBase.cs
//    Copyright (c) 2012 Bryan Kizer
//    All rights reserved.
//
//    The bulk of this code was taken from Brian Schroer's Posts :
//    http://geekswithblogs.net/brians/archive/2010/08/02/inotifypropertychanged-with-less-code-using-an-expression.aspx
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
namespace CombinifyWpf.ViewModels {
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Base class for all bindable view models
    /// </summary>
    public abstract class BindableBase : INotifyPropertyChanged {

        private ExtendedPropertyChangedEventArgs basePropertyArgs;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Initializes a new instance of the BindableBase class.
        /// </summary>
        public BindableBase() { }

        /// <summary>
        /// Notifies that all properties have changed.
        /// </summary>
        protected bool NotifyAll() {
            PropertyChanged( this, new PropertyChangedEventArgs( null ) );
            return true;
        }

        /// <summary>
        /// Notifies that a property has changed. Used for readonly properties that calculate the value.
        /// </summary>
        protected bool NotifyProperty<T>( Expression<Func<T>> expr ) {
            var body = expr.Body as MemberExpression;
            if( body != null ) {
                if( this.basePropertyArgs == null ) {
                    this.basePropertyArgs = new ExtendedPropertyChangedEventArgs( body.Member.Name );
                }
                else {
                    this.basePropertyArgs.InternalProperyName = body.Member.Name;
                }

                PropertyChanged( this, this.basePropertyArgs );
            }

            return true;
        }

        /// <summary>
        /// Sets property and notifies change. For use with normal field backed properties.
        /// </summary>
        /// <remarks>
        /// In the case of .net4.5 you can use [CallerMemberName] String propertyName = null
        /// </remarks>
        protected bool ChangeProperty<T>( T value, ref T currentValue, Expression<Func<T>> expr ) {
            if( value.Equals( currentValue ) ) {
                return false;
            }

            currentValue = value;
            var body = expr.Body as MemberExpression;
            if( body != null ) {
                if( this.basePropertyArgs == null ) {
                    this.basePropertyArgs = new ExtendedPropertyChangedEventArgs( body.Member.Name );
                }
                else {
                    this.basePropertyArgs.InternalProperyName = body.Member.Name;
                }

                PropertyChanged( this, this.basePropertyArgs );
            }

            return true;
        }

        /// <summary>
        /// Sets property and notifies change. For use with object property backed properties that cant be used with 'ref'
        /// </summary>
        protected bool ChangeProperty<T, U>( T value, U refObj, Expression<Func<U, T>> current, Expression<Func<T>> property ) {
            if( refObj == null ) {
                return false;
            }

            var cur = ( current.Body as MemberExpression ).Member as PropertyInfo;
            var curName = cur.Name;
            var curVal = cur.GetValue( refObj, null );

            if( value.Equals( curVal ) ) {
                return false;
            }

            typeof( U ).GetProperty( curName ).SetValue( refObj, value, null );

            var body = property.Body as MemberExpression;
            if( body != null ) {
                if( this.basePropertyArgs == null ) {
                    this.basePropertyArgs = new ExtendedPropertyChangedEventArgs( body.Member.Name );
                }
                else {
                    this.basePropertyArgs.InternalProperyName = body.Member.Name;
                }

                PropertyChanged( this, this.basePropertyArgs );
            }

            return true;
        }

    }
}
