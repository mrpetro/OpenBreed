using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteViewerVM : BaseViewModel
    {

        #region Private Fields

        private int _currentIndex = -1;
        private SpriteVM _currentItem = null; 

        private SpriteSetVM _currentSpriteSet;

        #endregion Private Fields

        #region Public Constructors

        public SpriteViewerVM(SpriteSetsVM spriteSets)
        {
            SpriteSets = spriteSets;

            PropertyChanged += SpriteViewerVM_PropertyChanged;

            SpriteSets.PropertyChanged += SpriteSets_PropertyChanged;
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
            private set { SetProperty(ref _currentItem, value); }
        }

        public SpriteSetVM CurrentSpriteSet
        {
            get { return _currentSpriteSet; }
            set { SetProperty(ref _currentSpriteSet, value); }
        }

        public SpriteSetsVM SpriteSets { get; private set; }

        #endregion Public Properties

        #region Private Methods

        private void SpriteSets_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SpriteSets.CurrentItem):
                    CurrentSpriteSet = SpriteSets.CurrentItem;
                    break;
                default:
                    break;
            }
        }

        private void SpriteViewerVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentIndex):
                    UpdateCurrentItem();
                    break;
                case nameof(CurrentItem):
                    UpdateCurrentIndex();
                    break;
                case nameof(CurrentSpriteSet):
                    CurrentItem = CurrentSpriteSet.Items.FirstOrDefault();
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
