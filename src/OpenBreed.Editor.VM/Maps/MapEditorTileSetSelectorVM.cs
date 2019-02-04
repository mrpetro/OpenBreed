using OpenBreed.Common;
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
    public class MapEditorTileSetSelectorVM : BaseViewModel
    {

        #region Private Fields

        private int _currentIndex = -1;
        private TileSetVM _currentItem = null;
        private string _title;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorTileSetSelectorVM(MapEditorTilesToolVM parent)
        {
            Parent = parent;

            TileSets = new BindingList<TileSetVM>();
            TileSets.ListChanged += (s, a) => OnPropertyChanged(nameof(TileSets));

            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

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

        public MapEditorTilesToolVM Parent { get; }
        public BindingList<TileSetVM> TileSets { get; }
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        internal void Connect()
        {
            Parent.Parent.PropertyChanged += MapEditor_PropertyChanged;
        }

        #endregion Internal Methods

        #region Private Methods

        private void MapEditor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var mapEditor = sender as MapEditorVM;

            switch (e.PropertyName)
            {
                case nameof(mapEditor.Editable):
                    OnEditableChanged(mapEditor.Editable);
                    break;
                default:
                    break;
            }
        }

        private void OnEditableChanged(MapVM editable)
        {
            TileSets.UpdateAfter(() =>
            {
                if (editable == null)
                    TileSets.Clear();
                else
                    editable.TileSets.ForEach(item => TileSets.Add(item));
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
            CurrentIndex = Parent.Parent.Editable.TileSets.IndexOf(CurrentItem);
        }

        private void UpdateCurrentItem()
        {
            if (CurrentIndex == -1)
                CurrentItem = null;
            else
                CurrentItem = Parent.Parent.Editable.TileSets[CurrentIndex];
        }

        #endregion Private Methods

    }
}
