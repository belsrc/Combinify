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
        /// <summary>
        /// The last directory to be used.
        /// </summary>
        private string _lastDir = string.Empty;

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
        }

        /// <summary>
        /// This is so ugly, have to find a better way
        /// Preferably not making individual event handles
        /// </summary>
        private void Button_EnabledChanged( object sender, EventArgs e ) {
            string name = ( sender as Button ).Name;

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
                };
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
                miListUp.Enabled = smiUp.Enabled = btnUp.Enabled =  false;
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

        #region "Click Event Handlers"
        /// <summary>
        /// Minify Single File click event.
        /// </summary>
        private void SingleFile_Click( object sender, EventArgs e ) {
            string path;

            if( Dialogs.GetOpenPath( out path, new OpenFileDialog(), "CSS Files (*.css)|*.css", "Open File", this._lastDir ) ) {
                txtSingle.Text = this._lastDir = path;

                // Get the root of the file, from the start of the string to the last '\'
                string root = path.Substring( 0, ( path.LastIndexOf( "\\" ) + 1 ) );

                // Get the file name, less the extension (and any other dot notation)
                string file = new FileInfo( path ).Name.Split( '.' )[ 0 ];

                // If the file already exists, notify the user that it will be replaced
                // If it doesnt exist skip to the writing
                if( File.Exists( root + file + ".min.css" ) ) {
                    if( MessageBox.Show( file + ".min.css already exists. This will replace it.", "Confirm Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning ) == DialogResult.OK ) {
                        this.WriteFile( path, root + file + ".min.css" );
                    }
                }
                else {
                    this.WriteFile( path, root + file + ".min.css" );
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
                }
            }
        }

        /// <summary>
        /// Start monitoring click event.
        /// </summary>
        private void StartMon_Click( object sender, EventArgs e ) {
            if( lstFiles.Items.Count > 0 && txtCombine.Text != string.Empty ) {
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
        }

        /// <summary>
        /// Remove item click event.
        /// </summary>
        private void RemoveFile_Click( object sender, EventArgs e ) {
            if( MessageBox.Show( "Are you sure you want to remove this item?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning ) == DialogResult.Yes ) {
                lstFiles.Items.RemoveAt( lstFiles.SelectedIndex );
            }
        }

        /// <summary>
        /// Clear list click event.
        /// </summary>
        private void ClearList_Click( object sender, EventArgs e ) {
            if( MessageBox.Show( "Are you sure you want to clear the entire list?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning ) == DialogResult.Yes ) {
                lstFiles.Items.Clear();
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

        /// <summary>
        /// Timer tick event.
        /// </summary>
        private void timeCheck_Tick( object sender, EventArgs e ) {
            // The stop button turns off the stopwatch but Ill check anyway
            if( this._isRunning && this.FilesHaveChanged() ) {
                this.ProcessFiles();
            }
        }

        /// <summary>
        /// Enable the start buttons if all the needed fields are supplied.
        /// </summary>
        private void CheckReadyState() {
            miMonitorStart.Enabled = smiStart.Enabled = ( lstFiles.Items.Count > 0 && txtCombine.Text != string.Empty );
        }

        /// <summary>
        /// Flip the boolean properties related to starting and stopping the monitoring routine.
        /// </summary>
        private void StartStopMonitoring() {
            // Flip the flag
            this._isRunning = !this._isRunning;

            // Flip the Enabled control properties
            btnCombineTo.Enabled = !btnCombineTo.Enabled;
            miMonitorStart.Enabled = smiStart.Enabled = !smiStart.Enabled;
            miMonitorStop.Enabled = smiStop.Enabled = !smiStop.Enabled;
            miListDir.Enabled = smiDir.Enabled = btnAddDir.Enabled = !btnAddDir.Enabled;
            miListFile.Enabled = smiFile.Enabled = btnAddFile.Enabled = !btnAddFile.Enabled;
            miListClear.Enabled = smiClear.Enabled = btnClear.Enabled = !btnClear.Enabled;

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
        /// Larger aggrigate for the file handling.
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
        private void DoCombine( List<string> oldFile ) {
            using( var sw = new StreamWriter( this._combineFile, false ) ) {
                sw.Write( FileOp.CombineFile( oldFile ) );
                sw.Close();
            }
        }

        /// <summary>
        /// Method to perform the combining and minifying using Thread lambda.
        /// </summary>
        private void DoCombineMinify( List<string> oldFile ) {
            using( var sw = new StreamWriter( this._combineFile, false ) ) {
                sw.Write( FileOp.MinifyFile( oldFile ) );
                sw.Close();
            }
        }

        /// <summary>
        /// Method to perform the minifying using Thread lambda.
        /// </summary>
        private void DoMinify( List<string> oldFile ) {
            // parse the file times again,
            //  faster than mini all files when just one prolly changed
            // pass each changed one to the single mini
        }

        /// <summary>
        /// Used for the single file minify. 
        /// Read the original file, minify, then write to the new file.
        /// </summary>
        private void WriteFile( string oldPath, string newPath ) {
            using( var sw = new StreamWriter( newPath, false ) ) {
                sw.Write( FileOp.MinifyFile( oldPath ) );
                sw.Close();
            }

            // Get the size of the original and new file (in bytes as long data type)
            long original = new FileInfo( oldPath ).Length;
            long minified = new FileInfo( newPath ).Length;

            // Hope casting to double doesnt bite me but, long/long was returning (rounded)long aka 0
            double savings = ( ( double )minified / ( double )original ) * 100;
            savings = Math.Round( 100 - savings, 2 );

            // Notify user of completion and files sizes/savings
            MessageBox.Show( "Minification complete\n\nOriginal Size: " + this.AutoFileSize( original ) +
                             "\nMinified Size: " + this.AutoFileSize( minified ) +
                             "\nReduction: " + savings + "%" );
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
        private string AutoFileSize( long number ) {
            double tmp = number;
            string suffix = " B ";

            if( tmp > 1024 ) { tmp = tmp / 1024; suffix = " KB"; }
            if( tmp > 1024 ) { tmp = tmp / 1024; suffix = " MB"; }
            if( tmp > 1024 ) { tmp = tmp / 1024; suffix = " GB"; }
            if( tmp > 1024 ) { tmp = tmp / 1024; suffix = " TB"; }

            return tmp.ToString( "n" ) + suffix;
        }

        private void miProjectNewFile_Click( object sender, EventArgs e ) {

        }

        private void miProjectNewDir_Click( object sender, EventArgs e ) {

        }

        private void miProjectOpen_Click( object sender, EventArgs e ) {

        }

        private void miProjectSave_Click( object sender, EventArgs e ) {

        }

        private void miProjectSaveAs_Click( object sender, EventArgs e ) {

        }
    }
}