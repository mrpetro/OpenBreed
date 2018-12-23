using OpenBreed.Common;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Levels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetSelectorVM : BaseViewModel
    {

        #region Private Fields

        private int _currentIndex = -1;
        private SpriteSetVM _currentItem = null;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetSelectorVM(LevelEditorVM parent)
        {
            Parent = parent;

            PropertyChanged += SpriteSetViewerVM_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { SetProperty(ref _currentIndex, value); }
        }

        public SpriteSetVM CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        public LevelEditorVM Parent { get; private set; }

        #endregion Public Properties

        #region Private Methods

        private void SpriteSetViewerVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
            CurrentIndex = Parent.CurrentLevel.SpriteSets.IndexOf(CurrentItem);
        }

        private void UpdateCurrentItem()
        {
            if (CurrentIndex == -1)
                CurrentItem = null;
            else
                CurrentItem = Parent.CurrentLevel.SpriteSets[CurrentIndex];
        }

        #endregion Private Methods

    }
}
