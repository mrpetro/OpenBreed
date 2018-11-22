using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenBreed.Editor.UI.WinForms.Helpers;
using OpenBreed.Common;
using OpenBreed.Editor.Cfg.Options.ABTA;

namespace OpenBreed.Editor.UI.WinForms.Controls
{
    public partial class EditorOptionsABTA : UserControl
    {
        public EditorOptionsABTA()
        {
            InitializeComponent();
        }

        private void SelectGameFolder()
        {
            FolderBrowserDialogHelper.PrepareToSelectABTAGameFolder(FolderBrowserDialog, tbxGameFolderPath.Text);

            DialogResult result = FolderBrowserDialog.ShowDialog(this);

            if (result == System.Windows.Forms.DialogResult.OK)
                tbxGameFolderPath.Text = FolderBrowserDialog.SelectedPath;
        }

        private void SelectGameRunFile()
        {
            OpenFileDialogHelper.PrepareToSelectABTAGameRunFile(OpenFileDialog, tbxGameRunFilePath.Text);

            DialogResult result = OpenFileDialog.ShowDialog(this);

            if (result == System.Windows.Forms.DialogResult.OK)
                tbxGameRunFilePath.Text = OpenFileDialog.FileName;
        }

        public void UpdateCfgWithCtrl(ABTACfg cfg)
        {
            cfg.GameFolderPath = tbxGameFolderPath.Text;
            cfg.GameRunFilePath = tbxGameRunFilePath.Text;
            cfg.GameRunFileArgs = tbxGameRunFileArgs.Text;
        }

        public void UpdateCtrlWithCfg(ABTACfg cfg)
        {
            tbxGameFolderPath.Text = cfg.GameFolderPath;
            tbxGameRunFilePath.Text = cfg.GameRunFilePath;
            tbxGameRunFileArgs.Text = cfg.GameRunFileArgs;
        }

        private void btnSelectGameFolder_Click(object sender, EventArgs e)
        {
            Tools.TryAction(SelectGameFolder);
        }

        private void btnSelectGameRunFile_Click(object sender, EventArgs e)
        {
            Tools.TryAction(SelectGameRunFile);
        }
    }
}
