using OpenBreed.Common.Interface.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBreed.Common.Windows.Dialog
{
    public class FolderBrowserQuery : IFolderBrowserQuery
    {
        private FolderBrowserDialog _dialog = new System.Windows.Forms.FolderBrowserDialog();

        public string Description
        {
            get { return _dialog.Description; }
            set { _dialog.Description = value; }
        }

        public string SelectedPath
        {
            get { return _dialog.SelectedPath; }
            set { _dialog.SelectedPath = value; }
        }

        public DialogAnswer Show()
        {
            return DialogProvider.ToDialogAnswer(_dialog.ShowDialog());
        }
    }
}
