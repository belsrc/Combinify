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
        /// Project object.
        /// </summary>
        private Project _prj;

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
        private bool _needsSaved = false;

        /// <summary>
        /// Whether the app is monitoring the file list or not.
        /// </summary>
        private bool _isRunning = false;
        #endregion

        /// <summary>
        /// Initializes a new instance of the frmMain class.
        /// </summary>
        public frmMain() {
            InitializeComponent();
            this._prj = new Project();
            string[] args = Environment.GetCommandLineArgs();

            // Check if the user dbl-clicked a project file
            // if so, load it
            if( args.Length > 1 ) {
                OpenProject( args[ 1 ] );
                ChangeTitle();

                // I suppose you could technically have a project with no files
                if( lstFiles.Items.Count > 0 ) {
                    ChangeClearEnable( true );
                }
            }
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
            if( this._needsSaved ) {
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
            name += ( sender as Button ).Enabled ? string.Empty : "_grey";
            ( sender as Button ).BackgroundImage = ( Image )Properties.Resources.ResourceManager.GetObject( name );
        }

        /// <summary>
        /// Warn user of possible file replacement
        /// Disable/Enable Controls
        /// </summary>
        private void Radio_CheckedChanged( object sender, EventArgs e ) {
            // Kill monitoring
            if( this._isRunning ) {
                StartStopMonitoring();
            }
            
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
            // Make sure it isnt currently watching
            if( !this._isRunning ) {
                int index = lstFiles.SelectedIndex;

                if( index == -1 ) {
                    ChangeMovementEnabled( false );
                }
                else {
                    miListRemove.Enabled = smiRemove.Enabled = btnRemove.Enabled = true;

                    // Check for start or end of list before enabling up and down
                    miListUp.Enabled = smiUp.Enabled = btnUp.Enabled = index != 0 ? true : false;
                    miListDown.Enabled = smiDown.Enabled = btnDown.Enabled = index != lstFiles.Items.Count - 1 ? true : false;
                }
            }
        }
        #endregion

        #region "Common Click Event Handlers"
        /// <summary>
        /// Minify Single File click event.
        /// </summary>
        private void SingleFile_Click( object sender, EventArgs e ) {
            string path;
            bool wasSuccessful = false;

            if( Dialogs.GetOpenPath( out path, new OpenFileDialog(), "CSS Files (*.css)|*.css", "Open File", this._prj.LastDirectory ) ) {
                txtSingle.Text = path;
                FileInfo info = new FileInfo( path );

                // Get the root of the file, from the start of the string to the last '\'
                string root = this._prj.LastDirectory = info.DirectoryName + "\\";

                // Get the file name, less the extension (and any other dot notation)
                string file = info.Name.Split( '.' )[ 0 ];

                // If the file already exists, notify the user that it will be replaced
                // If it doesnt exist skip to the writing
                if( File.Exists( root + file + ".min.css" ) ) {
                    if( MessageBox.Show( file + ".min.css already exists. This will replace it.", "Confirm Save", 
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Warning ) == DialogResult.OK )
                    {
                        this.WriteFile( path, root + file + ".min.css" );
                        wasSuccessful = true;
                    }
                }
                else {
                    this.WriteFile( path, root + file + ".min.css" );
                    wasSuccessful = true;
                }

                // Since they could have denied the overwrite, have to check a flag
                if( wasSuccessful ) {
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
            string path;
            if( Dialogs.GetSavePath( out path, new SaveFileDialog(), "CSS Files (*.css)|*.css", "Save File", this._prj.LastDirectory ) ) {
                this._prj.DestinationFile = path;
                txtCombine.Text = this._prj.DestinationFile;
                this.CheckReadyState();
            }
        }

        /// <summary>
        /// Add directory click event.
        /// </summary>
        private void AddDirectory_Click( object sender, EventArgs e ) {
            string path;

            if( Dialogs.GetFolderPath( out path, new FolderBrowserDialog(), "Select a directory to watch", false, this._prj.LastDirectory ) ) {
                this._prj.LastDirectory = path;

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

                // Set the projects cssfile list to the list control
                this._prj.CssFiles = lstFiles.Items.Cast<string>().ToList();

                ChangeClearEnable( true );
                this._needsSaved = this._hasPrj;
                this.CheckReadyState();
            }
        }

        /// <summary>
        /// Add file click event.
        /// </summary>
        private void AddFile_Click( object sender, EventArgs e ) {
            string path;

            if( Dialogs.GetOpenPath( out path, new OpenFileDialog(), "CSS Files (*.css)|*.css", "Open File", this._prj.LastDirectory ) ) {
                // Don't add dupes
                if( !lstFiles.Items.Contains( path ) ) {
                    // Only want lastDir to be the directory, excluding file name
                    this._prj.LastDirectory = path.Substring( 0, ( path.LastIndexOf( "\\" ) + 1 ) );
                    this._prj.CssFiles.Add( path );
                    lstFiles.Items.Add( path );
                    ChangeClearEnable( true );
                    this._needsSaved = this._hasPrj;
                    this.CheckReadyState();
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
            // Just set the project list to the list controls new order
            this._prj.CssFiles = lstFiles.Items.Cast<string>().ToList();
            this._needsSaved = this._hasPrj;
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
            // Just set the project list to the list controls new order
            this._prj.CssFiles = lstFiles.Items.Cast<string>().ToList();
            this._needsSaved = this._hasPrj;
        }

        /// <summary>
        /// Remove item click event.
        /// </summary>
        private void RemoveFile_Click( object sender, EventArgs e ) {
            if( MessageBox.Show( "Are you sure you want to remove this item?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning ) == DialogResult.Yes ) {
                lstFiles.Items.RemoveAt( lstFiles.SelectedIndex );
                this._needsSaved = this._hasPrj;
            }
        }

        /// <summary>
        /// Clear list click event.
        /// Should be disabled when isRunning but Ill put a check anyway
        /// </summary>
        private void ClearList_Click( object sender, EventArgs e ) {
            if( !this._isRunning ) {
                if( MessageBox.Show( "Are you sure you want to clear the entire list?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning ) == DialogResult.Yes ) {
                    lstFiles.Items.Clear();
                    this._needsSaved = this._hasPrj;
                    miMonitorStart.Enabled = smiStart.Enabled = false;
                    this.ChangeClearEnable( false );
                    this.ChangeMovementEnabled( false );
                }
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

            // Kill monitoring
            if( this._isRunning ) {
                StartStopMonitoring();
            }

            if( Dialogs.GetOpenPath( out path, new OpenFileDialog(), "Combinify Project (*.cpj)|*.cpj", "Open Project", this._prj.LastDirectory ) ) {
                OpenProject( path );

                // I suppose you could technically have a project with no files
                if( lstFiles.Items.Count > 0 ) {
                    ChangeClearEnable( true );
                }
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
                if( this._needsSaved ) {
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

                // Stop monitoring
                if( this._isRunning ) {
                    StartStopMonitoring();
                    miMonitorStart.Enabled = btnClear.Enabled = false;
                }
            }
        }

        /// <summary>
        /// New project from file click event
        /// </summary>
        private void miProjectNewFile_Click( object sender, EventArgs e ) {
            string path = string.Empty;

            // Kill monitoring
            if( this._isRunning ) {
                StartStopMonitoring();
            }

            if( Dialogs.GetOpenPath( out path, new OpenFileDialog(), "CSS Files (*.css)|*.css", "Open File", this._prj.LastDirectory ) ) {
                SetNewProject( new FileInfo( path ).DirectoryName + "\\" );
                lstFiles.Items.Add( path );
            }
        }

        /// <summary>
        /// New project from folder click event
        /// </summary>
        private void miProjectNewDir_Click( object sender, EventArgs e ) {
            string path;

            // Kill monitoring
            if( this._isRunning ) {
                StartStopMonitoring();
            }

            if( Dialogs.GetFolderPath( out path, new FolderBrowserDialog(), "Select a directory to watch", false, this._prj.LastDirectory ) ) {
                SetNewProject( path );
                lstFiles.Items.AddRange( FileOp.GetCssFiles( path ).ToArray() );
            }
        }
        #endregion


    }
}