using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.XmlDatabase.Items.Assets;
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

        public AssetEditorVM(IRepository repository) : base(repository)
        {
        }

        public override string EditorName { get { return "Asset Editor"; } }

        #endregion Public Constructors

        #region Public Properties


        #endregion Public Properties

        #region Public Methods

        protected override AssetVM CreateVM(IAssetEntry entry)
        {
            if (entry is IFileAssetEntry)
                return new FileAssetVM();
            else if (entry is IEPFArchiveAssetEntry)
                return new EPFArchiveFileAssetVM();
            else
                throw new NotImplementedException();
        }

        protected override void UpdateEntry(AssetVM source, IAssetEntry target)
        {
            base.UpdateEntry(source, target);
        }

        protected override void UpdateVM(IAssetEntry source, AssetVM target)
        {
            base.UpdateVM(source, target);
        }

        #endregion Public Methods

        #region Internal Methods

        #endregion Internal Methods

    }
}
