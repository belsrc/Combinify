
namespace CombinifyWpf {
    using System.Windows;
    using CombinifyWpf.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        // Pulled from another project source, leaving as a single element array since I'm
        // lazy and dont want to change the other code
        private static Dictionary<string, string> _types = new Dictionary<string, string>() {
            {"Stylesheet",".css"},
            {"Project",".cpj"}
        };

        /// <summary>
        /// Gets the list of file types to include in the treeview.
        /// </summary>
        public static Dictionary<string,string> FileTypes {
            get { return _types; }
        }
    }
}
