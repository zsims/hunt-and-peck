using HuntnPeck.Engine.Hints;
using HuntnPeck.Engine.Renderer;
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
        /// The hint session
        /// </summary>
        private readonly HintSession _hintSession;

        /// <summary>
        /// The matching hints (if any)
        /// </summary>
        private IEnumerable<Hint> _matchingHints;

        /// <summary>
        /// The renderer used for drawing hints
        /// </summary>
        private readonly HintRenderer _hintRenderer;

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
            _hintRenderer = new HintRenderer();

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
            _hintRenderer.RenderHints(e.Graphics, _matchingHints, _hintSession.Hints);
        }

        private void ProcessInput(string currentInput)
        {
            _matchingHints = _hintLabelService.FindMatchingHints(currentInput, _hintSession.Hints);

            if (_matchingHints.Count() == 1)
            {
                SelectedHint = _matchingHints.First();
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                // repaint the hints
                Invalidate();
            }
        }

        private void textBoxHintInput_TextChanged(object sender, EventArgs e)
        {
            ProcessInput(textBoxHintInput.Text);
        }

        /// <summary>
        /// Called before the hint intput text box recieves the event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxHintInput_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        /// <summary>
        /// Called when the overlay loses focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HintOverlay_Deactivate(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Gets the selected hint, if any
        /// </summary>
        public Hint SelectedHint { get; private set; }
    }
}
