using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{
    public class EditableEntryVM : BaseViewModel
    {
        #region Private Fields

        private string _name;

        #endregion Private Fields

        #region Public Properties

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        #endregion Public Properties
    }
}
