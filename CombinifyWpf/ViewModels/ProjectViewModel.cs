// -------------------------------------------------------------------------------
//    ProjectViewModel.cs
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
namespace CombinifyWpf.ViewModels {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CombinifyWpf.Models;
    using CombinifyWpf.Utils;

    /// <summary>
    /// View model for the Project model.
    /// </summary>
    public class ProjectViewModel : BindableBase {

        private Project _model;
        private long _combSize;
        private long _procSize;

        /// <summary>
        /// Initializes a new instance of the ProjectViewModel class.
        /// </summary>
        public ProjectViewModel() {
            if( this._model == null ) {
                Model = new Project();
            }
        }

        /* Properties
           ---------------------------------------------------------------------------------------*/

        /// <summary>
        /// Gets or sets the model that backs this view model.
        /// </summary>
        public Project Model {
            get { return this._model; }
            set {
                ChangeProperty( value, ref this._model, () => Model );
                NotifyAll();
            }
        }

        /// <summary>
        /// Gets or sets the projects last directory. 
        /// </summary>
        public string LastDirectory {
            get { return this._model.LastDirectory; }
            set { ChangeProperty( value, Model, x => x.LastDirectory, () => LastDirectory ); }
        }

        /// <summary>
        /// Gets or sets the projects destination file. 
        /// </summary>
        public string Destination {
            get { return this._model.DestinationFile; }
            set { ChangeProperty( value, Model, x => x.DestinationFile, () => Destination ); }
        }

        /// <summary>
        /// Gets or sets whether the project is currently being watched
        /// </summary>
        public bool IsWatching {
            get { return this._model.IsWatching; }
            set { ChangeProperty( value, Model, x => x.IsWatching, () => IsWatching ); }
        }

        /// <summary>
        /// Gets or sets a list of the projects watched file paths. 
        /// </summary>
        public List<string> WatchedFiles {
            get { return this._model.WatchedFiles; }
            set { ChangeProperty( value, Model, x => x.WatchedFiles, () => WatchedFiles ); }
        }

        /// <summary>
        /// Gets or sets a dictionary list of the watched files LastWriteTimes. 
        /// Fires the TimesChanged event.
        /// </summary>
        public Dictionary<string, DateTime> FileTimes {
            get { return this._model.FileTimes; }
            set { ChangeProperty( value, Model, x => x.FileTimes, () => FileTimes ); }
        }

        /// <summary>
        /// Gets or sets the path for the project file.
        /// </summary>
        public string ProjectSavePath { get; set; }

        /// <summary>
        /// Gets the combined size of all the watched files.
        /// </summary>
        public long CombinedSize {
            get {
                long original = 0L;
                if( WatchedFiles != null ) {
                    foreach( string f in WatchedFiles ) {
                        original += new FileInfo( f ).Length;
                    }
                }

                this._combSize = original;
                return original;
            }
        }

        /// <summary>
        /// Gets the size of the post processed file.
        /// </summary>
        public long ProcessedSize {
            get {
                long size = 0L;
                if( !string.IsNullOrWhiteSpace( Destination ) ) {
                    size = new FileInfo( Destination ).Length;
                }

                this._procSize = size;
                return size;
            }
        }

        /// <summary>
        /// Gets the difference in size from the pre and post processed file(s).
        /// </summary>
        public double Difference {
            get {
                double savings = 0;
                if( this._combSize > 0 && this._procSize > 0 ) {
                    savings = 1 - ( ( double )this._procSize / ( double )this._combSize );
                }

                return savings;
            }
        }

        /// <summary>
        /// Gets the last time the files were processed.
        /// </summary>
        public DateTime LastTime {
            get {
                return DateTime.Now;
            }
        }

        /* Methods
           ---------------------------------------------------------------------------------------*/

        /// <summary>
        /// Add a single file to the watched list. 
        /// </summary>
        /// <param name="path">File path.</param>
        public void AddFile( string path ) {
            if( !WatchedFiles.Contains( path ) ) {
                if( File.Exists( path ) ) {
                    WatchedFiles.Add( path );
                    WatchedFiles = WatchedFiles.ToList();
                }
            }
        }

        /// <summary>
        /// Adds all the CSS files from the specified directory to the watch list.
        /// </summary>
        /// <param name="path"></param>
        public void AddDirectory( string path ) {
            var pio = new ProjectIO();
            if( Directory.Exists( path ) ) {
                var tmp = pio.GetCssFiles( path );
                foreach( string s in tmp ) {
                    if( WatchedFiles.Contains( s ) ) {
                        tmp.Remove( s );
                    }
                }

                WatchedFiles.AddRange( tmp );
                WatchedFiles = WatchedFiles.ToList();
            }
        }

        /// <summary>
        /// Removes the specified item from the watch list. 
        /// </summary>
        /// <param name="item">The item to remove from the list.</param>
        public void RemoveItem( string item ) {
            if( WatchedFiles.Contains( item ) ) {
                WatchedFiles = WatchedFiles.Where( s => s != item ).ToList();
            }
        }

        /// <summary>
        /// Clears the files from the watch list.
        /// </summary>
        public void ClearWatched() {
            if( WatchedFiles != null ) {
                WatchedFiles.Clear();
                WatchedFiles = WatchedFiles.ToList();
            }

            if( FileTimes != null ) {
                FileTimes.Clear();
            }
        }

        /// <summary>
        /// Resets the current project.
        /// </summary>
        public void Close() {
            ProjectSavePath = string.Empty;
            LastDirectory = string.Empty;
            Destination = string.Empty;
            WatchedFiles = new List<string>();
            FileTimes = new Dictionary<string, DateTime>();
            IsWatching = false;
        }

        /// <summary>
        /// Checks the files in the watch list and 
        /// </summary>
        /// <returns>true if the files need processes, otherwise, false.</returns>
        public bool FilesNeedProcessed() {
            bool changed = false;

            if( WatchedFiles == null ) {
                return false;
            }

            if( FileTimes == null ) {
                return true;
            }

            foreach( string f in WatchedFiles ) {
                var info = new FileInfo( f );
                if( FileTimes[ info.Name ] < info.LastWriteTime ) {
                    changed = true;
                }
            }

            return changed;
        }

        /// <summary>
        /// Processes the CSS files depending on the user selected options.
        /// </summary>
        public void ProcessFiles() {
            Task.Factory.StartNew( () => {
                var proc = new Processor();
                var pio = new ProjectIO();
                string css = proc.ProcessFiles( WatchedFiles );
                SetTimeStamps();
                pio.SaveProcessedFile( Destination, css );

            } ).ContinueWith( ( result ) => {
                NotifyProperty( () => CombinedSize );
                NotifyProperty( () => ProcessedSize );
                NotifyProperty( () => Difference );
                NotifyProperty( () => LastTime );
            }, TaskContinuationOptions.OnlyOnRanToCompletion );
        }

        private void SetTimeStamps() {
            if( FileTimes == null ) {
                FileTimes = new Dictionary<string, DateTime>();
            }
            else {
                FileTimes.Clear();
            }

            foreach( var f in WatchedFiles ) {
                var info = new FileInfo( f );
                FileTimes.Add( info.Name, info.LastWriteTime );
            }
        }
    }
}
