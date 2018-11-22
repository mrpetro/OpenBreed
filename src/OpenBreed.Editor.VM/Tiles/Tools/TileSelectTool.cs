//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;
//using System.Drawing;
//using OpenBreed.Editor.VM.Tiles;
//using OpenBreed.Editor.VM.Tiles.Helpers;
//using OpenBreed.Editor.VM;

//namespace OpenBreed.Editor.VM.Tiles.Tools
//{
//    public class TileSelectTool
//    {
//        private readonly TilesSelector m_Selector;

//        public TileSelectTool(TilesSelector selector)
//        {
//            if (selector == null)
//                throw new InvalidOperationException("Selector is null!");

//            m_Selector = selector;
//        }

//        public void RegisterView(IToolController toolView)
//        {
//            toolView.KeyDown +=new KeyEventHandler(selector_KeyDown);
//            toolView.KeyUp += new KeyEventHandler(selector_KeyUp);
//            toolView.MouseDown += new MouseEventHandler(selector_MouseDown);
//            toolView.MouseUp += new MouseEventHandler(selector_MouseUp);
//            toolView.MouseMove += new MouseEventHandler(selector_MouseMove);
//            toolView.Paint += new PaintEventHandler(selector_Paint);
//        }

//        public void UnregisterView(IToolController toolView)
//        {
//            toolView.KeyDown -= new KeyEventHandler(selector_KeyDown);
//            toolView.KeyUp -= new KeyEventHandler(selector_KeyUp);
//            toolView.MouseDown -= new MouseEventHandler(selector_MouseDown);
//            toolView.MouseUp -= new MouseEventHandler(selector_MouseUp);
//            toolView.MouseMove -= new MouseEventHandler(selector_MouseMove);
//            toolView.Paint -= new PaintEventHandler(selector_Paint);
//        }

//        void selector_MouseMove(object sender, MouseEventArgs e)
//        {
//            IToolController view = (IToolController)sender;

//            if (m_Selector.Mode != SelectModeEnum.Nothing)
//            {
//                m_Selector.UpdateSelection(e.Location);
//                view.Invalidate();
//            }
//        }

//        void selector_MouseDown(object sender, MouseEventArgs e)
//        {
//            if (m_Selector.Mode != SelectModeEnum.Nothing)
//                return;

//            IToolController view = (IToolController)sender;

//            if (e.Button == System.Windows.Forms.MouseButtons.Left)
//                m_Selector.StartSelection(SelectModeEnum.Select, e.Location);
//            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
//                m_Selector.StartSelection(SelectModeEnum.Deselect, e.Location);

//            view.Invalidate();
//        }

//        void selector_MouseUp(object sender, MouseEventArgs e)
//        {
//            if (m_Selector.Mode == SelectModeEnum.Nothing)
//                return;

//            IToolController view = (IToolController)sender;

//            m_Selector.FinishSelection(e.Location);
//            view.Invalidate();
//        }

//        void selector_KeyUp(object sender, KeyEventArgs e)
//        {
//            if (!e.Control)
//                m_Selector.MultiSelect = false;
//        }

//        void selector_KeyDown(object sender, KeyEventArgs e)
//        {
//            if (e.Control)
//                m_Selector.MultiSelect = true;
//        }

//        void selector_Paint(object sender, PaintEventArgs e)
//        {
//            m_Selector.DrawSelection(e.Graphics);
//        }
//    }
//}
