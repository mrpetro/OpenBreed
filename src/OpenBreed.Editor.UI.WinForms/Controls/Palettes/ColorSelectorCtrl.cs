using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Palettes;

namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    public partial class ColorSelectorCtrl : UserControl
    {
        private PaletteEditorExVM vm;

        private int m_ColorsInRow = 16;
        private int m_ColorsBtnSize = 16;

        public ColorSelectorCtrl()
        {
            InitializeComponent();

            //InitDefaultColors();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        public void Initialize(PaletteEditorExVM vm)
        {
            this.vm = vm;

            this.vm.PropertyChanged += vm_PropertyChanged;
        }

        private void vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(vm.CurrentColorIndex):
                    Invalidate();
                    break;
                case nameof(vm.CurrentColor):
                    Invalidate();
                    break;
                case nameof(vm.Colors):
                    Invalidate();
                    break;
                default:
                    break;
            }
        }

        private void DrawColorSelection(Graphics gfx, Color color, float x, float y, float width, float height)
        {
            Pen pen = new Pen(color);
            gfx.DrawRectangle(pen, x, y, width, height);
        }

        private void DrawColor(Color color, Graphics gfx, float x, float y, float width, float height)
        {
            Brush brush = new SolidBrush(color);
            Pen pen = new Pen(Color.White);
            gfx.FillRectangle(brush, x, y, width, height);
            gfx.DrawRectangle(pen, x, y, width, height);
        }

        public Point GetColorCoords(int index)
        {
            return new Point(index % m_ColorsInRow, index / m_ColorsInRow);
        }

        public Point GetSelectionXY(Point coordinates)
        {
            int btnSize = m_ColorsBtnSize;

            int x = coordinates.X / btnSize;
            int y = coordinates.Y / btnSize;

            return new Point(x, y);
        }

        public int GetPaletteIndex(int x, int y)
        {
            return x + y * m_ColorsInRow;
        }

        private void DrawPaletteGrid(Graphics gfx)
        {
            int colorsNo = vm.Colors.Count;
            int xMax = m_ColorsInRow;
            int yMax = colorsNo / xMax;
            int btnSize = m_ColorsBtnSize;

            for (int j = 0; j < yMax; j++)
            {
                for (int i = 0; i < xMax; i++)
                {
                    int colorNo = i + xMax * j;

                    if (colorNo >= colorsNo)
                        return;

                    Color color = vm.Colors[colorNo];
                    DrawColor(color, gfx, i * btnSize, j * btnSize, btnSize, btnSize);
                }
            }
        }

        void ColorSelectorCtrl_Paint(object sender, PaintEventArgs e)
        {
            if (vm == null)
                return;

            DrawPaletteGrid(e.Graphics);

            int btnSize = m_ColorsBtnSize;
            int selectedColorIndex = vm.CurrentColorIndex;

            if (selectedColorIndex != -1)
            {
                Point point = GetColorCoords(selectedColorIndex);
                DrawColorSelection(e.Graphics, Color.Black, point.X * btnSize, point.Y * btnSize, btnSize, btnSize);
                DrawColorSelection(e.Graphics, Color.White, point.X * btnSize - 1, point.Y * btnSize - 1, btnSize + 2, btnSize + 2);
            }
        }

        void PaletteView_Resize(object sender, EventArgs e)
        {
            m_ColorsInRow = this.Size.Width / m_ColorsBtnSize;

            Refresh();
        }

        void ColorSelectorCtrl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Point pos = GetSelectionXY(e.Location);

                int newSelectedColorIndex = GetPaletteIndex(pos.X, pos.Y);

                if (newSelectedColorIndex >= vm.Colors.Count)
                    return;

                vm.CurrentColorIndex = newSelectedColorIndex;
            }
        }
    }
}
