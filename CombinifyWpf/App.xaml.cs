
namespace CombinifyWpf {
    using System.Windows;
    using CombinifyWpf.Models;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        // Pulled from another project source, leaving as a single element array since I'm
        // lazy and dont want to change the other code
        private static string[] _types = new string[] { ".css" };

        /// <summary>
        /// Gets the list of file types to include in the treeview.
        /// </summary>
        public static string[] FileTypes {
            get { return _types; }
        }
    }
}
