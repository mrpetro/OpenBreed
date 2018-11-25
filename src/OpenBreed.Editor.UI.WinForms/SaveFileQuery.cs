using OpenBreed.Editor.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.WinForms
{
    public class SaveFileQuery : ISaveFileQuery
    {
        private System.Windows.Forms.SaveFileDialog _dialog = new System.Windows.Forms.SaveFileDialog();

        public string Filter
        {
            get { return _dialog.Filter; }
            set { _dialog.Filter = value; }
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

        public string InitialDirectory
        {
            get { return _dialog.InitialDirectory; }
            set { _dialog.InitialDirectory = value; }
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
