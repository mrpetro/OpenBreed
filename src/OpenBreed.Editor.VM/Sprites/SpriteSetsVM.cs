using OpenBreed.Common.Sprites;
using OpenBreed.Editor.VM.Base;
using System;
using System.ComponentModel;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetsVM : BaseViewModel
    {

        #region Private Fields

        private int _currentIndex;
        private SpriteSetVM _currentItem;

        #endregion Private Fields

        #region Public Constructors

        public SpriteSetsVM(EditorVM root)
        {
            Root = root;

            SpriteViewer = new SpriteViewerVM(this);

            Items = new BindingList<SpriteSetVM>();
            Items.ListChanged += (s, e) => OnPropertyChanged(nameof(Items));

            PropertyChanged += SpriteSetsVM_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set
            {
                if (SetProperty(ref _currentIndex, value))
                    CurrentItem = Items[value];
            }
        }

        public SpriteSetVM CurrentItem
        {
            get { return _currentItem; }
            set
            {
                if (SetProperty(ref _currentItem, value))
                    CurrentIndex = Items.IndexOf(value);
            }
        }

        public BindingList<SpriteSetVM> Items { get; private set; }
        public EditorVM Root { get; private set; }
        public SpriteViewerVM SpriteViewer { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void AddSpriteSet(string spriteSetRef)
        {
            var spriteSetSourceDef = Root.CurrentDatabase.GetSourceDef(spriteSetRef);
            if (spriteSetSourceDef == null)
                throw new Exception("No SpriteSetSource definition found!");

            var source = Root.Sources.GetSource(spriteSetSourceDef);

            if (source == null)
                throw new Exception("SpriteSet source error: " + spriteSetRef);

            Items.Add(SpriteSetVM.Create(this, source));
        }

        #endregion Public Methods

        #region Private Methods

        private void SpriteSetsVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentItem):

                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods

    }
}