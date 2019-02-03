using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Tiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorTilesToolVM : BaseViewModel
    {
        #region Private Fields

        private int _currentIndex = -1;
        private TileSetVM _currentItem = null;
        private string _title;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorTilesToolVM(MapEditorVM parent)
        {
            Parent = parent;

            PropertyChanged += LevelTileSelectorVM_PropertyChanged;

            Parent.PropertyChanged += Parent_PropertyChanged;
        }

        private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var levelEditor = sender as MapEditorVM;

            switch (e.PropertyName)
            {
                case nameof(levelEditor.Editable):
                    OnMapChange(levelEditor.Editable);
                    break;
                default:
                    break;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public BindingList<TileSetVM> Items { get; private set; }

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { SetProperty(ref _currentIndex, value); }
        }

        public TileSetVM CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        public MapEditorVM Parent { get; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        internal void Connect()
        {
        }

        #endregion Internal Methods

        #region Private Methods

        private void LevelTileSelectorVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
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

        private void TileSetsVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentItem):
                    if (CurrentItem == null)
                        Title = "Tile sets - <no current tile set>";
                    else
                        Title = "Tile sets - " + CurrentItem;
                    break;
                default:
                    break;
            }
        }

        private void OnMapChange(LevelVM level)
        {
            if (level == null)
                Items = new BindingList<TileSetVM>();
            else
                Items = level.TileSets;
        }


        private void UpdateCurrentIndex()
        {
            CurrentIndex = Parent.Editable.TileSets.IndexOf(CurrentItem);
        }

        private void UpdateCurrentItem()
        {
            if (CurrentIndex == -1)
                CurrentItem = null;
            else
                CurrentItem = Parent.Editable.TileSets[CurrentIndex];
        }

        #endregion Private Methods

    }
}
