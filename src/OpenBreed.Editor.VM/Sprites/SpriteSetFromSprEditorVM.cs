using OpenBreed.Common.Data;
using OpenBreed.Common.Helpers;
using OpenBreed.Common.Model.Sprites;
using OpenBreed.Common.Sprites;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Editor.VM.Base;
using System.Collections.Generic;
using System.ComponentModel;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetFromSprEditorVM : BaseViewModel, IEntryEditor<ISpriteSetEntry, SpriteSetVM>
    {
        #region Private Fields

        private int currentSpriteIndex = -1;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetFromSprEditorVM(SpriteSetEditorVM parent)
        {
            Parent = parent;

            Parent.PropertyChanged += Parent_PropertyChanged;

            Items = new BindingList<SpriteVM>();
        }

        #endregion Public Constructors

        #region Public Properties

        public SpriteSetEditorVM Parent { get; }
        public BindingList<SpriteVM> Items { get; }

        public int CurrentSpriteIndex
        {
            get { return currentSpriteIndex; }
            set { SetProperty(ref currentSpriteIndex, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public SpriteSetVM CreateVM(ISpriteSetEntry entry)
        {
            return new SpriteSetFromSprVM();
        }

        public void UpdateEntry(SpriteSetVM vm, ISpriteSetEntry entry)
        {
        }

        public void UpdateVM(ISpriteSetEntry entry, SpriteSetVM vm)
        {
            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.SpriteSets.GetSpriteSet(entry.Id);

            if (model != null)
                FromModel(model);

            CurrentSpriteIndex = 0;
        }

        #endregion Public Methods

        #region Private Methods

        private void FromModel(SpriteSetModel spriteSet)
        {
            SetupSprites(spriteSet.Sprites);
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

        private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Parent.Palette):
                    foreach (var item in Items)
                        BitmapHelper.SetPaletteColors(item.Image, Parent.Palette.Data);
                    break;

                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}