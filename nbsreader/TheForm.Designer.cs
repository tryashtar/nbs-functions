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
            this.BpsLabel = new System.Windows.Forms.Label();
            this.BpsPanel = new System.Windows.Forms.Panel();
            this.FunctionInput = new System.Windows.Forms.NumericUpDown();
            this.FunctionLabel = new System.Windows.Forms.Label();
            this.BpsInput = new System.Windows.Forms.NumericUpDown();
            this.QuickInstallBox = new System.Windows.Forms.TextBox();
            this.CopyCommandButton = new System.Windows.Forms.Button();
            this.BpsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FunctionInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BpsInput)).BeginInit();
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
            // BpsLabel
            // 
            this.BpsLabel.AutoSize = true;
            this.BpsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.BpsLabel.Location = new System.Drawing.Point(33, 8);
            this.BpsLabel.Name = "BpsLabel";
            this.BpsLabel.Size = new System.Drawing.Size(53, 29);
            this.BpsLabel.TabIndex = 5;
            this.BpsLabel.Text = "bps";
            // 
            // BpsPanel
            // 
            this.BpsPanel.Controls.Add(this.FunctionInput);
            this.BpsPanel.Controls.Add(this.FunctionLabel);
            this.BpsPanel.Controls.Add(this.BpsInput);
            this.BpsPanel.Controls.Add(this.BpsLabel);
            this.BpsPanel.Location = new System.Drawing.Point(259, 81);
            this.BpsPanel.Name = "BpsPanel";
            this.BpsPanel.Size = new System.Drawing.Size(438, 92);
            this.BpsPanel.TabIndex = 6;
            this.BpsPanel.Visible = false;
            // 
            // FunctionInput
            // 
            this.FunctionInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.FunctionInput.Location = new System.Drawing.Point(3, 47);
            this.FunctionInput.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FunctionInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.FunctionInput.Name = "FunctionInput";
            this.FunctionInput.ReadOnly = true;
            this.FunctionInput.Size = new System.Drawing.Size(24, 38);
            this.FunctionInput.TabIndex = 8;
            this.FunctionInput.ValueChanged += new System.EventHandler(this.FunctionInput_ValueChanged);
            // 
            // FunctionLabel
            // 
            this.FunctionLabel.AutoSize = true;
            this.FunctionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.FunctionLabel.Location = new System.Drawing.Point(33, 52);
            this.FunctionLabel.Name = "FunctionLabel";
            this.FunctionLabel.Size = new System.Drawing.Size(108, 29);
            this.FunctionLabel.TabIndex = 7;
            this.FunctionLabel.Text = "functions";
            // 
            // BpsInput
            // 
            this.BpsInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.BpsInput.Location = new System.Drawing.Point(3, 3);
            this.BpsInput.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BpsInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.BpsInput.Name = "BpsInput";
            this.BpsInput.ReadOnly = true;
            this.BpsInput.Size = new System.Drawing.Size(24, 38);
            this.BpsInput.TabIndex = 6;
            this.BpsInput.ValueChanged += new System.EventHandler(this.BpsInput_ValueChanged);
            // 
            // QuickInstallBox
            // 
            this.QuickInstallBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.QuickInstallBox.Location = new System.Drawing.Point(259, 179);
            this.QuickInstallBox.Multiline = true;
            this.QuickInstallBox.Name = "QuickInstallBox";
            this.QuickInstallBox.Size = new System.Drawing.Size(438, 58);
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
            this.ClientSize = new System.Drawing.Size(709, 250);
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
            this.Load += new System.EventHandler(this.TheForm_Load);
            this.BpsPanel.ResumeLayout(false);
            this.BpsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FunctionInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BpsInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label FileLabel;
        private System.Windows.Forms.Label BpsLabel;
        private System.Windows.Forms.Panel BpsPanel;
        private System.Windows.Forms.TextBox QuickInstallBox;
        private System.Windows.Forms.Button CopyCommandButton;
        private System.Windows.Forms.NumericUpDown BpsInput;
        private System.Windows.Forms.NumericUpDown FunctionInput;
        private System.Windows.Forms.Label FunctionLabel;
    }
}

