using OpenBreed.Common.Interface.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace OpenBreed.Common.Windows.Dialog
{
    public class OpenFileQuery : IOpenFileQuery
    {
        #region Private Fields

        private readonly OpenFileDialog _dialog = new OpenFileDialog();

        #endregion Private Fields

        #region Public Properties

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

        #endregion Public Properties

        #region Public Methods

        public DialogAnswer Show()
        {
            if (_dialog.ShowDialog() is not bool result)
            {
                return DialogAnswer.Cancel;
            }

            return result ? DialogAnswer.OK : DialogAnswer.Cancel;
        }

        #endregion Public Methods
    }
}