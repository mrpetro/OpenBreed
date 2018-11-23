using OpenBreed.Common;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetViewerVM : BaseViewModel
    {

        #region Private Fields

        private int _currentIndex = -1;
        private SpriteSetVM _currentItem = null;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetViewerVM(EditorVM root)
        {
            Root = root;

            PropertyChanged += SpriteSetsVM_PropertyChanged;
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

        public EditorVM Root { get; private set; }

        #endregion Public Properties

        #region Private Methods

        private void SpriteSetsVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
            CurrentIndex = Root.SpriteSets.IndexOf(CurrentItem);
        }

        private void UpdateCurrentItem()
        {
            if (CurrentIndex == -1)
                CurrentItem = null;
            else
                CurrentItem = Root.SpriteSets[CurrentIndex];
        }

        #endregion Private Methods

    }
}
