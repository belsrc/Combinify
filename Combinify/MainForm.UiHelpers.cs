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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;

    public partial class frmMain {
        /// <summary>
        /// Opens an existing project.
        /// </summary>
        /// <param name="path">Project file path.</param>
        private void OpenProject( string path ) {
            // Set the project fields
            this._hasPrj = true;
            this._prjPath = path;

            // Set the title
            ChangeTitle();

            // Clear the list of any files
            lstFiles.Items.Clear();

            // Add the watched files from the project
            if( FileOp.ReadProject( out this._prj, this._prjPath ) ) {
                lstFiles.Items.AddRange( this._prj.CssFiles.ToArray() );
                txtCombine.Text = this._prj.DestinationFile;
            }
            else {
                MessageBox.Show( "The selected file was an invalid project" );
            }
        }

        /// <summary>
        /// Sets various fields for the a new project
        /// </summary>
        /// <param name="path">Path of the selected file/folder.</param>
        private void SetNewProject( string path ) {
            this._prj.LastDirectory = path;
            this._hasPrj = true;
            this._needsSaved = true;
            lstFiles.Items.Clear();
            miListClear.Enabled = smiClear.Enabled = btnClear.Enabled = true;
        }

        /// <summary>
        /// Sets various fields when a project gets closed
        /// </summary>
        private void CloseExistingProject() {
            this._hasPrj = false;
            this._prjPath = string.Empty;
            this._needsSaved = false;
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

            if( Dialogs.GetSavePath( out path, new SaveFileDialog(), "Combinify Project (*.cpj)|*.cpj", "Save Project", this._prj.LastDirectory ) ) {
                // Set the project fields
                this._hasPrj = true;
                this._prjPath = path;

                this.SaveProject();
                this.ChangeTitle();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Saves the contents of the watch list to the current project.
        /// </summary>
        private void SaveProject() {
            FileOp.WriteProject( this._prjPath, this._prj );
            this._needsSaved = false;
        }

        /// <summary>
        /// Enable the start buttons if all the needed fields are supplied.
        /// </summary>
        private void CheckReadyState() {
            miMonitorStart.Enabled = smiStart.Enabled = ( lstFiles.Items.Count > 0 &&
                                                        ( txtCombine.Text != string.Empty || radMinify.Checked ) );
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
                if( this._prj.FileTimes[ info.Name ] < info.LastWriteTime ) {
                    this._prj.FileTimes[ info.Name ] = info.LastWriteTime;
                    changed = true;
                }
            }

            return changed;
        }

        /// <summary>
        /// Flip the boolean properties related to starting and stopping the monitoring routine.
        /// </summary>
        private void StartStopMonitoring() {
            // Flip the flag
            this._isRunning = !this._isRunning;
            lstFiles.SelectedIndex = -1;

            // Flip the Enabled control properties
            btnCombineTo.Enabled = !this._isRunning;

            miMonitorStart.Enabled = smiStart.Enabled = !this._isRunning;
            miMonitorStop.Enabled = smiStop.Enabled = this._isRunning;

            this.ChangeAddEnabled( !this._isRunning );
            this.ChangeClearEnable( !this._isRunning );
            this.ChangeMovementEnabled( !this._isRunning );

            // Flip the stopwatch
            if( this._isRunning ) {
                // Clear the file time dictionary and then repopulate it
                // Im lazy and dont feel like doing checks
                this._prj.FileTimes.Clear();

                foreach( string f in lstFiles.Items.Cast<string>().ToList() ) {
                    var info = new FileInfo( f );
                    this._prj.FileTimes.Add( info.Name, info.LastWriteTime );
                }

                this.ProcessFiles();
                timeCheck.Start();
                lblMonitoring.Visible = true;
                lblStopped.Visible = false;
            }
            else {
                timeCheck.Stop();
                lblMonitoring.Visible = false;
                lblStopped.Visible = true;
            }
        }

        /// <summary>
        /// Larger aggregate for the file handling.
        /// </summary>
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
                long minified = new FileInfo( this._prj.DestinationFile ).Length;
                long original = 0;

                foreach( string f in files ) {
                    original += new FileInfo( f ).Length;
                }

                string[] comparison = this.GetFileComparison( original, minified );

                tssTotal.Text = "Combined Size: " + comparison[ 0 ];
                tssMini.Text = "Post Size: " + comparison[ 1 ];
                tssReduction.Text = "Change: " + comparison[ 2 ];
            }
            else {
                tssTotal.Text = "Combined Size: ---";
                tssMini.Text = "Post Size: ---";
                tssReduction.Text = "Change: ---";
            }

            tssLast.Text = "Last: " + DateTime.Now.ToString( "h:mm:ss tt" );
        }

        /// <summary>
        /// Change the add file/directory buttons enabled state.
        /// </summary>
        /// <param name="isEnabled">Whether they should be enabled or not.</param>
        private void ChangeAddEnabled( bool isEnabled ) {
            miListDir.Enabled = smiDir.Enabled = btnAddDir.Enabled = isEnabled;
            miListFile.Enabled = smiFile.Enabled = btnAddFile.Enabled = isEnabled;
        }

        /// <summary>
        /// Change the clear buttons enabled state.
        /// </summary>
        /// <param name="isEnabled">Whether they should be enabled or not.</param>
        private void ChangeClearEnable( bool isEnabled ) {
            miListClear.Enabled = isEnabled;
            smiClear.Enabled = isEnabled;
            btnClear.Enabled = isEnabled;
        }

        /// <summary>
        /// Change the move up/down buttons enabled state.
        /// </summary>
        /// <param name="isEnabled">Whether they should be enabled or not.</param>
        private void ChangeMovementEnabled( bool isEnabled ) {
            miListRemove.Enabled = smiRemove.Enabled = btnRemove.Enabled = isEnabled;
            miListUp.Enabled = smiUp.Enabled = btnUp.Enabled = isEnabled;
            miListDown.Enabled = smiDown.Enabled = btnDown.Enabled = isEnabled;
        }

        /// <summary>
        /// Method to perform the combining using Thread lambda.
        /// </summary>
        /// <param name="oldFile">A list of file paths to combine.</param>
        private void DoCombine( List<string> oldFile ) {
            using( var sw = new StreamWriter( this._prj.DestinationFile, false ) ) {
                sw.Write( FileOp.CombineFile( oldFile ) );
                sw.Close();
            }
        }

        /// <summary>
        /// Method to perform the combining and minifying using Thread lambda.
        /// </summary>
        /// <param name="oldFile">A list of file paths to combine and minify.</param>
        private void DoCombineMinify( List<string> oldFile ) {
            using( var sw = new StreamWriter( this._prj.DestinationFile, false ) ) {
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

                if( this._prj.FileTimes[ info.Name ] < info.LastWriteTime ||
                    !File.Exists( root + file + ".min.css" ) ) {
                        this._prj.FileTimes[ info.Name ] = info.LastWriteTime;
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
            long minified = new FileInfo( newer ).Length; ;

            return GetFileComparison( original, minified );
        }

        /// <summary>
        /// Determines the file size of the the new file and old file and 
        /// determines the difference in file size.
        /// </summary>
        /// <param name="older">The old file size in bytes.</param>
        /// <param name="newer">The new files size in bytes.</param>
        /// <returns>
        /// A string array containing the original file size [0], 
        /// the new file size [1], and the difference between the sizes [2].
        /// </returns>
        private string[] GetFileComparison( long older, long newer ) {
            string[] tmp = new string[ 3 ];

            // Hope casting to double doesnt bite me in the ass but,
            // long/long was returning (rounded)long aka 0
            double savings = ( ( double )newer / ( double )older ) * 100;
            savings = Math.Round( 100 - savings, 2 );

            tmp[ 0 ] = this.AutoFileSize( older );
            tmp[ 1 ] = this.AutoFileSize( newer );
            tmp[ 2 ] = savings.ToString();

            return tmp;
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
        /// Change the window title to include the project name.
        /// </summary>
        private void ChangeTitle() {
            string title = ( new FileInfo( this._prjPath ).Name ).Split( '.' )[ 0 ];
            this.Text = title + " - Combinify";
        }
    }
}
