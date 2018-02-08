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
        public TheForm()
        {
            InitializeComponent();
            OutputBox.SetFont(new System.Drawing.Font("Consolas", 14));
        }
        private void OpenButton_Click(object sender, EventArgs e)
        {
            DialogResult res = OpenDialog.ShowDialog();
            if (res != DialogResult.OK) return;

            FileStream fs = File.OpenRead(OpenDialog.FileName);
            BinaryReader br = new BinaryReader(fs);

            // skip past a bunch of crap
            br.ReadInt16();
            br.ReadInt16();
            ReadString(br);
            ReadString(br);
            ReadString(br);
            ReadString(br);
            double tempo = (double)br.ReadInt16() / 100;
            br.ReadByte();
            br.ReadByte();
            br.ReadByte();
            br.ReadInt32();
            br.ReadInt32();
            br.ReadInt32();
            br.ReadInt32();
            br.ReadInt32();
            ReadString(br);

            List<NoteBlock> noteblocks = new List<NoteBlock>();
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
                    noteblocks.Add(new NoteBlock(tick, layer, inst, key)); //Found note block, add to the list
                }
            }
            br.Dispose();
            fs.Dispose();

            OutputBox.Text = "";
            StringBuilder command = new StringBuilder("summon item ~ ~1 ~ {Item:{id:record_strad,Count:1b,tag:{HideFlags:32,display:{LocName:\"Custom Song\"},tryashtar-song:{n1:{p:{b1:1b},w:{" + BitTag((int)(noteblocks[0].Tick * 20 / tempo)) + "}},");
            for (int i = 0; i < noteblocks.Count; i++)
            {
                command.Append("n" + (i + 2) + ":{i:" + InstrumentConvert(noteblocks[i].Instrument) + "b,p:{");
                command.Append(BitTag(noteblocks[i].Key - 32));
                command.Append("}");
                int wait = 0;
                if (i < noteblocks.Count - 1)
                    wait = (int)((noteblocks[i + 1].Tick - noteblocks[i].Tick) * 20 / tempo);
                if (wait > 0)
                {
                    command.Append(",w:{");
                    command.Append(BitTag(wait));
                    command.Append("}");
                }
                command.Append("},");
                if (SplitCheck.Checked && command.Length > 32000)
                {
                    command.Append("}}}}");
                    OutputBox.Text += command.ToString().Replace(",}", "}") + "\n";
                    command.Clear();
                    command.Append("entitydata @e[type=item,c=1] {Item:{tag:{tryashtar-song:{");
                }
            }
            command.Append("}}}}");
            OutputBox.Text += command.ToString().Replace(",}", "}") + "\n";
            OutputBox.Enabled = true;
            ClickLabel.Visible = true;
            OutputBox.WrapMode = SplitCheck.Checked && OutputBox.Lines.Count > 2 ? ScintillaNET.WrapMode.None : ScintillaNET.WrapMode.Char;
        }

        private string BitTag(int input)
        {
            string output = "";
            var array = Convert.ToString(input, 2).Select(s => s.Equals('1')).ToArray();
            int index = 1;
            for (int i = array.Length - 1; i >= 0; i--)
            {
                if (array[i])
                    output += "b" + index + ":1b,";
                index++;
            }
            return output;
        }

        private byte InstrumentConvert(byte input)
        {
            switch (input)
            {
                case 0:
                    return 9;
                case 1:
                    return 0;
                case 2:
                    return 3;
                case 3:
                    return 1;
                case 4:
                    return 2;
                case 5:
                    return 7;
                case 6:
                    return 5;
                case 7:
                    return 4;
                case 8:
                    return 6;
                case 9:
                    return 8;
                default:
                    return 255;
            }
        }

        private string ReadString(BinaryReader br)
        {
            int len = br.ReadInt32();
            string str = "";
            for (int i = 0; i < len; i++)
                str += (char)br.ReadByte();
            return str;
        }
    }
}
