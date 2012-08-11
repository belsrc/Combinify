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
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// Class for easily calling the Open and Save file dialog in WinForms and 
    /// getting the return path string.
    /// Originally from the my CGUM library but there wasnt much point including 
    /// the whole thing just for this class
    /// </summary>
    public static class Dialogs {
        /// <summary>
        /// Configures and displays OpenFile control, returns bool selected, outs file path.
        /// </summary>
        /// <param name="path">File path of the selected file.</param>
        /// <param name="openFile">OpenFileDialog control name.</param>
        /// <param name="fileFilter"><c>(Optional)</c> File filter string.</param>
        /// <param name="msgTitle"><c>(Optional)</c>Title of the dialog window.</param>
        /// <param name="startDir">The initial directory to open the control at.</param>
        /// <returns>
        /// If the user clicks the OK button of the dialog that is displayed and the path 
        /// is not <see langword="null"/>, <see langword="true"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">openFile is null.</exception>
        public static bool GetOpenPath( out string path,
                                        OpenFileDialog openFile,
                                        string fileFilter = "All Files (*.*)|*.*",
                                        string msgTitle = "Open File",
                                        string startDir = "" ) 
        {
            if( openFile == null ) {
                throw new ArgumentNullException( "OpenFileDialog was null" );
            }

            openFile.Title = msgTitle;
            openFile.InitialDirectory = startDir == string.Empty ?
                                                    Directory.GetCurrentDirectory() :
                                                    startDir;
            openFile.FileName = string.Empty;
            openFile.Filter = fileFilter;

            if( openFile.ShowDialog() == DialogResult.OK ) {
                path = openFile.FileName;
                return true;
            }
            else {
                path = string.Empty;
                return false;
            }
        }

        /// <summary>
        /// Configures and displays SaveFile control, returns bool selected, outs file path.
        /// </summary>
        /// <param name="path">File path of the selected file.</param>
        /// <param name="saveFile">SaveFileDialog control name.</param>
        /// <param name="fileFilter"><c>(Optional)</c> File filter string.</param>
        /// <param name="msgTitle"><c>(Optional)</c>Title of the dialog window.</param>
        /// <param name="startDir">The initial directory to open the control at.</param>
        /// <returns>
        /// If the user clicks the OK button of the dialog that is displayed and the path 
        /// is not <see langword="null"/>, <see langword="true"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">saveFile is null.</exception>
        public static bool GetSavePath( out string path,
                                        SaveFileDialog saveFile,
                                        string fileFilter = "All Files (*.*)|*.*",
                                        string msgTitle = "Save File",
                                        string startDir = "" ) 
        {
            if( saveFile == null ) {
                throw new ArgumentNullException( "SaveFileDialog was null" );
            }

            saveFile.Title = msgTitle;
            saveFile.InitialDirectory = startDir == string.Empty ?
                                                    Directory.GetCurrentDirectory() :
                                                    startDir;
            saveFile.FileName = string.Empty;
            saveFile.Filter = fileFilter;

            if( saveFile.ShowDialog() == DialogResult.OK ) {
                path = saveFile.FileName;
                return true;
            }
            else {
                path = string.Empty;
                return false;
            }
        }

        /// <summary>
        /// Configures and displays FolderBrowser control, returns bool selected, outs file path.
        /// </summary>
        /// <param name="path">Folder path of the selected directory.</param>
        /// <param name="folderBrowse">FolderBrowserDialog control name.</param>
        /// <param name="msgTitle"><c>Optional</c>Title of the dialog window.</param>
        /// <param name="hasNewBtn"><c>(Optional)</c> Whether to allow 'New Folder' button.</param>
        /// <param name="startDir">The initial directory to open the control at.</param>
        /// <returns>
        /// If the user clicks the OK button of the dialog that is displayed and the path 
        /// is not <see langword="null"/>, <see langword="true"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool GetFolderPath( out string path, 
                                          FolderBrowserDialog folderBrowse, 
                                          string msgTitle = "Open Folder", 
                                          bool hasNewBtn = true, 
                                          string startDir = "" ) 
        {
            if( folderBrowse == null ) {
                throw new ArgumentNullException( "FolderBrowserDialog was null" );
            }

            folderBrowse.Description = msgTitle;
            folderBrowse.ShowNewFolderButton = hasNewBtn;
            folderBrowse.SelectedPath = startDir == string.Empty ? 
                                                    Directory.GetCurrentDirectory() : 
                                                    startDir;

            if( folderBrowse.ShowDialog() == DialogResult.OK ) {
                path = folderBrowse.SelectedPath;
                return true;
            }
            else {
                path = string.Empty;
                return false;
            }
        }
    }
}