using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace NBSDisc
{
    public partial class TheForm : Form
    {
        private NbsFile LoadedFile;
        public TheForm()
        {
            InitializeComponent();
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            var open = new OpenFileDialog();
            open.Filter = "NBS Files (*.nbs)|*nbs";
            open.Title = "Choose an NBS file";
            open.InitialDirectory = Properties.Settings.Default.OpenPath;
            if (open.ShowDialog() == DialogResult.OK)
            {
                LoadedFile = new NbsFile(open.FileName);
                TempoInput.Value = (int)(20 / LoadedFile.Tempo);
                SaveButton.Enabled = true;
                BpsPanel.Visible = true;
                FileLabel.Text = Path.GetFileNameWithoutExtension(open.FileName);
                Properties.Settings.Default.OpenPath = Path.GetDirectoryName(open.FileName);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var save = new SaveFileDialog();
            save.Filter = "Namespace folder|*";
            save.Title = "Save your functions in a namespace";
            save.InitialDirectory = Properties.Settings.Default.ExportPath;
            if (save.ShowDialog() == DialogResult.OK)
            {
                SaveFunctions(LoadedFile, (int)TempoInput.Value, save.FileName);
                Properties.Settings.Default.ExportPath = Path.GetDirectoryName(save.FileName);
                CopyCommandButton.Enabled = true;
                QuickInstallBox.Visible = true;
                string nspace = Path.GetFileNameWithoutExtension(save.FileName);
                QuickInstallBox.Text = "/give @p command_block{BlockEntityTag:{auto:1b,Command:\"function " + nspace + ":setup\"}}";
            }
        }

        private static void SaveFunctions(NbsFile nbs, int tempoticks, string functionspath)
        {
            string nspace = Path.GetFileNameWithoutExtension(functionspath);
            Directory.CreateDirectory(Path.Combine(functionspath, "functions"));
            string[] functions = new string[nbs.Noteblocks.Last().Tick + 1];
            foreach (var block in nbs.Noteblocks)
            {
                functions[block.Tick] += $"execute as @a at @s run playsound {GetInstrumentName(block.Instrument)} record @s ~ ~ ~ 1 {GetPitchValue(block.Key)}\n";
            }
            for (int i = 0; i < functions.Length; i++)
            {
                functions[i] += "data merge block ~ ~ ~ {Command:\"function " + nspace + ":" + (i + 1) + "\"}";
                File.WriteAllText(Path.Combine(functionspath, "functions", i + ".mcfunction"), functions[i]);
            }
            string setup;
            if (tempoticks == 1)
                setup = "setblock ~ ~ ~ repeating_command_block{Command:\"function " + nspace + ":0\"}";
            else
                setup = "fill ~ ~ ~ ~ ~ ~ air replace command_block{Command:\"function "+nspace+":setup\"}\nscoreboard objectives add nbsmusic dummy\nsetblock ~ ~ ~-1 repeating_command_block{Command:\"scoreboard players add #" + nspace + " nbsmusic 1\"}\nsetblock ~ ~ ~-2 chain_command_block{auto:1b,Command:\"execute if score #" + nspace + " nbsmusic matches " + (tempoticks+1) + ".. run scoreboard players set #" + nspace + " nbsmusic 1\"}\nsetblock ~ ~ ~-3 chain_command_block[conditional=true]{auto:1b,Command:\"function " + nspace + ":0\"}";
            File.WriteAllText(Path.Combine(functionspath, "functions", "setup.mcfunction"), setup);
        }

        private static string GetInstrumentName(byte input)
        {
            switch (input)
            {
                case 0:
                    return "block.note.harp";
                case 1:
                    return "block.note.bass";
                case 2:
                    return "block.note.basedrum";
                case 3:
                    return "block.note.snare";
                case 4:
                    return "block.note.hat";
                case 5:
                    return "block.note.guitar";
                case 6:
                    return "block.note.flute";
                case 7:
                    return "block.note.bell";
                case 8:
                    return "block.note.chime";
                case 9:
                    return "block.note.xylophone";
                default:
                    return "";
            }
        }

        private static double GetPitchValue(byte input)
        {
            return Math.Pow(2, (((double)input) - 45) / 12);
        }

        private void TheForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void TempoInput_ValueChanged(object sender, EventArgs e)
        {
            BpsLabel.Text = "= " + Math.Round(20 / TempoInput.Value, 1) + " bps";
        }

        private void CopyCommandButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(QuickInstallBox.Text);
        }
    }

    public class NoteBlock
    {
        public short Tick;
        public short Layer;
        public byte Instrument;
        public byte Key;

        public NoteBlock(short tick, short layer, byte inst, byte key)
        {
            Tick = tick;
            Layer = layer;
            Instrument = inst;
            Key = key;
        }
    }

    public class NbsFile
    {
        public readonly short Length;
        public readonly short Layers;
        public readonly string Name;
        public readonly string Author;
        public readonly string OriginalAuthor;
        public readonly string Description;
        public readonly double Tempo;
        public readonly List<NoteBlock> Noteblocks = new List<NoteBlock>();
        public NbsFile(string path)
        {
            FileStream fs = File.OpenRead(path);
            BinaryReader br = new BinaryReader(fs);
            Length = br.ReadInt16();
            Layers = br.ReadInt16();
            Name = ReadString(br);
            Author = ReadString(br);
            OriginalAuthor = ReadString(br);
            Description = ReadString(br);
            Tempo = (double)br.ReadInt16() / 100;

            // skip irrelevant info
            br.ReadByte();
            br.ReadByte();
            br.ReadByte();
            br.ReadInt32();
            br.ReadInt32();
            br.ReadInt32();
            br.ReadInt32();
            br.ReadInt32();
            ReadString(br);

            short tick = -1;
            short jumps = 0;
            while (true)
            {
                jumps = br.ReadInt16();
                if (jumps == 0)
                    break;
                tick += jumps;
                short layer = -1;
                while (true)
                {
                    jumps = br.ReadInt16();
                    if (jumps == 0)
                        break;
                    layer += jumps;
                    byte inst = br.ReadByte();
                    byte key = br.ReadByte();
                    Noteblocks.Add(new NoteBlock(tick, layer, inst, key)); //Found note block, add to the list
                }
            }
            br.Dispose();
            fs.Dispose();
        }

        private static string ReadString(BinaryReader br)
        {
            int len = br.ReadInt32();
            string str = "";
            for (int i = 0; i < len; i++)
                str += (char)br.ReadByte();
            return str;
        }
    }
}
