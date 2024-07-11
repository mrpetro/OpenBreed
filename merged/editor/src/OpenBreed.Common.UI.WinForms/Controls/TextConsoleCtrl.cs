using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections;

namespace OpenBreed.Common.UI.WinForms.Controls
{
    public partial class TextConsoleCtrl : UserControl
    {
        private Queue<string> m_TextQueue = null;
        private Font m_DrawFont = null;
        private Point m_CarretPos;
        private int m_MaxTextQueueSize = 1;
        private Bitmap m_BitmapBuffer = null;
        private Graphics m_Gfx = null;

        #region Events

        public delegate void TextAdd(string text);

        /// <summary>
        /// Rised when new message incomming
        /// </summary>
        public event TextAdd TextAdded;

        #endregion

        #region Properties
        public Color TextColor { get; set; }

        #endregion

        public TextConsoleCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            //LogMan.Instance.MessageAdded += new LogMan.Message(LogMan_MessageAdded);

            m_TextQueue = new Queue<string>();
            m_DrawFont = new Font( "Arial", 8);
            TextColor = Color.Red;
        }

        #region Public methods

        /// <summary>
        /// Log debug message
        /// </summary>
        /// <param name="msg"></param>
        public void AppendText(string text)
        {
            m_TextQueue.Enqueue(text);

            if (m_TextQueue.Count > m_MaxTextQueueSize)
                m_TextQueue.Dequeue();

            var ev = TextAdded;
            if (ev != null)
                ev(text);

            Refresh();
        }

        #endregion

        private void TextConsoleCtrl_Paint(object sender, PaintEventArgs e)
        {
            Size proposedSize = new Size(this.ClientRectangle.Width, this.ClientRectangle.Height);

            TextFormatFlags flags = TextFormatFlags.WordBreak;

            foreach (string text in m_TextQueue)
            {
                Size textSize = TextRenderer.MeasureText(text, m_DrawFont, proposedSize, flags);
                Rectangle rectangle = new Rectangle(m_CarretPos, textSize);
                TextRenderer.DrawText(m_Gfx, text, m_DrawFont, rectangle, TextColor, flags);

                if (m_CarretPos.Y < this.ClientRectangle.Height)
                    m_CarretPos.Y += textSize.Height;
                else
                {
                    Rectangle r1 = new Rectangle(0, textSize.Height, this.ClientRectangle.Width, this.ClientRectangle.Height - textSize.Height);
                    m_Gfx.DrawImage(m_BitmapBuffer, 0, 0, r1, GraphicsUnit.Pixel);
                }
            }

            e.Graphics.DrawImage(m_BitmapBuffer, 0, 0, m_BitmapBuffer.Width, m_BitmapBuffer.Height);
        }

        private void TextConsoleCtrl_Load(object sender, EventArgs e)
        {
            m_BitmapBuffer = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height, PixelFormat.Format24bppRgb);
            m_Gfx = Graphics.FromImage(m_BitmapBuffer);
            m_Gfx.Clear(this.BackColor);
        }

        private void TextConsoleCtrl_Resize(object sender, EventArgs e)
        {
            m_BitmapBuffer = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height, PixelFormat.Format24bppRgb);
            m_Gfx = Graphics.FromImage(m_BitmapBuffer);
            m_Gfx.Clear(this.BackColor);
        }
    }
}
