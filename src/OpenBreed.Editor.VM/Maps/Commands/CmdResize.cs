//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using OpenBreed.Common.Commands;
//using OpenBreed.Editor.VM.Levels.Helpers;

//namespace OpenBreed.Editor.VM.Levels.Commands
//{
//    class CmdResize : ICommand
//    {
//        private readonly MapBodyVM m_Presentation;
//        private readonly MapResizeOperation m_Operation;

//        public CmdResize(MapBodyVM presentation, MapResizeOperation operation)
//        {
//            m_Presentation = presentation;
//            m_Operation = operation;
//        }

//        public void Execute()
//        {
//            m_Presentation.Resize(m_Operation.SizeXAfter, m_Operation.SizeYAfter);
//            m_Presentation.Update();
//        }

//        public void UnExecute()
//        {
//            m_Presentation.Resize(m_Operation.SizeXBefore, m_Operation.SizeYBefore);
//            m_Presentation.Update();
//        }
//    }
//}
