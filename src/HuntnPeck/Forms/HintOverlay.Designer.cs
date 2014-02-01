namespace HuntnPeck.Forms
{
	partial class HintOverlay
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.textBoxHintInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxHintInput
            // 
            this.textBoxHintInput.Location = new System.Drawing.Point(54, 56);
            this.textBoxHintInput.Name = "textBoxHintInput";
            this.textBoxHintInput.Size = new System.Drawing.Size(100, 20);
            this.textBoxHintInput.TabIndex = 0;
            this.textBoxHintInput.TextChanged += new System.EventHandler(this.textBoxHintInput_TextChanged);
            this.textBoxHintInput.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textBoxHintInput_PreviewKeyDown);
            // 
            // HintOverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.textBoxHintInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.Name = "HintOverlay";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "HuntAndPeck";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.HintOverlay_Deactivate);
            this.Load += new System.EventHandler(this.OverlayForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OverlayForm_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.TextBox textBoxHintInput;
	}
}