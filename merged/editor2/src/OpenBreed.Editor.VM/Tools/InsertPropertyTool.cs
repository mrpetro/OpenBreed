//using OpenBreed.Editor.VM.Levels;
//using OpenBreed.Editor.VM.Levels.Helpers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;

//namespace OpenBreed.Editor.VM.Tools
//{
//    public class InsertPropertyTool : LevelTool
//    {
//        private readonly MapBodyVM m_Model = null;

//        public InsertPropertyTool(MapBodyVM model, IToolController controller) :
//            base("InsertPropertyTool", controller)
//        {
//            if (model == null)
//                throw new ArgumentNullException("model");

//            m_Model = model;
//        }

//        public override void Activate()
//        {
//            Controller.KeyDown += new KeyEventHandler(Controller_KeyDown);
//            Controller.KeyUp += new KeyEventHandler(Controller_KeyUp);
//            Controller.MouseDown += new MouseEventHandler(Controller_MouseDown);
//            Controller.MouseUp += new MouseEventHandler(Controller_MouseUp);
//            Controller.MouseMove += new MouseEventHandler(Controller_MouseMove);
//            Controller.Paint -= new PaintEventHandler(Controller_Paint);
//        }

//        public override void Deactivate()
//        {
//            Controller.KeyDown -= new KeyEventHandler(Controller_KeyDown);
//            Controller.KeyUp -= new KeyEventHandler(Controller_KeyUp);
//            Controller.MouseDown -= new MouseEventHandler(Controller_MouseDown);
//            Controller.MouseUp -= new MouseEventHandler(Controller_MouseUp);
//            Controller.MouseMove -= new MouseEventHandler(Controller_MouseMove);
//            Controller.Paint -= new PaintEventHandler(Controller_Paint);
//        }

//        void Controller_MouseMove(object sender, MouseEventArgs e)
//        {
//            IToolController view = (IToolController)sender;

//            if (m_Model.PropertyInserter.UpdateCursor(e.Location))
//            {
//                m_Model.PropertyInserter.UpdateInserting();
//                view.Invalidate();
//            }
//        }

//        void Controller_MouseDown(object sender, MouseEventArgs e)
//        {
//            if (m_Model.PropertyInserter.Mode != InsertModeEnum.Nothing)
//                return;

//            IToolController view = (IToolController)sender;

//            if (e.Button == System.Windows.Forms.MouseButtons.Left)
//            {
//                m_Model.PropertyInserter.StartInserting(InsertModeEnum.Point);
//            }
//        }

//        void Controller_MouseUp(object sender, MouseEventArgs e)
//        {
//            if (m_Model.PropertyInserter.Mode == InsertModeEnum.Nothing)
//                return;

//            IToolController view = (IToolController)sender;

//            if (e.Button == System.Windows.Forms.MouseButtons.Left)
//            {
//                m_Model.PropertyInserter.FinishInserting();
//            }
//        }

//        void Controller_KeyUp(object sender, KeyEventArgs e)
//        {
//            IToolController view = (IToolController)sender;
//        }

//        void Controller_KeyDown(object sender, KeyEventArgs e)
//        {
//            IToolController view = (IToolController)sender;
//        }

//        void Controller_Paint(object sender, PaintEventArgs e)
//        {
//            int tileSize = m_Model.PropertyInserter.Model.Map.TileSize;

//            m_Model.PropertyInserter.DrawInsertion(e.Graphics, tileSize);
//        }

//    }
//}
