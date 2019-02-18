using OpenBreed.Common.Assets;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Images
{
    public class ImageDataSourceVM : BaseViewModel
    {
        #region Public Constructors

        public ImageDataSourceVM()
        {

            AssetEntryRef = new EntryRefVM(typeof(IAssetEntry));
            AssetEntryRef.PropertyChanged += AssetEntryRef_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public EntryRefVM AssetEntryRef { get; }

        #endregion Public Properties

        #region Private Methods

        private void AssetEntryRef_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AssetEntryRef.RefId):
                    //Load(AssetEntryRef.RefId);
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods


    }
}
