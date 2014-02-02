using HuntAndPeck.Engine.Hints;
using HuntAndPeck.Engine.Services.Interfaces;
using HuntAndPeck.UserInterface.Renderer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace HuntAndPeck.UserInterface.Forms
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
        private readonly IHintRenderer _hintRenderer;

        /// <summary>
        /// The service for labeling/matching hints
        /// </summary>
        private IHintLabelService _hintLabelService;

        public HintOverlay(IHintLabelService hintLabelService, IHintRenderer renderer, HintSession hintSession)
        {
            InitializeComponent();

            _hintLabelService = hintLabelService;
            _hintSession = hintSession;
            _matchingHints = hintSession.Hints;
            _hintRenderer = renderer;

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
                HintMatched(_matchingHints.First());
            }
            else
            {
                // repaint the hints
                Invalidate();
            }
        }

        private void HintMatched(Hint selectedHint)
        {
            // Closing, so we don't need to know about focus loss
            Deactivate -= HintOverlay_Deactivate;

            SelectedHint = selectedHint;
            DialogResult = DialogResult.OK;
            Close();
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
