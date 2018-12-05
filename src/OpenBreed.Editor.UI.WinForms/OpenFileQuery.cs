using OpenBreed.Editor.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBreed.Editor.UI.WinForms
{
    public class OpenFileQuery : IOpenFileQuery
    {
        private System.Windows.Forms.OpenFileDialog _dialog = new System.Windows.Forms.OpenFileDialog();

        public string InitialDirectory
        {
            get { return _dialog.InitialDirectory; }
            set { _dialog.InitialDirectory = value; }
        }

        public string Filter
        {
            get { return _dialog.Filter; }
            set { _dialog.Filter = value; }
        }

        public bool Multiselect
        {
            get { return _dialog.Multiselect; }
            set { _dialog.Multiselect = value; }
        }

        public string Title
        {
            get { return _dialog.Title; }
            set { _dialog.Title = value; }
        }

        public string[] FileNames
        {
            get { return _dialog.FileNames; }
        }

        public string FileName
        {
            get { return _dialog.FileName; }
            set { _dialog.FileName = value; }
        }

        public DialogAnswer Show()
        {
            return DialogProvider.ToDialogAnswer(_dialog.ShowDialog());
        }
    }
}
