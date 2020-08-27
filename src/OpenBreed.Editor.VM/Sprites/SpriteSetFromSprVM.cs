using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Sprites;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Editor.VM.Base;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetFromSprVM : SpriteSetVM
    {
        #region Private Fields

        private int _currentIndex = -1;
        private SpriteVM _currentItem;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetFromSprVM()
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

        #region Internal Methods

        internal override void FromEntry(IEntry entry)
        {
            FromEntry((ISpriteSetFromSprEntry)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void FromEntry(ISpriteSetFromSprEntry entry)
        {
            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.SpriteSets.GetSpriteSet(entry.Id);

            if (model != null)
                FromModel(model);
        }

        private void FromModel(SpriteSetModel spriteSet)
        {
            SetupSprites(spriteSet.Sprites);

            CurrentItem = Items.FirstOrDefault();
        }

        private void SetupSprites(List<SpriteModel> sprites)
        {
            Items.UpdateAfter(() =>
            {
                Items.Clear();

                foreach (var sprite in sprites)
                    Items.Add(SpriteVM.Create(sprite));
            });
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