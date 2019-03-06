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
            // use dot for decimal separator regardless of PC setting
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;
            InitializeComponent();
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

        // converts an NBS file to a bunch of functions and saves them to the specified path
        // "ticksperbeat" determines the tempo
        //      1 plays notes at the full 20 tps speed, 2 plays every other tick, 3 every third tick, etc.
        // "beatsperfunction" determines how many functions to generate
        //      1 generates one function per play tick, 2 generates one function per two play ticks, 3 every third, etc.
        //      larger numbers generate fewer functions but run more commands per tick
        // returns a command used to set up the blocks that play the song
        private static string SaveFunctions(NbsFile nbs, int ticksperbeat, int beatsperfunction, string functionspath)
        {
            string nspace = Path.GetFileNameWithoutExtension(functionspath);
            Directory.CreateDirectory(Path.Combine(functionspath, "functions"));

            // create the list of functions
            string[] functions = new string[(nbs.Noteblocks.Last().Tick / beatsperfunction) + 1];

            // add each block to the function that should play it
            foreach (var block in nbs.Noteblocks)
            {
                string play = $" as @a at @s run playsound {GetInstrumentName(block.Instrument)} record @s ~ ~ ~ 1 {GetPitchValue(block.Key).ToString()}\n";
                if (beatsperfunction > 1)
                    play.Insert(0, " if score #" + nspace + "2 nbsmusic matches " + (block.Tick % beatsperfunction));
                functions[block.Tick / beatsperfunction] += "execute" + play;
            }

            // point each function to the next one, and save each function
            for (int i = 0; i < functions.Length; i++)
            {
                if (beatsperfunction > 1)
                    functions[i] += "execute if score #" + nspace + "2 nbsmusic matches " + (beatsperfunction - 1) + " run ";
                functions[i] += "data merge block ~ ~ ~ {Command:\"function " + nspace + ":" + (i + 1) + "\"}";
                File.WriteAllText(Path.Combine(functionspath, "functions", i + ".mcfunction"), functions[i]);
            }

            // generate the command used for setup
            StringBuilder setup = new StringBuilder();
            if (beatsperfunction > 1 || ticksperbeat > 1)
                setup.AppendLine("scoreboard objectives add nbsmusic dummy");
            string runfunction = $"function {nspace}:0";
            string addnote = $"scoreboard players add #{nspace}2 nbsmusic 1";
            string resetnote = $"execute if score #{nspace}2 nbsmusic matches {beatsperfunction}.. run scoreboard players set #{nspace}2 nbsmusic 0";
            string addbeat = $"scoreboard players add #{nspace}1 nbsmusic 1";
            string resetbeat = $"execute if score #{nspace}1 nbsmusic matches {ticksperbeat}.. run scoreboard players set #{nspace}1 nbsmusic 0";

            if (ticksperbeat == 1)
            {
                // the function will play notes every tick
                if (beatsperfunction > 1)
                {
                    setup.AppendLine(PlaceCommandBlock(0, CommandBlockType.Repeating, addnote));
                    setup.AppendLine(PlaceCommandBlock(-1, CommandBlockType.Chain, resetnote));
                    setup.AppendLine(PlaceCommandBlock(-2, CommandBlockType.Chain, runfunction));
                }
                else
                    setup.AppendLine(PlaceCommandBlock(0, CommandBlockType.Repeating, runfunction));
            }
            else
            {
                // use a scoreboard to decide when to play notes
                setup.AppendLine(PlaceCommandBlock(0, CommandBlockType.Repeating, addbeat));
                setup.AppendLine(PlaceCommandBlock(-1, CommandBlockType.Chain, resetbeat));
                setup.AppendLine(PlaceCommandBlock(-2, CommandBlockType.ConditionalChain, runfunction));
                if (beatsperfunction > 1)
                {
                    setup.AppendLine(PlaceCommandBlock(-3, CommandBlockType.ConditionalChain, addnote));
                    setup.AppendLine(PlaceCommandBlock(-4, CommandBlockType.ConditionalChain, resetnote));
                }
            }
            setup.AppendLine("setblock ~ ~1 ~ lever[face=floor]");
            File.WriteAllText(Path.Combine(functionspath, "functions", "setup.mcfunction"), setup.ToString());
            return "/give @p command_block{BlockEntityTag:{auto:1b,Command:\"function " + nspace + ":setup\"}}";
        }

        private enum CommandBlockType
        {
            Repeating,
            Chain,
            ConditionalChain
        }
        private static string PlaceCommandBlock(int z, CommandBlockType cblock, string command)
        {
            string result = "setblock ~ ~ ~";
            if (z != 0)
                result += z;
            if (cblock == CommandBlockType.Repeating)
                result += " repeating_command_block{";
            else if (cblock == CommandBlockType.Chain)
                result += " chain_command_block{auto:1b,";
            else if (cblock == CommandBlockType.ConditionalChain)
                result += " chain_command_block[conditional=true]{auto:1b,";

            // logically we should escape the command, but in practice that isn't ever necessary here
            return result + "Command:\"" + command + "\"}";
        }

        // converts NBS saved instrument values to playsound string names
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

        // converts note block "note" property to playsound pitch values
        private static double GetPitchValue(byte input)
        {
            return Math.Pow(2, (((double)input) - 45) / 12);
        }

        private static int Clamp(int value, int min, int max)
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
        // if "vanilla" is set, this will throw an exception if it encounters custom notes or out-of-range pitches
        public NbsFile(string path, bool vanilla)
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
                    if (vanilla && !(inst >= 0 && inst <= 9))
                        throw new IndexOutOfRangeException($"Custom instrument detected with ID {inst} on note #{Noteblocks.Count + 1}");
                    byte key = br.ReadByte();
                    if (vanilla && !(key >= 33 && key <= 57))
                        throw new IndexOutOfRangeException($"Out-of-range pitch detected with ID {key} on note #{Noteblocks.Count + 1}");
                    Noteblocks.Add(new NoteBlock(tick, layer, inst, key));
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
