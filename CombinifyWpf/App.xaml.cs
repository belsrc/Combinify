
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

        private static Dictionary<string, string> _icons = new Dictionary<string, string>() {
            {".css",  @"pack://application:,,,/Images/css-64x64.png"},
            {".html", @"pack://application:,,,/Images/html-64x64.png"},
            {".js",   @"pack://application:,,,/Images/javascript-64x64.png"},
            {".php",  @"pack://application:,,,/Images/php-64x64.png"},
            {".xml",  @"pack://application:,,,/Images/xml-64x64.png"},
            {".cpj",  @"pack://application:,,,/Images/project-64x64.png"},
            {".png",  @"pack://application:,,,/Images/image-64x64.png"},
            {".jpg",  @"pack://application:,,,/Images/image-64x64.png"},
            {".jpeg",  @"pack://application:,,,/Images/image-64x64.png"}
        };

        /// <summary>
        /// Gets the list of file types to include in the treeview.
        /// </summary>
        public static Dictionary<string,string> FileTypes {
            get { return _types; }
        }

        /// <summary>
        /// Gets the list of icons to show in the file dialog.
        /// </summary>
        public static Dictionary<string,string> FileIcons {
            get { return _icons; }
        }
    }
}
