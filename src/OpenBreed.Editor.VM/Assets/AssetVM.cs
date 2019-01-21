using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Assets;

namespace OpenBreed.Editor.VM.Assets
{
    public class AssetVM : EditableEntryVM
    {

        #region Private Fields

        private string name;

        #endregion Private Fields

        #region Public Constructors

        public AssetVM()
        {
        }

        #endregion Public Constructors

        #region Internal Methods

        internal virtual void Load(IAssetEntry source)
        {
            Name = source.Name;
        }

        #endregion Internal Methods

    }
}
