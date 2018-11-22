using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Editor.VM.Maps.Tools
{
    public class LevelTool : IEditorTool
    {
        private string m_Name;

        private readonly IToolController m_Controller;

        private ToolsMan m_ToolsMan;

        public string Name { get { return m_Name; } }
        protected IToolController Controller { get { return m_Controller; } }

        protected LevelTool(string name, IToolController controller)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (controller == null)
                throw new ArgumentNullException("controller");

            m_Name = name;
            m_Controller = controller;
        }

        public virtual void Activate()
        {
            throw new NotImplementedException();
        }

        public virtual void Deactivate()
        {
            throw new NotImplementedException();
        }

        public void Register(ToolsMan toolsMan)
        {
            if (m_ToolsMan != null)
                throw new InvalidOperationException("Tool already registered!");

            m_ToolsMan = toolsMan;
        }
    }
}
