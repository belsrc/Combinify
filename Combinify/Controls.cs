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
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// A collection of methods to transform elements of a WinForm control based on mouse over/out conditions.
    /// </summary>
    public static class ControlHover {
        /// <summary>
        /// Changes the text color of the control to light blue.
        /// </summary>
        /// <param name="control">WinForms control name.</param>
        public static void HoverOver( Control control ) {
            if( control.Enabled ) {
                control.ForeColor = SystemColors.MenuHighlight;
            }
        }

        /// <summary>
        /// Changes the text color of the control to the specified color.
        /// </summary>
        /// <param name="control">WinForms control name.</param>
        /// <param name="color">Color to use.</param>
        public static void HoverOver( Control control, Color color ) {
            if( control.Enabled ) {
                control.ForeColor = color;
            }
        }

        /// <summary>
        /// Changes the text color of the control to black.
        /// </summary>
        /// <param name="control">WinForms control name.</param>
        public static void HoverOut( Control control ) {
            if( control.Enabled && control.ForeColor != SystemColors.WindowText ) {
                control.ForeColor = SystemColors.WindowText;
            }
        }

        /// <summary>
        /// Changes the text color of the control to specified color.
        /// </summary>
        /// <param name="control">WinForms control name.</param>
        /// <param name="color">Color to use</param>
        public static void HoverOut( Control control, Color color ) {
            if( control.Enabled && control.ForeColor != color ) {
                control.ForeColor = color;
            }
        }
    }
}
