// -------------------------------------------------------------------------------
//    SelectedAdorner.cs
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
namespace CombinifyWpf.Adorners {
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// A simple outlined selected item adorner.
    /// </summary>
    public class SelectedAdorner :Adorner {

        /// <summary>
        /// Initializes a new instance of the SelectedAdorner class.
        /// </summary>
        public SelectedAdorner( UIElement targetElement ) 
            : base( targetElement ) { }

        /// <summary>
        /// Draws the content of a DrawingContext object during the render pass of a Panel element.
        /// </summary>
        /// <param name="drawingContext">The drawing instructions for a specific element.</param>
        protected override void OnRender( DrawingContext drawingContext ) {
            var color = Color.FromRgb( 27, 161, 226 );
            var size = this.AdornedElement.RenderSize;

            // Draw the control outline
            Rect adornerRect = new Rect( size );

            // Draw the filled in corner area
            Point cornerStart = new Point( size.Width - 25, 0 );
            LineSegment[] cornerSegments = new LineSegment[] {
                new LineSegment( new Point( size.Width, 0 ), true ),
                new LineSegment( new Point( size.Width, 25 ), true )
            };
            PathFigure cornerFigure = new PathFigure( cornerStart, cornerSegments, true );
            PathGeometry cornerGeo = new PathGeometry( new PathFigure[] { cornerFigure } );

            // Draw the corner checkmark
            var checkStart = new Point( size.Width - 11, 6 );
            var checkMid = new Point( size.Width - 8, 11 );
            var checkEnd = new Point( size.Width - 2, 2 );

            drawingContext.DrawRectangle( null, new Pen( new SolidColorBrush( color ), 3 ), adornerRect );
            drawingContext.DrawGeometry( new SolidColorBrush( color ), null, cornerGeo );
            drawingContext.DrawLine( new Pen( Brushes.White, 2 ), checkStart, checkMid );
            drawingContext.DrawLine( new Pen( Brushes.White, 2 ), checkMid, checkEnd );
        }
    }
}
