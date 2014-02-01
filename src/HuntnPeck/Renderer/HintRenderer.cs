using HuntnPeck.Engine.Hints;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntnPeck.Engine.Renderer
{
    internal class HintRenderer
    {
        /// <summary>
        /// The font hints are drawn in
        /// </summary>
        private readonly Font _hintFont = new Font("Arial", 16);

        /// <summary>
        /// Brush for hint text
        /// </summary>
        private readonly Brush _hintTextBrush = new SolidBrush(Color.Red);

        /// <summary>
        /// Brush for hint text
        /// </summary>
        private readonly Brush _hintBoxBrush = new LinearGradientBrush(new Point(0, 0), new Point(0, 100), Color.FromArgb(255, 32, 124, 202), Color.FromArgb(255, 125, 185, 232));

        public void RenderHints(Graphics graphics, IEnumerable<Hint> matchingHints, IEnumerable<Hint> allHints)
        {
            foreach (var hint in matchingHints)
            {
                string label = hint.Label.ToUpper();

                // Draw the hint string + background
                var length = graphics.MeasureString(label, _hintFont);
                graphics.FillRectangle(_hintBoxBrush, (float)hint.BoundingRectangle.X, (float)hint.BoundingRectangle.Y, length.Width, length.Height);
                graphics.DrawString(label, _hintFont, _hintTextBrush, new PointF((float)hint.BoundingRectangle.X, (float)hint.BoundingRectangle.Y));
            }
        }
    }
}
