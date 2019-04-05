using OpenBreed.Common.Palettes;
using OpenBreed.Common.Sprites;
using OpenBreed.Editor.VM.Base;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetFromImageVM : SpriteSetVM
    {
        #region Private Fields

        private int _currentIndex = -1;
        private SpriteVM _currentItem;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetFromImageVM()
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

        #endregion Public Properties

        #region Public Methods

        public void Connect()
        {
        }

        #endregion Public Methods

        #region Private Methods

        internal override void FromModel(SpriteSetModel spriteSet)
        {
            SetupSprites(spriteSet.Sprites);

            CurrentItem = Items.FirstOrDefault();
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
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
                CurrentIndex = Items.IndexOf(CurrentItem);
        }

        private void UpdateCurrentItem()
        {
            if (CurrentIndex == -1)
                CurrentItem = null;
            else
                CurrentItem = Items[CurrentIndex];
        }

        #endregion Private Methods
    }
}