using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using OpenBreed.Editor.VM.Maps.Tools;

namespace OpenABEd
{
    public partial class ToolsView : DockContent
    {
        private ToolsMan _vm;

        //private List<System.Windows.Forms.Button> m_ToolButtons = new List<Button>();

        public ToolsView()
        {
            InitializeComponent();
        }

        private void AddToolButton(string toolName)
        {
            var button = new ToolStripSplitButton();

            button.Name = toolName;
            button.AutoSize = false;
            button.Size = new System.Drawing.Size(32, 32);
            button.Text = toolName;
            button.Click += (o, a) => { _vm.ActivateTool(toolName); CheckOnlyOneButton(button); };
            button.Margin = new System.Windows.Forms.Padding(20);
            ToolStrip.Items.Add(button);

            if (button.Name == "InsertTileTool")
            {
                button.DropDownItems.Add("Draw");
                button.DropDownItems.Add("Line");
                button.DropDownItems.Add("Box");

            }
            else if (button.Name == "InsertPropertyTool")
            {
                button.DropDownItems.Add("Draw");
                button.DropDownItems.Add("Line");
                button.DropDownItems.Add("Box");

            }

        }

        private void CheckOnlyOneButton(ToolStripSplitButton button)
        {



        }

        public void Initialize(ToolsMan vm)
        {
            _vm = vm;

            List<string> toolNames = _vm.GetActivableToolsList();

            //m_ToolButtons.Clear();
            this.SuspendLayout();

            foreach (var toolName in toolNames)
                AddToolButton(toolName);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
