using OpenBreed.Common;
using OpenBreed.Editor.VM.Base;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetViewerVM : BaseViewModel
    {
        #region Private Fields

        private int _currentIndex = -1;
        private SpriteVM _currentItem;
        private SpriteSetVM _currentSpriteSet;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetViewerVM()
        {
            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { SetProperty(ref _currentIndex, value); }
        }

        public SpriteVM CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        public SpriteSetVM CurrentSpriteSet
        {
            get { return _currentSpriteSet; }
            set { SetProperty(ref _currentSpriteSet, value); }
        }

        public EditorVM Root { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Connect()
        {
        }

        #endregion Public Methods

        #region Private Methods

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentSpriteSet):
                    UpdateCurrentItem();
                    break;
                case nameof(CurrentIndex):
                    UpdateCurrentItem();
                    break;

                case nameof(CurrentItem):
                    UpdateCurrentIndex();
                    break;
                default:
                    break;
            }
        }

        private void UpdateCurrentIndex()
        {
            if (CurrentItem == null)
                CurrentIndex = -1;
            else
                CurrentIndex = CurrentSpriteSet.Items.IndexOf(CurrentItem);
        }

        private void UpdateCurrentItem()
        {
            if (CurrentIndex == -1)
                CurrentItem = null;
            else
                CurrentItem = CurrentSpriteSet.Items[CurrentIndex];
        }

        #endregion Private Methods
    }
}