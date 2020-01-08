using System.ComponentModel;

namespace OpenBreed.Editor.VM.Sprites
{
    public class SpriteSetVM : EditableEntryVM
    {
        #region Public Constructors

        public SpriteSetVM()
        {
            Items = new BindingList<SpriteVM>();
            Items.ListChanged += (s, e) => OnPropertyChanged(nameof(Items));
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<SpriteVM> Items { get; }

        #endregion Public Properties
    }
}