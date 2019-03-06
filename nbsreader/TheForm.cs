using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Globalization;

namespace NBSDisc
{
    public partial class TheForm : Form
    {
        private int TicksPerBeat = 1;
        private int BeatsPerFunction = 1;
        private NbsFile LoadedFile;
        public TheForm()
        {
            InitializeComponent();
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }


        

        private void OpenButton_Click(object sender, EventArgs e)
        {
            var open = new OpenFileDialog();
            open.Filter = "NBS Files (*.nbs)|*nbs";
            open.Title = "Choose an NBS file";
            open.InitialDirectory = Properties.Settings.Default.OpenPath;
            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    LoadedFile = new NbsFile(open.FileName, true);
                }
                catch (IndexOutOfRangeException ex)
                {
                    MessageBox.Show("That NBS file is not compatible with vanilla because of one or more of its notes.\n\n"+ex.Message,"Bad NBS file!");
                    return;
                }
                SetTicksPerBeat((int)(20 / LoadedFile.Tempo));
                SetBeatsPerFunction(BeatsPerFunction);
                SaveButton.Enabled = true;
                BpsPanel.Visible = true;
                FileLabel.Text = Path.GetFileNameWithoutExtension(open.FileName);
                Properties.Settings.Default.OpenPath = Path.GetDirectoryName(open.FileName);
            }
        }

        // TO DO: save depending on where where the user is
        // * (random folder)
        // * (world)
        // * /datapacks
        // * /(some pack)
        // * /data
        // * /(namespace)
        // * /functions
        // * /further folders...
        private void SaveButton_Click(object sender, EventArgs e)
        {
            var save = new SaveFileDialog();
            save.Filter = "Namespace folder|*";
            save.Title = "Save your functions in a namespace";
            save.InitialDirectory = Properties.Settings.Default.ExportPath;
            if (save.ShowDialog() == DialogResult.OK)
            {
                string setup = DatapackExporter.SaveFunctions(LoadedFile, TicksPerBeat, BeatsPerFunction, save.FileName);
                Properties.Settings.Default.ExportPath = Path.GetDirectoryName(save.FileName);
                CopyCommandButton.Enabled = true;
                QuickInstallBox.Visible = true;
                string nspace = Path.GetFileNameWithoutExtension(save.FileName);
                QuickInstallBox.Text = setup;
            }
        }

        private void TheForm_Load(object sender, EventArgs e)
        {
            SetBeatsPerFunction(1);
        }

        private void TheForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void CopyCommandButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(QuickInstallBox.Text);
        }

        private void BpsInput_ValueChanged(object sender, EventArgs e)
        {
            if (BpsInput.Value != 0)
            {
                SetTicksPerBeat(Clamp(TicksPerBeat - Math.Sign(BpsInput.Value), 1, 100));
                BpsInput.Value = 0;
            }
        }

        private void FunctionInput_ValueChanged(object sender, EventArgs e)
        {
            if (FunctionInput.Value != 0)
            {
                SetBeatsPerFunction(Clamp(BeatsPerFunction + Math.Sign(FunctionInput.Value), 1, 100));
                FunctionInput.Value = 0;
            }
        }

        private void SetTicksPerBeat(int value)
        {
            TicksPerBeat = value;
            BpsLabel.Text = Math.Round(1200d / TicksPerBeat, 1) + " BPM";
        }

        private void SetBeatsPerFunction(int value)
        {
            BeatsPerFunction = value;
            if (LoadedFile == null)
                FunctionLabel.Text = "";
            else
                FunctionLabel.Text = "×" + BeatsPerFunction + " runtime, " + (LoadedFile.Noteblocks.Last().Tick / BeatsPerFunction) + " functions";
        }
    }

}
