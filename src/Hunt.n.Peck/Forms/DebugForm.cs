using Engine.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hunt.n.Peck.Forms
{
    public partial class DebugForm : Form
    {
        private UiAutomationHintProviderService _service;

        public DebugForm()
        {
            InitializeComponent();

            _service = new UiAutomationHintProviderService();
        }

        private void buttonTarget_Click(object sender, EventArgs e)
        {
            var hints = _service.EnumHints();
            var bitmap = _service.RenderDebugHints(hints);
            if (bitmap != null)
            {
                pictureBoxDebug.Image = bitmap;
            }
        }
    }
}
