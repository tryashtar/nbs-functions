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
            this.OpenButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.FileLabel = new System.Windows.Forms.Label();
            this.DivLabel = new System.Windows.Forms.Label();
            this.TempoInput = new System.Windows.Forms.NumericUpDown();
            this.BpsLabel = new System.Windows.Forms.Label();
            this.BpsPanel = new System.Windows.Forms.Panel();
            this.QuickInstallBox = new System.Windows.Forms.TextBox();
            this.CopyCommandButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TempoInput)).BeginInit();
            this.BpsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenButton
            // 
            this.OpenButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.OpenButton.Location = new System.Drawing.Point(7, 9);
            this.OpenButton.Margin = new System.Windows.Forms.Padding(4);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(240, 78);
            this.OpenButton.TabIndex = 0;
            this.OpenButton.Text = "Open NBS File";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Enabled = false;
            this.SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.SaveButton.Location = new System.Drawing.Point(7, 95);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(4);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(240, 78);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Save Functions";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // FileLabel
            // 
            this.FileLabel.AutoSize = true;
            this.FileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.FileLabel.Location = new System.Drawing.Point(254, 9);
            this.FileLabel.Name = "FileLabel";
            this.FileLabel.Size = new System.Drawing.Size(0, 29);
            this.FileLabel.TabIndex = 2;
            // 
            // DivLabel
            // 
            this.DivLabel.AutoSize = true;
            this.DivLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.DivLabel.Location = new System.Drawing.Point(5, 5);
            this.DivLabel.Name = "DivLabel";
            this.DivLabel.Size = new System.Drawing.Size(59, 29);
            this.DivLabel.TabIndex = 3;
            this.DivLabel.Text = "20 ÷";
            // 
            // TempoInput
            // 
            this.TempoInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.TempoInput.Location = new System.Drawing.Point(10, 34);
            this.TempoInput.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.TempoInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TempoInput.Name = "TempoInput";
            this.TempoInput.Size = new System.Drawing.Size(62, 34);
            this.TempoInput.TabIndex = 4;
            this.TempoInput.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TempoInput.ValueChanged += new System.EventHandler(this.TempoInput_ValueChanged);
            // 
            // BpsLabel
            // 
            this.BpsLabel.AutoSize = true;
            this.BpsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.BpsLabel.Location = new System.Drawing.Point(78, 37);
            this.BpsLabel.Name = "BpsLabel";
            this.BpsLabel.Size = new System.Drawing.Size(105, 29);
            this.BpsLabel.TabIndex = 5;
            this.BpsLabel.Text = "= 20 bps";
            // 
            // BpsPanel
            // 
            this.BpsPanel.Controls.Add(this.BpsLabel);
            this.BpsPanel.Controls.Add(this.DivLabel);
            this.BpsPanel.Controls.Add(this.TempoInput);
            this.BpsPanel.Location = new System.Drawing.Point(259, 95);
            this.BpsPanel.Name = "BpsPanel";
            this.BpsPanel.Size = new System.Drawing.Size(200, 78);
            this.BpsPanel.TabIndex = 6;
            this.BpsPanel.Visible = false;
            // 
            // QuickInstallBox
            // 
            this.QuickInstallBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.QuickInstallBox.Location = new System.Drawing.Point(259, 179);
            this.QuickInstallBox.Multiline = true;
            this.QuickInstallBox.Name = "QuickInstallBox";
            this.QuickInstallBox.Size = new System.Drawing.Size(339, 58);
            this.QuickInstallBox.TabIndex = 7;
            this.QuickInstallBox.Visible = false;
            // 
            // CopyCommandButton
            // 
            this.CopyCommandButton.Enabled = false;
            this.CopyCommandButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.CopyCommandButton.Location = new System.Drawing.Point(7, 180);
            this.CopyCommandButton.Name = "CopyCommandButton";
            this.CopyCommandButton.Size = new System.Drawing.Size(240, 58);
            this.CopyCommandButton.TabIndex = 8;
            this.CopyCommandButton.Text = "Copy Command";
            this.CopyCommandButton.UseVisualStyleBackColor = true;
            this.CopyCommandButton.Click += new System.EventHandler(this.CopyCommandButton_Click);
            // 
            // TheForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 250);
            this.Controls.Add(this.CopyCommandButton);
            this.Controls.Add(this.QuickInstallBox);
            this.Controls.Add(this.BpsPanel);
            this.Controls.Add(this.FileLabel);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.OpenButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TheForm";
            this.Text = "NBS to Disc";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TheForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.TempoInput)).EndInit();
            this.BpsPanel.ResumeLayout(false);
            this.BpsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label FileLabel;
        private System.Windows.Forms.Label DivLabel;
        private System.Windows.Forms.NumericUpDown TempoInput;
        private System.Windows.Forms.Label BpsLabel;
        private System.Windows.Forms.Panel BpsPanel;
        private System.Windows.Forms.TextBox QuickInstallBox;
        private System.Windows.Forms.Button CopyCommandButton;
    }
}

