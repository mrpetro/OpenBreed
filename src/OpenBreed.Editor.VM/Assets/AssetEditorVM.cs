using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Assets
{
    public class AssetEditorVM : EntryEditorBaseVM<IAssetEntry, AssetVM>
    {
        #region Public Constructors

        public AssetEditorVM()
        {
        }

        public override string EditorName { get { return "Asset Editor"; } }

        #endregion Public Constructors

        #region Public Properties


        #endregion Public Properties

        #region Public Methods

        protected override void UpdateEntry(AssetVM source, IAssetEntry target)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateVM(IAssetEntry source, AssetVM target)
        {
            var model = DataProvider.AssetsProvider.GetAsset(source.Name);
            target.Name = source.Name;
        }

        #endregion Public Methods

        #region Internal Methods

        #endregion Internal Methods

    }
}
