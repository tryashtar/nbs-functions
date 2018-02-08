namespace NBSDisc {
    partial class TheForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TheForm));
            this.OpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.OpenButton = new System.Windows.Forms.Button();
            this.OutputBox = new NBSDisc.SingularityScintilla();
            this.SplitCheck = new System.Windows.Forms.CheckBox();
            this.ClickLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // openNbs
            // 
            this.OpenDialog.Filter = "Note Block Songs|*.nbs";
            // 
            // OpenButton
            // 
            this.OpenButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.OpenButton.Location = new System.Drawing.Point(7, 13);
            this.OpenButton.Margin = new System.Windows.Forms.Padding(4);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(240, 78);
            this.OpenButton.TabIndex = 0;
            this.OpenButton.Text = "Open NBS File";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // OutputBox
            // 
            this.OutputBox.AdditionalSelectionTyping = true;
            this.OutputBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputBox.AutoResize = NBSDisc.ResizeMode.None;
            this.OutputBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OutputBox.Enabled = false;
            this.OutputBox.Font = new System.Drawing.Font("Consolas", 16F);
            this.OutputBox.Location = new System.Drawing.Point(7, 100);
            this.OutputBox.Margin = new System.Windows.Forms.Padding(4);
            this.OutputBox.Multiline = true;
            this.OutputBox.MultiPaste = ScintillaNET.MultiPaste.Each;
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.ScrollWidth = 1;
            this.OutputBox.Size = new System.Drawing.Size(1024, 517);
            this.OutputBox.TabIndex = 1;
            this.OutputBox.VirtualSpaceOptions = ScintillaNET.VirtualSpace.RectangularSelection;
            this.OutputBox.Watermark = "";
            // 
            // SplitCheck
            // 
            this.SplitCheck.AutoSize = true;
            this.SplitCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.SplitCheck.Location = new System.Drawing.Point(254, 13);
            this.SplitCheck.Name = "SplitCheck";
            this.SplitCheck.Size = new System.Drawing.Size(365, 29);
            this.SplitCheck.TabIndex = 2;
            this.SplitCheck.Text = "Using command blocks (not functions)";
            this.SplitCheck.UseVisualStyleBackColor = true;
            // 
            // ClickLabel
            // 
            this.ClickLabel.AutoSize = true;
            this.ClickLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ClickLabel.Location = new System.Drawing.Point(254, 66);
            this.ClickLabel.Name = "ClickLabel";
            this.ClickLabel.Size = new System.Drawing.Size(248, 25);
            this.ClickLabel.TabIndex = 3;
            this.ClickLabel.Text = "Triple-click a line to select it";
            this.ClickLabel.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 625);
            this.Controls.Add(this.ClickLabel);
            this.Controls.Add(this.SplitCheck);
            this.Controls.Add(this.OutputBox);
            this.Controls.Add(this.OpenButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "NBS to Disc";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog OpenDialog;
        private System.Windows.Forms.Button OpenButton;
        private SingularityScintilla OutputBox;
        private System.Windows.Forms.CheckBox SplitCheck;
        private System.Windows.Forms.Label ClickLabel;
    }
}

