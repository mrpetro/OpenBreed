using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenBreed.Editor.UI.WinForms.Views;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Levels.Readers.XML;
using OpenBreed.Editor.VM.Levels;
using System.IO;
using OpenBreed.Common;

namespace OpenBreed.Editor.UI.WinForms.Forms
{
    public partial class ABTAMapListForm : Form
    {
        private EditorVM m_Model;
        public LevelDef SelectedLevelDef { get; set; }

        public ABTAMapListForm(EditorVM model, LevelDef initLevelDef)
        {
            InitializeComponent();

            m_Model = model;

            UpdateLevelList(initLevelDef);
        }

        private void UpdateLevelList(LevelDef initLevelDef)
        {

            var sourceDefs = m_Model.CurrentDatabase.GetAllSourcesOfType("LevelXML");

            List<LevelDef> levelDefs = new List<LevelDef>();

            foreach (var sourceDef in sourceDefs)
            {
                string path = "Resources\\ABTA\\Maps\\" + sourceDef.Name;
                string fullPath = Path.Combine(ProgramTools.AppDir, path);

                if (!File.Exists(fullPath))
                    continue;

                var levelDef = Tools.RestoreFromXml<LevelDef>(fullPath);

                levelDefs.Add(levelDef);
            }

            lstLevels.DataSource = levelDefs;

            if (initLevelDef != null)
            {
                if (lstLevels.Items.Contains(initLevelDef))
                    lstLevels.SelectedItem = initLevelDef;
            }
        }

        private void UpdateLevelInfo(LevelDef levelDef)
        {
            using (var model = m_Model.Projects.GetModel(levelDef))
            {
                tbxName.Text = model.Name;
                //tbxMap.Text = model.Map.Source.Name;
                //tbxDescription.Text = model.Map.Mission.MTXT;
                //cbxSpriteSets.DataSource = model.SpriteSets.Select(item => item.PresentationName).ToList();
            }
        }

        private void btnLevelEdit_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void lstLevels_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedLevelDef = (LevelDef)lstLevels.SelectedItem;

            if(lstLevels.SelectedItem != null)
                UpdateLevelInfo(SelectedLevelDef);
        }
    }
}
