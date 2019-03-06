using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NBSDisc
{
    public class NoteBlock
    {
        public short Tick { get; }
        public short Layer { get; }
        public byte Instrument { get; }
        public byte Key { get; }

        public NoteBlock(short tick, short layer, byte inst, byte key)
        {
            Tick = tick;
            Layer = layer;
            Instrument = inst;
            Key = key;
        }

        // converts note block "note" property to playsound pitch values
        public double GetPitchValue()
        {
            return Math.Pow(2, (((double)Key) - 45) / 12);
        }

        // converts NBS saved instrument values to playsound string names
        public string GetInstrumentName()
        {
            switch (Instrument)
            {
                case 0:
                    return "block.note_block.harp";
                case 1:
                    return "block.note_block.bass";
                case 2:
                    return "block.note_block.basedrum";
                case 3:
                    return "block.note_block.snare";
                case 4:
                    return "block.note_block.hat";
                case 5:
                    return "block.note_block.guitar";
                case 6:
                    return "block.note_block.flute";
                case 7:
                    return "block.note_block.bell";
                case 8:
                    return "block.note_block.chime";
                case 9:
                    return "block.note_block.xylophone";
                case 10:
                    return "block.note_block.iron_xylophone";
                case 11:
                    return "block.note_block.cow_bell";
                case 12:
                    return "block.note_block.didgeridoo";
                case 13:
                    return "block.note_block.bit";
                case 14:
                    return "block.note_block.banjo";
                case 15:
                    return "block.note_block.pling";
                default:
                    throw new InvalidOperationException($"No such instrument ID {Instrument}");
            }
        }
    }

    public class NbsFile
    {
        public short Length { get; }
        public short Layers { get; }
        public string Name { get; }
        public string Author { get; }
        public string OriginalAuthor { get; }
        public string Description { get; }
        public double Tempo { get; }
        public List<NoteBlock> Noteblocks = new List<NoteBlock>();
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
                    if (vanilla && !(inst >= 0 && inst <= 15))
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

    public class DatapackExporter
    {
        // converts an NBS file to a bunch of functions and saves them to the specified path
        // "ticksperbeat" determines the tempo
        //      1 plays notes at the full 20 tps speed, 2 plays every other tick, 3 every third tick, etc.
        // "beatsperfunction" determines how many functions to generate
        //      1 generates one function per play tick, 2 generates one function per two play ticks, 3 every third, etc.
        //      larger numbers generate fewer functions but run more commands per tick
        // returns a command used to set up the blocks that play the song
        public static string SaveFunctions(NbsFile nbs, int ticksperbeat, int beatsperfunction, string functionspath)
        {
            string nspace = Path.GetFileNameWithoutExtension(functionspath);
            Directory.CreateDirectory(Path.Combine(functionspath, "functions"));

            // create the list of functions
            string[] functions = new string[(nbs.Noteblocks.Last().Tick / beatsperfunction) + 1];

            // add each block to the function that should play it
            foreach (var block in nbs.Noteblocks)
            {
                string play = $" as @a at @s run playsound {block.GetInstrumentName()} record @s ~ ~ ~ 1 {block.GetPitchValue().ToString()}\n";
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
    }
}
