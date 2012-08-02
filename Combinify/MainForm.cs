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
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Forms;
    using System.Threading;

    public partial class frmMain : Form {
        private string _lastDir = "";
        private string _combineFile = "";
        private bool _isRunning = false;
        private Dictionary<string, DateTime> _fileTimes;

        public frmMain() {
            InitializeComponent();
            this._fileTimes = new Dictionary<string, DateTime>();
        }

        #region "Button Events"
        // Simple text color change on button mouse over
        private void Button_MouseEnter( object sender, EventArgs e ) {
            ControlHover.HoverOver( sender as Control );
        }

        // Simple text color change on button mouse out
        private void Button_MouseLeave( object sender, EventArgs e ) {
            ControlHover.HoverOut( sender as Control );
        }

        // Event for the Minify Single File button
        private void btnSingle_Click( object sender, EventArgs e ) {
            string path;

            if( Dialogs.GetOpenPath( out path, new OpenFileDialog(), "CSS Files (*.css)|*.css", "Open File", this._lastDir ) ) {
                txtSingle.Text = path;

                // Get the root of the file, from the start of the string to the last '\'
                string root = path.Substring( 0, ( path.LastIndexOf( "\\" ) + 1 ) );

                // Get the file name, less the extension (and any other dot notation)
                string file = ( new FileInfo( path ).Name ).Split( '.' )[ 0 ];

                // If the file already exists, notify the user that it will be replaced
                // If it doesnt exist skip to the writing
                if( File.Exists( root + file + ".min.css" ) ) {
                    if( MessageBox.Show( file + ".min.css already exists. This will replace it.", "Confirm Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning ) == DialogResult.OK ) {
                        WriteFile( path, root + file + ".min.css" );
                    }
                }
                else {
                    WriteFile( path, root + file + ".min.css" );
                }
            }
        }

        // Event for the Add Directory button
        private void btnDirectory_Click( object sender, EventArgs e ) {
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

                btnClear.Enabled = true;
                CheckReadyState();
            }
        }

        // Event for the Add Single File button
        private void btnAddFile_Click( object sender, EventArgs e ) {
            string path;
            if( Dialogs.GetOpenPath( out path, new OpenFileDialog(), "CSS Files (*.css)|*.css", "Open File", this._lastDir ) ) {
                // Don't add dupes
                if( !lstFiles.Items.Contains( path ) ) {
                    // Only want lastDir to be the directory, excluding file name
                    this._lastDir = path.Substring( 0, ( path.LastIndexOf( "\\" ) + 1 ) );
                    lstFiles.Items.Add( path );
                    btnClear.Enabled = true;
                    CheckReadyState();
                }
            }
        }

        // Event for the Clear List button
        private void btnClear_Click( object sender, EventArgs e ) {
            lstFiles.Items.Clear();
            ( ( Control )sender ).Enabled = false;
            btnClear.Enabled = false;
            btnStart.Enabled = false;
            smiStart.Enabled = false;
        }

        // Event for the Select Combine To File button
        private void btnCombineTo_Click( object sender, EventArgs e ) {
            if( Dialogs.GetSavePath( out this._combineFile, new SaveFileDialog(), "CSS Files (*.css)|*.css", "Save File", this._lastDir ) ) {
                txtCombine.Text = this._combineFile;
                CheckReadyState();
            }
        }

        /* Start monitoring directory button click event */
        private void btnStart_Click( object sender, EventArgs e ) {
            if( lstFiles.Items.Count > 0 && txtCombine.Text != string.Empty ) {
                StartStopMonitoring();
            }
        }

        /* Stop monitoring directory button click event */
        private void btnStop_Click( object sender, EventArgs e ) {
            StartStopMonitoring();
        }
        #endregion

        #region "List Context Menu Events"
        // Set the lists selected index to the list item that was clicked
        private void lstFiles_MouseDown( object sender, MouseEventArgs e ) {
            if( e.Button == MouseButtons.Right ) {
                lstFiles.SelectedIndex = lstFiles.IndexFromPoint( e.X, e.Y );
            }
        }

        // Check if there is a selected item and enable/disable context menu
        // items based on that
        private void cmsListOps_Opening( object sender, CancelEventArgs e ) {
            if( lstFiles.SelectedIndex == -1 ) {
                smiRemove.Enabled = false;
                smiUp.Enabled = false;
                smiDown.Enabled = false;
            }
            else {
                smiRemove.Enabled = true;
                // Check for start or end of list before enabling up and down
                smiUp.Enabled = lstFiles.SelectedIndex != 0 ? true : false;
                smiDown.Enabled = lstFiles.SelectedIndex != lstFiles.Items.Count - 1 ? true : false;
            }
        }

        // Remove item context menu click event
        private void smiRemove_Click( object sender, EventArgs e ) {
            if( MessageBox.Show( "Are you sure you want to remove this item?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning ) == DialogResult.Yes ) {
                lstFiles.Items.RemoveAt( lstFiles.SelectedIndex );
            }
        }

        // Move list item up context menu click event
        private void smiUp_Click( object sender, EventArgs e ) {
            int index = lstFiles.SelectedIndex;
            var tmp = lstFiles.Items[ index - 1 ];
            lstFiles.Items[ index - 1 ] = lstFiles.Items[ index ];
            lstFiles.Items[ index ] = tmp;
            lstFiles.SelectedIndex = index - 1;
        }

        // Move list item down context menu click event
        private void smiDown_Click( object sender, EventArgs e ) {
            int index = lstFiles.SelectedIndex;
            var tmp = lstFiles.Items[ index + 1 ];
            lstFiles.Items[ index + 1 ] = lstFiles.Items[ index ];
            lstFiles.Items[ index ] = tmp;
            lstFiles.SelectedIndex = index + 1;
        }
        #endregion

        #region "Tray Icon Related Methods"
        // Minize to the system tray event
        private void Form1_Resize( object sender, EventArgs e ) {
            if( this.WindowState == FormWindowState.Minimized ) {
                this.Hide();
                niTray.BalloonTipTitle = "Combinify";
                niTray.BalloonTipText = "Ninjas can't catch me if I'm hiding in your tray.";
                niTray.ShowBalloonTip( 3000 );
            }
        }

        // Restore from system tray event
        private void niTray_DoubleClick( object sender, EventArgs e ) {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        /* Start monitoring directory contexzt meny click event */
        private void smiStart_Click( object sender, EventArgs e ) {
            if( lstFiles.Items.Count > 0 && txtCombine.Text != string.Empty ) {
                StartStopMonitoring();
            }
        }

        /* Stop monitoring directory context menu click event */
        private void smiStop_Click( object sender, EventArgs e ) {
            StartStopMonitoring();
        }

        // Tray icon Close event
        private void smiClose_Click( object sender, EventArgs e ) {
            this.Close();
        }
        #endregion

        private void timeCheck_Tick( object sender, EventArgs e ) {
            // The stop button turns off the stopwatch but Ill check anyway
            if( this._isRunning && FilesHaveChanged() ) {
                ProcessFiles();
            }
        }

        // Enable the start buttons if all the needed fields are supplied
        private void CheckReadyState() {
            btnStart.Enabled = ( lstFiles.Items.Count > 0 && txtCombine.Text != string.Empty );
            smiStart.Enabled = ( lstFiles.Items.Count > 0 && txtCombine.Text != string.Empty );
        }

        // Flip the boolean properties related to starting and stopping
        // the monitoring routine
        private void StartStopMonitoring() {
            // Flip the flag
            this._isRunning = !this._isRunning;

            // Flip the control properties
            btnStart.Visible = !btnStart.Visible;
            btnStop.Visible = !btnStop.Visible;
            btnDirectory.Enabled = !btnDirectory.Enabled;
            btnClear.Enabled = !btnClear.Enabled;
            btnAddFile.Enabled = !btnAddFile.Enabled;
            btnCombineTo.Enabled = !btnCombineTo.Enabled;
            smiStart.Enabled = !smiStart.Enabled;
            smiStop.Enabled = !smiStop.Enabled;

            // Flip the stopwatch
            if( this._isRunning ) {
                // Clear the file time dictionary and then repopulate it
                // Im lazy and dont feel like doing checks
                _fileTimes.Clear();

                foreach( string f in lstFiles.Items.Cast<String>().ToList() ) {
                    var info = new FileInfo( f );
                    _fileTimes.Add( info.Name, info.LastWriteTime );
                }

                ProcessFiles();
                timeCheck.Start();
            }
            else {
                timeCheck.Stop();
            }
        }

        // Larger aggrigate for the file handling
        private void ProcessFiles() {
            // Cast the list items to a string list
            var files = lstFiles.Items.Cast<String>().ToList();

            // Determine the operation type by the radio that is checked 
            // then spin a new thread to do the operation on low priority,
            // and wait for the thread to finish to proceed
            if( radCombMin.Checked == true ) {
                var cmThread = new Thread( () => DoCombineMinify( files ) );
                cmThread.Priority = ThreadPriority.Lowest;
                cmThread.Start();
                cmThread.Join();
            }
            else {
                var cThread = new Thread( () => DoCombine( files ) );
                cThread.Priority = ThreadPriority.Lowest;
                cThread.Start();
                cThread.Join();
            }

            // Get the size of the minified (or combined, albeit, its pointless but better than zeros)
            // and the total combined size of the listed files and then figure the the amount of saves
            // soace after the operation
            long original = 0;
            long minified = new FileInfo( this._combineFile ).Length; ;
            foreach( string f in files ) {
                original += new FileInfo( f ).Length;
            }

            // Hope casting to double doesnt bite me but, long/long was returning (rounded)long aka 0
            double changed = ( ( double )minified / ( double )original ) * 10000;
            changed = Math.Truncate( changed ) / 100;

            tssTotal.Text = "Combined Size: " + AutoFileSize( original );
            tssMini.Text = "Post Size: " + AutoFileSize( minified );

            if( changed > 100.0 ) {
                changed = Math.Truncate( ( changed - 100.0 ) * 1000 ) / 1000;
                tssReduction.Text = "Change: +" + changed + "%";
            }
            else {
                tssReduction.Text = "Change: -" + changed + "%";
            }
            tssLast.Text = "Last: " + DateTime.Now.ToString( "h:mm:ss tt" );
        }

        // Method to perform the combining and minifying using Thread lambda
        private void DoCombineMinify( List<string> oldFile ) {
            using( var sw = new StreamWriter( this._combineFile, false ) ) {
                sw.Write( FileOp.MinifyFile( oldFile ) );
                sw.Close();
            }
        }

        // Method to perform the combining using Thread lambda
        private void DoCombine( List<string> oldFile ) {
            using( var sw = new StreamWriter( this._combineFile, false ) ) {
                sw.Write( FileOp.CombineFile( oldFile ) );
                sw.Close();
            }
        }

        // Used for the single file minify. Read the original file, 
        // minify, then write to the new file
        private void WriteFile( string oldPath, string newPath ) {
            using( var sw = new StreamWriter( newPath, false ) ) {
                sw.Write( FileOp.MinifyFile( oldPath ) );
                sw.Close();
            }

            // Get the size of the original and new file (in bytes as long data type)
            long original = new FileInfo( oldPath ).Length;
            long minified = new FileInfo( newPath ).Length;

            // Hope casting to double doesnt bite me but, long/long was returning (rounded)long aka 0
            double savings = ( ( double )minified / ( double )original ) * 10000;
            savings = Math.Truncate( savings ) / 100;

            // Notify user of completion and files sizes/savings
            MessageBox.Show( "Minification complete\n\nOriginal Size: " + AutoFileSize( original ) +
                             "\nMinified Size: " + AutoFileSize( minified ) +
                             "\nReduction: " + savings + "%" );
        }

        // Go through the list of files and check to see if they have been 
        // modified since the last check. If they have flip the flag. No
        // break since I want all changed times updated
        private bool FilesHaveChanged() {
            bool changed = false;
            var files = lstFiles.Items.Cast<String>().ToList();

            foreach( string f in files ) {
                var info = new FileInfo( f );
                if( _fileTimes[ info.Name ] < info.LastWriteTime ) {
                    _fileTimes[ info.Name ] = info.LastWriteTime;
                    changed = true;
                }
            }

            return changed;
        }

        // Formats from bytes to KB, MB, GB, TB - From LiFo's comment on SO
        // (http://stackoverflow.com/questions/5850596/conversion-of-long-to-decimal-in-c-sharp#answer-5850663)
        private string AutoFileSize( long number ) {
            double tmp = number;
            string suffix = " B ";

            if( tmp > 1024 ) { tmp = tmp / 1024; suffix = " KB"; }
            if( tmp > 1024 ) { tmp = tmp / 1024; suffix = " MB"; }
            if( tmp > 1024 ) { tmp = tmp / 1024; suffix = " GB"; }
            if( tmp > 1024 ) { tmp = tmp / 1024; suffix = " TB"; }

            return tmp.ToString( "n" ) + suffix;
        }
    }
}