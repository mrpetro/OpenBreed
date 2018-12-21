using OpenBreed.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Editor.VM.Levels.Tools
{
    public delegate void ToolActivatedEventHandler(object sender, ToolActivatedEventArgs e);

    public class ToolActivatedEventArgs : EventArgs
    {
        public string ToolName { get; set; }

        public ToolActivatedEventArgs(string toolName)
        {
            ToolName = toolName;
        }
    }

    public class ToolsMan
    {
        private readonly Dictionary<string, IEditorTool> m_PassiveTools = new Dictionary<string, IEditorTool>();
        private readonly Dictionary<string, IEditorTool> m_ActivableTools = new Dictionary<string, IEditorTool>();
        private IEditorTool m_ActiveTool = null;

        public event ToolActivatedEventHandler ToolActivated;

        public ToolsMan()
        {
        }

        protected virtual void OnToolActivated(ToolActivatedEventArgs e)
        {
            if (ToolActivated != null) ToolActivated(this, e);
        }

        /// <summary>
        /// Passive tools are always active
        /// </summary>
        /// <param name="tool"></param>
        public void AddPassiveTool(IEditorTool tool)
        {
            if (m_PassiveTools.ContainsKey(tool.Name))
                throw new InvalidOperationException(tool + " already added to passive tools");

            m_PassiveTools.Add(tool.Name, tool);

            //Passive tools are activated immediately
            tool.Activate();
        }

        /// <summary>
        /// Activable tools are activated on demand
        /// </summary>
        /// <param name="tool"></param>
        public void AddActivableTool(IEditorTool tool)
        {
            if (m_ActivableTools.ContainsKey(tool.Name))
                throw new InvalidOperationException(tool + " already added to activable tools");

            m_ActivableTools.Add(tool.Name, tool);
        }

        /// <summary>
        /// This deactivates tool that is currently active
        /// </summary>
        public void DeactivateTool()
        {
            if (m_ActiveTool == null)
            {
                LogMan.Instance.LogDebug("No tool is currently active.");
                return;
            }

            m_ActiveTool.Deactivate();
        }

        /// <summary>
        /// This activates the tool with given name, if it's known as activable
        /// Any already active tool is first being deactivated.
        /// </summary>
        /// <param name="toolName"></param>
        public void ActivateTool(string toolName)
        {
            if (m_ActiveTool != null)
            {
                if (m_ActiveTool.Name == toolName)
                {
                    LogMan.Instance.LogDebug("Tool " + toolName + " already active.");
                    return;
                }
            }

            IEditorTool tool = null;

            if (m_ActivableTools.TryGetValue(toolName, out tool))
            {
                DeactivateTool();

                tool.Activate();
                m_ActiveTool = tool;
                OnToolActivated(new ToolActivatedEventArgs(toolName));
            }
            else
                throw new InvalidOperationException("Tool " + toolName + " not known as activable.");
        }

        public List<string> GetActivableToolsList()
        {
            return m_ActivableTools.Keys.ToList();
        }

        public void ClearTools()
        {
            foreach (var passiveTool in m_PassiveTools)
                passiveTool.Value.Deactivate();

            m_PassiveTools.Clear();

            if (m_ActiveTool != null)
                DeactivateTool();

            m_ActivableTools.Clear();
        }
    }
}
