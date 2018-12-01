using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.UI.WinForms.Helpers;
using OpenBreed.Common;
using OpenBreed.Editor.Cfg.Options.ABSE;

namespace OpenBreed.Editor.UI.WinForms.Controls
{
    public partial class EditorOptionsABSE : UserControl
    {
        public EditorOptionsABSE()
        {
            InitializeComponent();
            btnSelectGameFolder.Click += btnSelectGameFolder_Click;
        }

        private void SelectGameFolder()
        {
            FolderBrowserDialogHelper.PrepareToSelectABHCGameFolder(FolderBrowserDialog, tbxGameFolderPath.Text);

            DialogResult result = FolderBrowserDialog.ShowDialog(this);

            if (result == System.Windows.Forms.DialogResult.OK)
                tbxGameFolderPath.Text = FolderBrowserDialog.SelectedPath;
        }

        public void UpdateCfgWithCtrl(ABSECfg cfg)
        {
            cfg.GameFolderPath = tbxGameFolderPath.Text;
        }

        public void UpdateCtrlWithCfg(ABSECfg cfg)
        {
            tbxGameFolderPath.Text = cfg.GameFolderPath;
        }

        private void btnSelectGameFolder_Click(object sender, EventArgs e)
        {
            Tools.TryAction(SelectGameFolder);
        }
    }
}
