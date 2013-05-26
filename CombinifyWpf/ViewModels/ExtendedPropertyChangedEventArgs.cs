// -------------------------------------------------------------------------------
//    ExtendedPropertyChangedEventArgs.cs
//    
//    this code was taken from Joe's Comment on Josh Smith's Post :
//    http://joshsmithonwpf.wordpress.com/2007/08/29/a-base-class-which-implements-inotifypropertychanged/
// -------------------------------------------------------------------------------
namespace CombinifyWpf.ViewModels {
    using System.ComponentModel;

    /// <summary>
    /// Class to allow you to create one EventArg per class instead of one each time prop change
    /// </summary>
    public class ExtendedPropertyChangedEventArgs : PropertyChangedEventArgs {

        private string basePropertyName;

        /// <summary>
        /// Initializes a new instance of the ExtendedPropertyChangedEventArgs class.
        /// </summary>
        public ExtendedPropertyChangedEventArgs( string propertyName ) :
            base( propertyName ) {
            this.basePropertyName = propertyName;

        }


        public override string PropertyName { get { return this.basePropertyName; } }

        public string InternalProperyName { set { this.basePropertyName = value; } }
    }
}
