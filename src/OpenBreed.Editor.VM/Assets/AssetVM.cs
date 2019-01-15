using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Assets
{
    public class AssetVM : EditableEntryVM
    {
        #region Private Fields

        private string _source;

        #endregion Private Fields

        #region Public Constructors

        public AssetVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }

        #endregion Public Properties

    }
}
