/*
    Copyright (c) 2012, Bryan Kizer
    All rights reserved. 

    Redistribution and use in source and binary forms, with or without 
    modification, are permitted provided that the following conditions are 
    met: 

    * Redistributions of source code must retain the above copyright notice, 
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of the Organization nor the names of its contributors 
      may be used to endorse or promote products derived from this software 
      without specific prior written permission. 
  
    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS 
    IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED 
    TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
    PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT 
    HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
    SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
    TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
    PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
    LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
    NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS 
    SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
namespace QuickMinCombine {
    using System;
    using System.Drawing;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// Partial class for the main form.
    /// </summary>
    public partial class frmMain : Form {
        #region "Class Variables"
        /// <summary>
        /// The last directory to be used.
        /// </summary>
        private string _lastDir = string.Empty;

        /// <summary>
        /// Whether theres a current working project
        /// </summary>
        private bool _hasPrj = false;

        /// <summary>
        /// The file path for current working project
        /// </summary>
        private string _prjPath = string.Empty;

        /// <summary>
        /// Flag for if the user needs to save the current project
        /// </summary>
        private bool _needSaved = false;

        /// <summary>
        /// Path for the combined file.
        /// </summary>
        private string _combineFile = string.Empty;

        /// <summary>
        /// Whether the app is monitoring the file list or not.
        /// </summary>
        private bool _isRunning = false;

        /// <summary>
        /// A dictionary containing the file name [key] and LastWriteTime [value].
        /// </summary>
        private Dictionary<string, DateTime> _fileTimes;
        #endregion

        /// <summary>
        /// Initializes a new instance of the frmMain class.
        /// </summary>
        public frmMain() {
            InitializeComponent();
            this._fileTimes = new Dictionary<string, DateTime>();
        }

        /// <summary>
        /// Form closing event
        /// </summary>
        private void frmMain_FormClosing( object sender, FormClosingEventArgs e ) {
            // Hide the tray icon, otherwise they keep piling up
            if( niTray.Visible == true ) {
                niTray.Visible = false;
            }

            // Check if the project needs to be saved
            if( this._needSaved ) {
                var result = SaveBeforeClose();

                // Yes or No result in the same effect at this level
                if( result == DialogResult.Cancel ) {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// This is so ugly, have to find a better way
        /// Preferably not making individual event handles
        /// </summary>
        private void Button_EnabledChanged( object sender, EventArgs e ) {
            string name = ( sender as Button ).Name;

            // Big ugly case
            if( !( sender as Button ).Enabled ) {
                switch( name ) {
                    case "btnUp":
                        ( sender as Button ).BackgroundImage = ( Image )Properties.Resources.ResourceManager.GetObject( "arrow_up_grey" );
                        break;

                    case "btnDown":
                        ( sender as Button ).BackgroundImage = ( Image )Properties.Resources.ResourceManager.GetObject( "arrow_down_grey" );
                        break;

                    case "btnRemove":
                        ( sender as Button ).BackgroundImage = ( Image )Properties.Resources.ResourceManager.GetObject( "minus_grey" );
                        break;

                    case "btnClear":
                        ( sender as Button ).BackgroundImage = ( Image )Properties.Resources.ResourceManager.GetObject( "cross_grey" );
                        break;

                    case "btnCombineTo":
                        ( sender as Button ).BackgroundImage = ( Image )Properties.Resources.ResourceManager.GetObject( "file_blank_grey" );
                        break;

                    case "btnAddDir":
                        ( sender as Button ).BackgroundImage = ( Image )Properties.Resources.ResourceManager.GetObject( "add_folder_grey" );
                        break;

                    case "btnAddFile":
                        ( sender as Button ).BackgroundImage = ( Image )Properties.Resources.ResourceManager.GetObject( "add_file_grey" );
                        break;
                };
            }
            else {
                switch( name ) {
                    case "btnUp":
                        ( sender as Button ).BackgroundImage = ( Image )Properties.Resources.ResourceManager.GetObject( "arrow-up" );
                        break;

                    case "btnDown":
                        ( sender as Button ).BackgroundImage = ( Image )Properties.Resources.ResourceManager.GetObject( "arrow-down" );
                        break;

                    case "btnRemove":
                        ( sender as Button ).BackgroundImage = ( Image )Properties.Resources.ResourceManager.GetObject( "minus" );
                        break;

                    case "btnClear":
                        ( sender as Button ).BackgroundImage = ( Image )Properties.Resources.ResourceManager.GetObject( "cross" );
                        break;

                    case "btnCombineTo":
                        ( sender as Button ).BackgroundImage = ( Image )Properties.Resources.ResourceManager.GetObject( "file-blank" );
                        break;

                    case "btnAddDir":
                        ( sender as Button ).BackgroundImage = ( Image )Properties.Resources.ResourceManager.GetObject( "add-folder" );
                        break;

                    case "btnAddFile":
                        ( sender as Button ).BackgroundImage = ( Image )Properties.Resources.ResourceManager.GetObject( "add-file" );
                        break;
                };
            }
        }

        /// <summary>
        /// Warn user of possible file replacement
        /// Disable/Enable Controls
        /// </summary>
        private void radMinify_CheckedChanged( object sender, EventArgs e ) {
            if( radMinify.Checked ) {
                MessageBox.Show( "This will replace any files with the name <file name>.min.css without further warning.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                txtCombine.Enabled = btnCombineTo.Enabled = false;
            }
            else {
                txtCombine.Enabled = btnCombineTo.Enabled = true;
            }

            CheckReadyState();
        }

        /// <summary>
        /// Timer tick event.
        /// </summary>
        private void timeCheck_Tick( object sender, EventArgs e ) {
            // The stop button turns off the stopwatch but Ill check anyway
            if( this._isRunning && this.FilesHaveChanged() ) {
                this.ProcessFiles();
            }
        }

        #region "Tray Icon Event Handlers"
        /// <summary>
        /// Minize to the system tray event.
        /// </summary>
        private void Form1_Resize( object sender, EventArgs e ) {
            if( this.WindowState == FormWindowState.Minimized ) {
                this.Hide();
                niTray.BalloonTipTitle = "Combinify";
                niTray.BalloonTipText = "Ninjas can't catch me if I'm hiding in your tray.";
                niTray.ShowBalloonTip( 3000 );
            }
        }

        /// <summary>
        /// Restore from system tray event.
        /// </summary>
        private void niTray_DoubleClick( object sender, EventArgs e ) {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
        #endregion

        #region "List Event Handlers"
        /// <summary>
        /// Set the lists selected index to the list item that was clicked.
        /// </summary>
        private void lstFiles_MouseDown( object sender, MouseEventArgs e ) {
            if( e.Button == MouseButtons.Right ) {
                lstFiles.SelectedIndex = lstFiles.IndexFromPoint( e.X, e.Y );
            }
        }

        /// <summary>
        /// Check if there is a selected item and enable/disable menu items based on that.
        /// </summary>
        private void lstFiles_SelectedIndexChanged( object sender, EventArgs e ) {
            if( lstFiles.SelectedIndex == -1 ) {
                miListRemove.Enabled = smiRemove.Enabled = btnRemove.Enabled = false;
                miListUp.Enabled = smiUp.Enabled = btnUp.Enabled = false;
                miListDown.Enabled = smiDown.Enabled = btnDown.Enabled = false;
            }
            else {
                miListRemove.Enabled = smiRemove.Enabled = btnRemove.Enabled = true;

                // Check for start or end of list before enabling up and down
                miListUp.Enabled = smiUp.Enabled = btnUp.Enabled = lstFiles.SelectedIndex != 0 ? true : false;
                miListDown.Enabled = smiDown.Enabled = btnDown.Enabled = lstFiles.SelectedIndex != lstFiles.Items.Count - 1 ? true : false;
            }
        }
        #endregion

        #region "Common Click Event Handlers"
        /// <summary>
        /// Minify Single File click event.
        /// </summary>
        private void SingleFile_Click( object sender, EventArgs e ) {
            string path;
            bool wasSuccess = false;

            if( Dialogs.GetOpenPath( out path, new OpenFileDialog(), "CSS Files (*.css)|*.css", "Open File", this._lastDir ) ) {
                txtSingle.Text = path;
                FileInfo info = new FileInfo( path );

                // Get the root of the file, from the start of the string to the last '\'
                string root = this._lastDir = info.DirectoryName + "\\";

                // Get the file name, less the extension (and any other dot notation)
                string file = info.Name.Split( '.' )[ 0 ];

                // If the file already exists, notify the user that it will be replaced
                // If it doesnt exist skip to the writing
                if( File.Exists( root + file + ".min.css" ) ) {
                    if( MessageBox.Show( file + ".min.css already exists. This will replace it.", "Confirm Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning ) == DialogResult.OK ) {
                        this.WriteFile( path, root + file + ".min.css" );
                        wasSuccess = true;
                    }
                }
                else {
                    this.WriteFile( path, root + file + ".min.css" );
                    wasSuccess = true;
                }

                // Since they could have denied the overwrite, have to check a flag
                if( wasSuccess ) {
                    string[] comp = this.GetFileComparison( path, root + file + ".min.css" );

                    // Notify user of completion and files sizes/savings
                    MessageBox.Show( "Minification complete\n\nOriginal Size: " + comp[ 0 ] +
                                     "\nMinified Size: " + comp[ 1 ] +
                                     "\nReduction: " + comp[ 2 ] + "%" );
                }
            }
        }

        /// <summary>
        /// Combine To File click event.
        /// </summary>
        private void CombineTo_Click( object sender, EventArgs e ) {
            if( Dialogs.GetSavePath( out this._combineFile, new SaveFileDialog(), "CSS Files (*.css)|*.css", "Save File", this._lastDir ) ) {
                txtCombine.Text = this._combineFile;
                this.CheckReadyState();
            }
        }

        /// <summary>
        /// Add directory click event.
        /// </summary>
        private void AddDirectory_Click( object sender, EventArgs e ) {
            string path;
            if( Dialogs.GetFolderPath( out path, new FolderBrowserDialog(), "Select a directory to watch", false, this._lastDir ) ) {
                this._lastDir = path;

                // If the list is empty go ahead and just add files
                // otherwise, check for dupes
                if( lstFiles.Items.Count == 0 ) {
                    lstFiles.Items.AddRange( FileOp.GetCssFiles( path ).ToArray() );
                }
                else {
                    var fl = FileOp.GetCssFiles( path );
                    foreach( string f in fl ) {
                        if( !lstFiles.Items.Contains( f ) ) {
                            lstFiles.Items.Add( f );
                        }
                    }
                }

                miListClear.Enabled = smiClear.Enabled = btnClear.Enabled = true;
                this._needSaved = this._hasPrj;
                this.CheckReadyState();
            }
        }

        /// <summary>
        /// Add file click event.
        /// </summary>
        private void AddFile_Click( object sender, EventArgs e ) {
            string path;
            if( Dialogs.GetOpenPath( out path, new OpenFileDialog(), "CSS Files (*.css)|*.css", "Open File", this._lastDir ) ) {
                // Don't add dupes
                if( !lstFiles.Items.Contains( path ) ) {
                    // Only want lastDir to be the directory, excluding file name
                    this._lastDir = path.Substring( 0, ( path.LastIndexOf( "\\" ) + 1 ) );
                    lstFiles.Items.Add( path );
                    miListClear.Enabled = smiClear.Enabled = btnClear.Enabled = true;
                    this.CheckReadyState();
                    this._needSaved = this._hasPrj;
                }
            }
        }

        /// <summary>
        /// Start monitoring click event.
        /// </summary>
        private void StartMon_Click( object sender, EventArgs e ) {
            if( lstFiles.Items.Count > 0 &&
              ( txtCombine.Text != string.Empty || radMinify.Checked ) ) {
                this.StartStopMonitoring();
            }
        }

        /// <summary>
        /// Stop monitoring click event.
        /// </summary>
        private void StopMon_Click( object sender, EventArgs e ) {
            this.StartStopMonitoring();
        }

        /// <summary>
        /// Move list item up click event.
        /// </summary>
        private void FileUp_Click( object sender, EventArgs e ) {
            int index = lstFiles.SelectedIndex;
            var tmp = lstFiles.Items[ index - 1 ];
            lstFiles.Items[ index - 1 ] = lstFiles.Items[ index ];
            lstFiles.Items[ index ] = tmp;
            lstFiles.SelectedIndex = index - 1;
            this._needSaved = this._hasPrj;
        }

        /// <summary>
        /// Move list item down click event.
        /// </summary>
        private void FileDown_Click( object sender, EventArgs e ) {
            int index = lstFiles.SelectedIndex;
            var tmp = lstFiles.Items[ index + 1 ];
            lstFiles.Items[ index + 1 ] = lstFiles.Items[ index ];
            lstFiles.Items[ index ] = tmp;
            lstFiles.SelectedIndex = index + 1;
            this._needSaved = this._hasPrj;
        }

        /// <summary>
        /// Remove item click event.
        /// </summary>
        private void RemoveFile_Click( object sender, EventArgs e ) {
            if( MessageBox.Show( "Are you sure you want to remove this item?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning ) == DialogResult.Yes ) {
                lstFiles.Items.RemoveAt( lstFiles.SelectedIndex );
                this._needSaved = this._hasPrj;
            }
        }

        /// <summary>
        /// Clear list click event.
        /// </summary>
        private void ClearList_Click( object sender, EventArgs e ) {
            if( MessageBox.Show( "Are you sure you want to clear the entire list?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning ) == DialogResult.Yes ) {
                lstFiles.Items.Clear();
                this._needSaved = this._hasPrj;
                miMonitorStart.Enabled = smiStart.Enabled = false;
                miListClear.Enabled = smiClear.Enabled = btnClear.Enabled = false;
                miListRemove.Enabled = smiRemove.Enabled = btnRemove.Enabled = false;
                miListUp.Enabled = smiUp.Enabled = btnUp.Enabled = false;
                miListDown.Enabled = smiDown.Enabled = btnDown.Enabled = false;
            }
        }

        /// <summary>
        /// Exit app click event.
        /// </summary>
        private void ExitApp_Click( object sender, EventArgs e ) {
            this.Close();
        }
        #endregion

        #region "Project Event Handlers"
        /// <summary>
        /// Open project click event
        /// </summary>
        private void miProjectOpen_Click( object sender, EventArgs e ) {
            string path = string.Empty;

            if( Dialogs.GetOpenPath( out path, new OpenFileDialog(), "Combinify Project (*.cpj)|*.cpj", "Save Project", this._lastDir ) ) {
                // Set the project fields
                this._hasPrj = true;
                this._prjPath = path;

                // Clear the list of any files
                lstFiles.Items.Clear();

                // Add the watched files from the project
                lstFiles.Items.AddRange( FileOp.ReadProject( out this._lastDir, this._prjPath ) );
            }
        }

        /// <summary>
        /// Save project click event
        /// </summary>
        private void miProjectSave_Click( object sender, EventArgs e ) {
            // If theres a current working project save to that one
            // else call the new save method
            if( this._hasPrj && this._prjPath != string.Empty ) {
                this.SaveProject();
            }
            else {
                this.SaveNewProject();
            }
        }

        /// <summary>
        /// Save project as click event
        /// </summary>
        private void miProjectSaveAs_Click( object sender, EventArgs e ) {
            this.SaveNewProject();
        }

        /// <summary>
        /// Close project click event
        /// </summary>
        private void miProjectClose_Click( object sender, EventArgs e ) {
            // Check if it actually has a working project
            if( this._hasPrj ) {

                // Check if the project needs to be saved
                if( this._needSaved ) {
                    var result = SaveBeforeClose();

                    // Yes or No result in the same effect 
                    // at this level so go ahead and empty the values
                    if( result == DialogResult.Yes ||
                        result == DialogResult.No ) {
                        CloseExistingProject();
                    }
                }
                else {
                    CloseExistingProject();
                }
            }
        }

        /// <summary>
        /// New project from file click event
        /// </summary>
        private void miProjectNewFile_Click( object sender, EventArgs e ) {
            string path = string.Empty;

            if( Dialogs.GetOpenPath( out path, new OpenFileDialog(), "CSS Files (*.css)|*.css", "Open File", this._lastDir ) ) {
                SetNewProject( new FileInfo( path ).DirectoryName + "\\" );
                lstFiles.Items.Add( path );
            }
        }

        /// <summary>
        /// New project from folder click event
        /// </summary>
        private void miProjectNewDir_Click( object sender, EventArgs e ) {
            string path;

            if( Dialogs.GetFolderPath( out path, new FolderBrowserDialog(), "Select a directory to watch", false, this._lastDir ) ) {
                SetNewProject( path );
                lstFiles.Items.AddRange( FileOp.GetCssFiles( path ).ToArray() );
            }
        }
        #endregion

        /// <summary>
        /// Enable the start buttons if all the needed fields are supplied.
        /// </summary>
        private void CheckReadyState() {
            miMonitorStart.Enabled = smiStart.Enabled = ( lstFiles.Items.Count > 0 &&
                                                        ( txtCombine.Text != string.Empty || radMinify.Checked ) );
        }

        /// <summary>
        /// Flip the boolean properties related to starting and stopping the monitoring routine.
        /// </summary>
        private void StartStopMonitoring() {
            // Flip the flag
            this._isRunning = !this._isRunning;

            // Flip the Enabled control properties
            btnCombineTo.Enabled = ( !this._isRunning && !radMinify.Checked );
            miMonitorStart.Enabled = smiStart.Enabled = !this._isRunning;
            miMonitorStop.Enabled = smiStop.Enabled = this._isRunning;
            miListDir.Enabled = smiDir.Enabled = btnAddDir.Enabled = !this._isRunning;
            miListFile.Enabled = smiFile.Enabled = btnAddFile.Enabled = !this._isRunning;
            miListClear.Enabled = smiClear.Enabled = btnClear.Enabled = !this._isRunning;

            // Flip the stopwatch
            if( this._isRunning ) {
                // Clear the file time dictionary and then repopulate it
                // Im lazy and dont feel like doing checks
                this._fileTimes.Clear();

                foreach( string f in lstFiles.Items.Cast<string>().ToList() ) {
                    var info = new FileInfo( f );
                    this._fileTimes.Add( info.Name, info.LastWriteTime );
                }

                this.ProcessFiles();
                timeCheck.Start();
            }
            else {
                timeCheck.Stop();
            }
        }

        /// <summary>
        /// Larger aggregate for the file handling.
        /// </summary>
        /// <remarks>
        /// Ugly method is ugly. Clean me PLEASE
        /// </remarks>
        private void ProcessFiles() {
            // Cast the list items to a string list
            var files = lstFiles.Items.Cast<string>().ToList();
            Thread thread;

            // Determine the operation type by the radio that is checked 
            // then spin a new thread to do the operation on low priority,
            // and wait for the thread to finish to proceed
            if( radCombine.Checked ) {
                thread = new Thread( () => this.DoCombine( files ) );
            }
            else if( radCombMin.Checked ) {
                thread = new Thread( () => this.DoCombineMinify( files ) );
            }
            else {
                thread = new Thread( () => this.DoMinify( files ) );
            }

            thread.Priority = ThreadPriority.Lowest;
            thread.Start();
            thread.Join();

            if( !radMinify.Checked ) {
                // Get the size of the minified (or combined, albeit, its pointless but better than zeros)
                // and the total combined size of the listed files and then figure the the amount of saves
                // soace after the operation
                long original = 0;
                long minified = new FileInfo( this._combineFile ).Length;
                foreach( string f in files ) {
                    original += new FileInfo( f ).Length;
                }

                // Hope casting to double doesnt bite me but, long/long was returning (rounded)long aka 0
                double changed = ( ( double )minified / ( double )original ) * 100;

                tssTotal.Text = "Combined Size: " + AutoFileSize( original );
                tssMini.Text = "Post Size: " + AutoFileSize( minified );

                if( changed > 100.0 ) {
                    changed = Math.Round( changed - 100.0, 2 );
                    tssReduction.Text = "Change: +" + changed + "%";
                }
                else {
                    tssReduction.Text = "Change: -" + Math.Round( 100 - changed, 2 ) + "%";
                }
            }
            else {
                tssTotal.Text = "Combined Size: ---";
                tssMini.Text = "Post Size: ---";
                tssReduction.Text = "Change: ---";
            }

            tssLast.Text = "Last: " + DateTime.Now.ToString( "h:mm:ss tt" );
        }

        /// <summary>
        /// Method to perform the combining using Thread lambda.
        /// </summary>
        /// <param name="oldFile">A list of file paths to combine.</param>
        private void DoCombine( List<string> oldFile ) {
            using( var sw = new StreamWriter( this._combineFile, false ) ) {
                sw.Write( FileOp.CombineFile( oldFile ) );
                sw.Close();
            }
        }

        /// <summary>
        /// Method to perform the combining and minifying using Thread lambda.
        /// </summary>
        /// <param name="oldFile">A list of file paths to combine and minify.</param>
        private void DoCombineMinify( List<string> oldFile ) {
            using( var sw = new StreamWriter( this._combineFile, false ) ) {
                sw.Write( FileOp.MinifyFile( oldFile ) );
                sw.Close();
            }
        }

        /// <summary>
        /// Method to perform the minifying using Thread lambda.
        /// </summary>
        /// <param name="oldFile">A list of file paths to minify.</param>
        private void DoMinify( List<string> oldFile ) {
            var files = lstFiles.Items.Cast<string>().ToList();

            foreach( string f in files ) {
                var info = new FileInfo( f );
                string root = info.DirectoryName + "\\";
                string file = info.Name.Split( '.' )[ 0 ];

                if( this._fileTimes[ info.Name ] < info.LastWriteTime ||
                    !File.Exists( root + file + ".min.css" ) ) {
                    this._fileTimes[ info.Name ] = info.LastWriteTime;
                    WriteFile( f, root + file + ".min.css" );
                }
            }
        }

        /// <summary>
        /// Used for the single file minify. 
        /// Read the original file, minify, then write to the new file.
        /// </summary>
        /// <param name="newPath">The file path of the original file.</param>
        /// <param name="oldPath">The file path of the new file.</param>
        private void WriteFile( string oldPath, string newPath ) {
            using( var sw = new StreamWriter( newPath, false ) ) {
                sw.Write( FileOp.MinifyFile( oldPath ) );
                sw.Close();
            }
        }

        /// <summary>
        /// Determines the file size of the the new file and old file and 
        /// determines the difference in file size.
        /// </summary>
        /// <param name="older">The path to the old version of the file.</param>
        /// <param name="newer">The path to the new version of the file.</param>
        /// <returns>
        /// A string array containing the original file size [0], 
        /// the new file size [1], and the difference between the sizes [2].
        /// </returns>
        private string[] GetFileComparison( string older, string newer ) {
            string[] tmp = new string[ 3 ];

            // Get the size of the original and new file (in bytes as long data type)
            long original = new FileInfo( older ).Length;
            long minified = new FileInfo( newer ).Length;

            // Hope casting to double doesnt bite me in the ass but,
            // long/long was returning (rounded)long aka 0
            double savings = ( ( double )minified / ( double )original ) * 100;
            savings = Math.Round( 100 - savings, 2 );

            tmp[ 0 ] = this.AutoFileSize( original );
            tmp[ 1 ] = this.AutoFileSize( minified );
            tmp[ 2 ] = savings.ToString();

            return tmp;
        }

        /// <summary>
        /// Go through the list of files and check to see if they have been 
        /// modified since the last check. If they have flip the flag. No 
        /// break since I want all changed times updated.
        /// </summary>
        private bool FilesHaveChanged() {
            bool changed = false;
            var files = lstFiles.Items.Cast<string>().ToList();

            foreach( string f in files ) {
                var info = new FileInfo( f );
                if( this._fileTimes[ info.Name ] < info.LastWriteTime ) {
                    this._fileTimes[ info.Name ] = info.LastWriteTime;
                    changed = true;
                }
            }

            return changed;
        }

        /// <summary>
        /// Formats from bytes to KB, MB, GB, TB.
        /// From LiFo's comment on SO 
        /// (stackoverflow.com/questions/5850596/conversion-of-long-to-decimal-in-c-sharp#answer-5850663)
        /// </summary>
        /// <param name="number">A long int representing the bytes of a file.</param>
        /// <returns>A string representing  bytes in KB, MB, GB, TB.</returns>
        private string AutoFileSize( long number ) {
            double tmp = number;
            string suffix = " B ";

            if( tmp > 1024 ) { tmp = tmp / 1024; suffix = " KB"; }
            if( tmp > 1024 ) { tmp = tmp / 1024; suffix = " MB"; }
            if( tmp > 1024 ) { tmp = tmp / 1024; suffix = " GB"; }
            if( tmp > 1024 ) { tmp = tmp / 1024; suffix = " TB"; }

            return tmp.ToString( "n" ) + suffix;
        }

        /// <summary>
        /// Sets various fields for the a new project
        /// </summary>
        /// <param name="path">Path of the selected file/folder.</param>
        private void SetNewProject( string path ) {
            this._lastDir = path;
            this._hasPrj = true;
            this._needSaved = true;
            lstFiles.Items.Clear();
            miListClear.Enabled = smiClear.Enabled = btnClear.Enabled = true;
        }

        /// <summary>
        /// Sets various fields when a project gets closed
        /// </summary>
        private void CloseExistingProject() {
            this._hasPrj = false;
            this._prjPath = string.Empty;
            this._needSaved = false;
            lstFiles.Items.Clear();
        }

        /// <summary>
        /// Saves the contents of the watch list before closing or clearing.
        /// </summary>
        /// <returns>
        /// Users choice of YES, NO, CANCEL
        /// Yes causes the project to save. No and Cancel are
        /// handled by the calling method
        /// </returns>
        private DialogResult SaveBeforeClose() {
            var result = MessageBox.Show( "Do you want to save the changes to the current project?",
                "Combinify", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );

            if( result == DialogResult.Yes ) {
                if( this._hasPrj && this._prjPath != string.Empty ) {
                    this.SaveProject();
                }
                else {
                    bool test = this.SaveNewProject();
                    result = test ? DialogResult.Yes : DialogResult.Cancel;
                }
            }

            return result;
        }

        /// <summary>
        /// Saves the contents of the watch list to a new project.
        /// </summary>
        /// <return>
        /// Returns <see langword="true"/> if the user selects a save path;
        /// otherwise, <see langword="false"/>.
        /// </return>
        private bool SaveNewProject() {
            string path = string.Empty;

            if( Dialogs.GetSavePath( out path, new SaveFileDialog(), "Combinify Project (*.cpj)|*.cpj", "Save Project", this._lastDir ) ) {
                // Set the project fields
                this._hasPrj = true;
                this._prjPath = path;

                this.SaveProject();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Saves the contents of the watch list to the current project.
        /// </summary>
        private void SaveProject() {
            FileOp.WriteProject( this._prjPath, this._lastDir, lstFiles.Items.Cast<string>().ToList() );
            this._needSaved = false;
        }
    }
}