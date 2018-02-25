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
        private int TicksPerBeat = 1;
        private int BeatsPerFunction = 1;
        private NbsFile LoadedFile;
        public TheForm()
        {
            InitializeComponent();
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

        private void OpenButton_Click(object sender, EventArgs e)
        {
            var open = new OpenFileDialog();
            open.Filter = "NBS Files (*.nbs)|*nbs";
            open.Title = "Choose an NBS file";
            open.InitialDirectory = Properties.Settings.Default.OpenPath;
            if (open.ShowDialog() == DialogResult.OK)
            {
                LoadedFile = new NbsFile(open.FileName);
                SetTicksPerBeat((int)(20 / LoadedFile.Tempo));
                SetBeatsPerFunction(BeatsPerFunction);
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
                string setup = SaveFunctions(LoadedFile, TicksPerBeat, BeatsPerFunction, save.FileName);
                Properties.Settings.Default.ExportPath = Path.GetDirectoryName(save.FileName);
                CopyCommandButton.Enabled = true;
                QuickInstallBox.Visible = true;
                string nspace = Path.GetFileNameWithoutExtension(save.FileName);
                QuickInstallBox.Text = setup;
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
        // returns "setup" command
        private static string SaveFunctions(NbsFile nbs, int ticksperbeat, int beatsperfunction, string functionspath)
        {
            string nspace = Path.GetFileNameWithoutExtension(functionspath);
            Directory.CreateDirectory(Path.Combine(functionspath, "functions"));
            string[] functions = new string[(nbs.Noteblocks.Last().Tick / beatsperfunction) + 1];
            foreach (var block in nbs.Noteblocks)
            {
                string play = $" as @a at @s run playsound " +
                    $"{GetInstrumentName(block.Instrument)} record @s ~ ~ ~ 1 {GetPitchValue(block.Key)}\n";
                if (beatsperfunction > 1)
                    play = " if score #" + nspace + "2 nbsmusic matches " + (block.Tick % beatsperfunction) + play;
                functions[block.Tick / beatsperfunction] += "execute" + play;
            }
            for (int i = 0; i < functions.Length; i++)
            {
                if (beatsperfunction > 1)
                    functions[i] += "execute if score #" + nspace + "2 nbsmusic matches " + (beatsperfunction - 1) + " run ";
                functions[i] += "data merge block ~ ~ ~ {Command:\"function " + nspace + ":" + (i + 1) + "\"}";
                File.WriteAllText(Path.Combine(functionspath, "functions", i + ".mcfunction"), functions[i]);
            }
            StringBuilder setup = new StringBuilder();
            if (beatsperfunction > 1 || ticksperbeat > 1)
                setup.AppendLine("scoreboard objectives add nbsmusic dummy");
            if (ticksperbeat == 1)
            {
                if (beatsperfunction > 1)
                {
                    setup.AppendLine("setblock ~ ~ ~ repeating_command_block{Command:\"scoreboard players add #" + nspace + "2 nbsmusic 1\"}");
                    setup.AppendLine("setblock ~ ~ ~-1 chain_command_block[conditional=true]{auto:1b,Command:\"execute if score #" + nspace + "2 nbsmusic matches " + beatsperfunction + ".. run scoreboard players set #" + nspace + "2 nbsmusic 0\"");
                    setup.AppendLine("setblock ~ ~ ~-2 chain_command_block{auto:1b,Command:\"function " + nspace + ":0\"}");
                }
                else
                    setup.AppendLine("setblock ~ ~ ~ repeating_command_block{Command:\"function " + nspace + ":0\"}");
            }
            else
            {
                setup.AppendLine("setblock ~ ~ ~ repeating_command_block{Command:\"scoreboard players add #" + nspace + "1 nbsmusic 1\"}");
                setup.AppendLine("setblock ~ ~ ~-1 chain_command_block{auto:1b,Command:\"execute if score #" + nspace + "1 nbsmusic matches " + ticksperbeat + ".. run scoreboard players set #" + nspace + "1 nbsmusic 0\"}");
                setup.AppendLine("setblock ~ ~ ~-2 chain_command_block[conditional=true]{auto:1b,Command:\"function " + nspace + ":0\"}");
                if (beatsperfunction > 1)
                {
                    setup.AppendLine("setblock ~ ~ ~-3 chain_command_block[conditional=true]{auto:1b,Command:\"scoreboard players add #" + nspace + "2 nbsmusic 1\"}");
                    setup.AppendLine("setblock ~ ~ ~-4 chain_command_block[conditional=true]{auto:1b,Command:\"execute if score #" + nspace + "2 nbsmusic matches " + beatsperfunction + ".. run scoreboard players set #" + nspace + "2 nbsmusic 0\"}");
                }
            }
            setup.AppendLine("setblock ~ ~1 ~ lever[face=floor]");
            File.WriteAllText(Path.Combine(functionspath, "functions", "setup.mcfunction"), setup.ToString());
            return "/give @p command_block{BlockEntityTag:{auto:1b,Command:\"function " + nspace + ":setup\"}}";
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

        private void CopyCommandButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(QuickInstallBox.Text);
        }

        private void BpsInput_ValueChanged(object sender, EventArgs e)
        {
            if (BpsInput.Value < 0)
            {
                SetTicksPerBeat(Math.Min(TicksPerBeat + 1, 100));
                BpsInput.Value = 0;
            }
            if (BpsInput.Value > 0)
            {
                SetTicksPerBeat(Math.Max(TicksPerBeat - 1, 1));
                BpsInput.Value = 0;
            }
        }

        private void FunctionInput_ValueChanged(object sender, EventArgs e)
        {
            if (FunctionInput.Value < 0)
            {
                SetBeatsPerFunction(Math.Max(BeatsPerFunction - 1, 1));
                FunctionInput.Value = 0;
            }
            if (FunctionInput.Value > 0)
            {
                SetBeatsPerFunction(Math.Min(BeatsPerFunction + 1, 100));
                FunctionInput.Value = 0;
            }
        }

        private void TheForm_Load(object sender, EventArgs e)
        {
            SetBeatsPerFunction(1);
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
