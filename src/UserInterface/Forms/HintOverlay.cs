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
        /// The matching hints (if any)
        /// </summary>
        private IEnumerable<Hint> _matchingHints = new Hint[0];

        /// <summary>
        /// The renderer used for drawing hints
        /// </summary>
        private readonly IHintRenderer _hintRenderer;

        /// <summary>
        /// The service for labeling/matching hints
        /// </summary>
        private IHintLabelService _hintLabelService;

        public HintOverlay(IHintLabelService hintLabelService, IHintRenderer renderer)
        {
            InitializeComponent();

            _hintLabelService = hintLabelService;
            _hintRenderer = renderer;
        }

        public HintSession HintSession { get; set; }

        private void OverlayForm_Load(object sender, EventArgs e)
        {
            Opacity = 0.5;

            // Size to the hint session owning window
            Top = (int)HintSession.OwningWindowBounds.Top;
            Left = (int)HintSession.OwningWindowBounds.Left;
            Width = (int)HintSession.OwningWindowBounds.Width;
            Height = (int)HintSession.OwningWindowBounds.Height;

            // label the hints
            _hintLabelService.LabelHints(HintSession.Hints);

            // everything is matching initially...
            _matchingHints = HintSession.Hints;
        }

        private void OverlayForm_Paint(object sender, PaintEventArgs e)
        {
            _hintRenderer.RenderHints(e.Graphics, _matchingHints, HintSession.Hints);
        }

        private void ProcessInput(string currentInput)
        {
            _matchingHints = _hintLabelService.FindMatchingHints(currentInput, HintSession.Hints);

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
