using HuntnPeck.Engine.Hints;
using HuntnPeck.Engine.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace HuntnPeck.Forms
{
    public partial class HintOverlay : Form
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

        /// <summary>
        /// The hint session
        /// </summary>
        private readonly HintSession _hintSession;

        /// <summary>
        /// The current input
        /// </summary>
        private string _currentInput = string.Empty;

        /// <summary>
        /// The matching hints (if any)
        /// </summary>
        private IEnumerable<Hint> _matchingHints;

        /// <summary>
        /// The service for labeling/matching hints
        /// </summary>
        private IHintLabelService _hintLabelService;

        public HintOverlay(IHintLabelService hintLabelService, HintSession hintSession)
        {
            InitializeComponent();

            _hintLabelService = hintLabelService;
            _hintSession = hintSession;
            _matchingHints = hintSession.Hints;

            // Size to the hint session owning window
            Top = (int)hintSession.OwningWindowBounds.Top;
            Left = (int)hintSession.OwningWindowBounds.Left;
            Width = (int)hintSession.OwningWindowBounds.Width;
            Height = (int)hintSession.OwningWindowBounds.Height;
        }

        private void OverlayForm_Load(object sender, EventArgs e)
        {
            Opacity = 0.5;

            // label the hints
            _hintLabelService.LabelHints(_hintSession.Hints);
        }

        private void OverlayForm_Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;

            foreach (var hint in _matchingHints)
            {
                string label = hint.Label.ToUpper();

                // Draw the hint string + background
                var length = graphics.MeasureString(label, _hintFont);
                graphics.FillRectangle(_hintBoxBrush, (float)hint.BoundingRectangle.X, (float)hint.BoundingRectangle.Y, length.Width, length.Height);
                graphics.DrawString(label, _hintFont, _hintTextBrush, new PointF((float)hint.BoundingRectangle.X, (float)hint.BoundingRectangle.Y));
            }
        }

        private void ProcessInput()
        {
            _matchingHints = _hintLabelService.FindMatchingHints(_currentInput, _hintSession.Hints);

            if (_matchingHints.Count() == 1)
            {
                SelectedHint = _matchingHints.First();
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // repaint the hints
                this.Invalidate();
            }
        }

        private static char GetChar(KeyEventArgs e)
        {
            int keyValue = e.KeyValue;
            if (!e.Shift && keyValue >= (int)Keys.A && keyValue <= (int)Keys.Z)
                return (char)(keyValue + 32);
            return (char)keyValue;
        }

        private void OverlayForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                this.Close();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Back && _currentInput.Length > 0)
            {
                _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
                e.SuppressKeyPress = true;
                ProcessInput();
            }
            else if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                _currentInput += GetChar(e);
                e.SuppressKeyPress = true;
                ProcessInput();
            }
        }

        /// <summary>
        /// Gets the selected hint, if any
        /// </summary>
        public Hint SelectedHint { get; private set; }
    }
}
