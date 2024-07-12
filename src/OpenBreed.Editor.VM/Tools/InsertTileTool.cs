//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using OpenBreed.Editor.UI.WinForms.Presentations.Maps.Helpers;
//using OpenBreed.Editor.VM.Tools;
//using System.Windows.Forms;

//namespace OpenBreed.Editor.VM.Tools
//{
//    public class InsertTileTool : LevelTool
//    {
//        private readonly MapBodyPresentationModel m_Model = null;

//        public InsertTileTool(MapBodyPresentationModel model, IToolController controller) :
//            base("InsertTileTool", controller)
//        {
//            if (model == null)
//                throw new ArgumentNullException("model");

//            m_Model = model;
//        }

//        public override void Activate()
//        {
//            Controller.KeyDown += new KeyEventHandler(inserter_KeyDown);
//            Controller.KeyUp += new KeyEventHandler(inserter_KeyUp);
//            Controller.MouseDown += new MouseEventHandler(inserter_MouseDown);
//            Controller.MouseUp += new MouseEventHandler(inserter_MouseUp);
//            Controller.MouseMove += new MouseEventHandler(inserter_MouseMove);
//            Controller.Paint += new PaintEventHandler(inserter_Paint);
//        }

//        public override void Deactivate()
//        {
//            Controller.KeyDown -= new KeyEventHandler(inserter_KeyDown);
//            Controller.KeyUp -= new KeyEventHandler(inserter_KeyUp);
//            Controller.MouseDown -= new MouseEventHandler(inserter_MouseDown);
//            Controller.MouseUp -= new MouseEventHandler(inserter_MouseUp);
//            Controller.MouseMove -= new MouseEventHandler(inserter_MouseMove);
//            Controller.Paint -= new PaintEventHandler(inserter_Paint);
//        }

//        void inserter_MouseMove(object sender, MouseEventArgs e)
//        {
//            IToolController view = (IToolController)sender;

//            //if (m_Model.TilesInserter.UpdateCursor(e.Location))
//            //{
//            //    m_Model.TilesInserter.UpdateInserting();
//            //    view.Invalidate();
//            //}
//        }

//        void inserter_MouseDown(object sender, MouseEventArgs e)
//        {
//            if (m_Model.TilesInserter.Mode != InsertModeEnum.Nothing)
//                return;

//            IToolController view = (IToolController)sender;

//            if (e.Button == System.Windows.Forms.MouseButtons.Left)
//            {
//                m_Model.TilesInserter.StartInserting(InsertModeEnum.Point);
//            }
//        }

//        void inserter_MouseUp(object sender, MouseEventArgs e)
//        {
//            if (m_Model.TilesInserter.Mode == InsertModeEnum.Nothing)
//                return;

//            IToolController view = (IToolController)sender;

//            if (e.Button == System.Windows.Forms.MouseButtons.Left)
//            {
//                m_Model.TilesInserter.FinishInserting();
//            }
//        }

//        void inserter_KeyUp(object sender, KeyEventArgs e)
//        {
//            IToolController view = (IToolController)sender;
//        }

//        void inserter_KeyDown(object sender, KeyEventArgs e)
//        {
//            IToolController view = (IToolController)sender;
//        }

//        void inserter_Paint(object sender, PaintEventArgs e)
//        {
//            int tileSize = m_Model.TilesInserter.Model.MapPMod.TileSize;

//            m_Model.TilesInserter.DrawInsertion(e.Graphics, tileSize);
//        }
//    }
//}
