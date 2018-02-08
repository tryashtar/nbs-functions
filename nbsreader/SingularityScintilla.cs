using ScintillaNET;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NBSDisc
{
    public enum ResizeMode
    {
        Width,
        Height,
        Both,
        None
    }

    // stops double-click from opening the form designer ;)
    [System.ComponentModel.DesignerCategory("")]
    public class SingularityScintilla : Scintilla
    {
        protected bool InternalMultiline = true;
        public bool Multiline
        {
            get { return InternalMultiline; }
            set
            {
                InternalMultiline = value;
                HScrollBar = value;
            }
        }
        public bool IgnoreHoverScroll = false;
        public char[] DisallowedChars;
        private ResizeMode InternalResizeMode = ResizeMode.None;
        public ResizeMode AutoResize
        {
            get { return InternalResizeMode; }
            set
            {
                InternalResizeMode = value;
                UpdateBoxDimensions();
            }
        }
        private string InternalWatermark = "";
        public string Watermark
        {
            get { return InternalWatermark; }
            set
            {
                InternalWatermark = value;
                if (TextLength == 0)
                    UpdateBoxDimensions();
            }
        }

        public void SetFont(Font font)
        {
            Styles[Style.Default].Font = font.Name;
            Styles[Style.Default].Size = (int)font.Size;
            UpdateBoxDimensions();
        }

        public Font GetFont()
        {
            return new Font(Styles[Style.Default].Font, Styles[Style.Default].Size);
        }

        public SingularityScintilla()
        {
            SetSelectionBackColor(true, Color.FromArgb(165, 184, 209));
            EmptyUndoBuffer();
            ScrollWidth = 1;
            Margins[0].Width = 0;
            Margins[1].Width = 0;
            Margins[2].Width = 2;
            VirtualSpaceOptions = VirtualSpace.RectangularSelection;
            AdditionalSelectionTyping = true;
            MultiPaste = MultiPaste.Each;
            BorderStyle = BorderStyle.FixedSingle;
            Styles[Style.LineNumber].BackColor = Color.FromArgb(180, 180, 180);
            Styles[Style.LineNumber].ForeColor = Color.FromArgb(50, 50, 50);
            Margins[2].Type = MarginType.Color;
            Margins[2].BackColor = Color.White;
            this.KeyPress += SingularityScintilla_KeyPress;
            this.InsertCheck += SingularityScintilla_InsertCheck;
            this.Insert += SingularityScintilla_Insert;
            this.Delete += SingularityScintilla_Delete;
            this.LostFocus += SingularityScintilla_LostFocus;
            this.ZoomChanged += SingularityScintilla_ZoomChanged;
        }

        private void SingularityScintilla_ZoomChanged(object sender, EventArgs e)
        {
            if (this.Zoom != 0 && !Multiline)
                this.Zoom = 0;
        }

        private void SingularityScintilla_LostFocus(object sender, EventArgs e)
        {
            // SelectionEnd = SelectionStart;
        }

        private void SingularityScintilla_Insert(object sender, ModificationEventArgs e)
        {
            UpdateBoxDimensions();
        }

        private void SingularityScintilla_Delete(object sender, ModificationEventArgs e)
        {
            UpdateBoxDimensions();
        }

        private void UpdateBoxDimensions()
        {
            if (AutoResize == ResizeMode.None)
                return;
            string usetext = Text;
            if (usetext.Length == 0)
                usetext = Watermark;
            if (usetext.Length == 0)
                usetext = " ";
            Size size = TextRenderer.MeasureText(usetext, this.GetFont());
            const int wbuffer = 15;
            if (AutoResize == ResizeMode.Width || AutoResize == ResizeMode.Both)
                Width = Math.Max(50 + wbuffer, size.Width + wbuffer);
            if (AutoResize == ResizeMode.Height || AutoResize == ResizeMode.Both)
                Height = size.Height + 5;
            // without this, the scrollbar seems to reappear annoyingly
            ScrollWidth = 2;
        }

        private void SingularityScintilla_InsertCheck(object sender, InsertCheckEventArgs e)
        {
            if (!Multiline)
                e.Text = Regex.Replace(e.Text, @"\r\n?|\n", " ");
            if (DisallowedChars != null)
            {
                foreach (char ch in DisallowedChars)
                {
                    e.Text = e.Text.Replace(ch.ToString(), "");
                }
            }
        }

        // stops weird control characters from appearing with certain shortcuts
        private void SingularityScintilla_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < 32)
            {
                e.Handled = true;
                return;
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            UpdateBoxDimensions();
            base.OnFontChanged(e);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x20a && IgnoreHoverScroll && !Focused)
                SendMessage(this.Parent.Handle, m.Msg, m.WParam, m.LParam);
            else
                base.WndProc(ref m);
            if (m.Msg == 0xf)
            {
                if (string.IsNullOrEmpty(this.Text)
                    && !string.IsNullOrEmpty(Watermark))
                {
                    using (var g = this.CreateGraphics())
                    {
                        Rectangle rect = this.ClientRectangle;
                        rect.Inflate(-1, 0);
                        TextRenderer.DrawText(g, Watermark, this.GetFont(),
                             rect, Color.LightGray, Styles[Style.Default].BackColor,
                             TextFormatFlags.Top | TextFormatFlags.Left);
                    }
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!Multiline && keyData == Keys.Enter)
            {
                OnKeyDown(new KeyEventArgs(Keys.Enter));
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift))
            {
                Parent.SelectNextControl(this, false, true, true, true);
                return true;
            }
            if (keyData == Keys.Tab)
            {
                Parent.SelectNextControl(this, true, true, true, true);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
