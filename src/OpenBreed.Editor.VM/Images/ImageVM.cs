namespace OpenBreed.Editor.VM.Images
{
    public class ImageVM : EditableEntryVM
    {
        #region Private Fields

        private string _assetRef;

        #endregion Private Fields

        #region Public Constructors

        public ImageVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string AssetRef
        {
            get { return _assetRef; }
            set { SetProperty(ref _assetRef, value); }
        }

        #endregion Public Properties
    }
}