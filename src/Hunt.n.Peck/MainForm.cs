using Hunt.n.Peck.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hunt.n.Peck
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void debugHintsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var debugForm = new DebugForm())
            {
                debugForm.ShowDialog();
            }
        }
    }
}
