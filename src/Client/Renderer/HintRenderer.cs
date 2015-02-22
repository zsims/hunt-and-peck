using hap.Engine.Hints;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hap.Client.Renderer
{
    /// <summary>
    /// Renders hints 
    /// </summary>
    internal class HintRenderer : IHintRenderer
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
        /// Underlines for hint capabilities
        /// </summary>
        private readonly Dictionary<HintCapabilityIdentifer, Pen> _hintUnderlines;

        /// <summary>
        /// Pen for the bounding box of hints
        /// </summary>
        private readonly Pen _hintBoundingBoxPen;

        /// <summary>
        /// How many hint underline types there are
        /// </summary>
        private readonly int _hintUnderlinesCount;

        /// <summary>
        /// Brush for hint text
        /// </summary>
        private readonly Brush _hintBoxBrush;

        /// <summary>
        /// Ctor
        /// </summary>
        public HintRenderer()
        {
            _hintFont = new Font("Arial", 16);
            _hintTextBrush = new SolidBrush(Color.Red);
            _hintBoundingBoxPen = new Pen(Color.Blue, 1);
            _hintBoxBrush = new LinearGradientBrush(new Point(0, 0), new Point(0, 100), Color.FromArgb(255, 32, 124, 202), Color.FromArgb(255, 125, 185, 232));

            _hintUnderlines = new Dictionary<HintCapabilityIdentifer, Pen>()
            {
                {HintCapabilityIdentifer.Invoke, new Pen(Color.Blue, 2)}
            };

            _hintUnderlinesCount = _hintUnderlines.Count();
        }

        /// <summary>
        /// Renders the hints using the given graphics object
        /// </summary>
        /// <param name="graphics">The graphics object to use for rendering</param>
        /// <param name="matchingHints">The matching hints</param>
        /// <param name="allHints">All of the hints, matching and non-matching</param>
        public void RenderHints(Graphics graphics, IEnumerable<Hint> matchingHints, IEnumerable<Hint> allHints)
        {
            // TODO: Add "debug" mode where only 
            foreach (var hint in matchingHints.Where(x => x.Capabilities.Any()))
            {
                string label = hint.Label.ToUpper();

                // Draw the hint string + background
                var length = graphics.MeasureString(label, _hintFont);

                // box around the label
                //graphics.FillRectangle(_hintBoxBrush, (float)hint.BoundingRectangle.X, (float)hint.BoundingRectangle.Y, length.Width, length.Height);
                graphics.DrawString(label, _hintFont, _hintTextBrush, new PointF((float)hint.BoundingRectangle.X, (float)hint.BoundingRectangle.Y));

                // draw the bounding box
                graphics.DrawRectangle(_hintBoundingBoxPen,
                    (float)hint.BoundingRectangle.X,
                    (float)hint.BoundingRectangle.Y,
                    (float)hint.BoundingRectangle.Width,
                    (float)hint.BoundingRectangle.Height);

                DrawHintCapabilities(graphics, hint);
            }
        }

        /// <summary>
        /// Draws the hint capabilities
        /// </summary>
        /// <param name="graphics">The graphics object</param>
        /// <param name="hint">The hint to draw the capabilities of</param>
        private void DrawHintCapabilities(Graphics graphics, Hint hint)
        {
            // for the capabilities, draw a small N% underline
            var capabilityNo = 0;
            var capabilitySize = hint.BoundingRectangle.Width / _hintUnderlinesCount;
            var capabilityY = hint.BoundingRectangle.Bottom;
            foreach (var capability in hint.Capabilities)
            {
                var startX = hint.BoundingRectangle.Left + (capabilityNo * capabilitySize);
                var endX = startX + capabilitySize;

                Pen pen = null;

                if (_hintUnderlines.TryGetValue(capability.Identifier, out pen))
                {
                    graphics.DrawLine(pen, new PointF((float)startX, (float)capabilityY), new PointF((float)endX, (float)capabilityY));
                }

                ++capabilityNo;
            }
        }

    
        /// <summary>
        /// Disposes the renderer
        /// </summary>
        public void Dispose()
        {
            _hintFont.Dispose();
            _hintTextBrush.Dispose();
            _hintBoxBrush.Dispose();
            _hintBoundingBoxPen.Dispose();

            foreach (var pen in _hintUnderlines.Values)
            {
                pen.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
