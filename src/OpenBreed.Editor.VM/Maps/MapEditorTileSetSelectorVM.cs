using OpenBreed.Common;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Tiles.Helpers;
using OpenBreed.Model.Tiles;
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

        private string currentItem = null;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorTileSetSelectorVM(MapEditorTilesToolVM parent)
        {
            Parent = parent;

            TileSetNames = new BindingList<string>();
            TileSetNames.ListChanged += (s, a) => OnPropertyChanged(nameof(TileSetNames));
        }

        #endregion Public Constructors

        #region Public Properties

        public string CurrentItem
        {
            get { return currentItem; }
            set { SetProperty(ref currentItem, value); }
        }

        public MapEditorTilesToolVM Parent { get; }

        public BindingList<string> TileSetNames { get; }

        #endregion Public Properties

        #region Internal Methods

        internal void UpdateList(string tileSetRef)
        {
            TileSetNames.UpdateAfter(() =>
            {
                TileSetNames.Clear();
                TileSetNames.Add(tileSetRef);
            });

            CurrentItem = TileSetNames.FirstOrDefault();
        }

        #endregion Internal Methods

        #region Private Methods


        public event EventHandler<string> CurrentItemChanged;

        private void OnCurrentItemChanged()
        {
            CurrentItemChanged?.Invoke(this, CurrentItem);
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(CurrentItem):
                    OnCurrentItemChanged();
                    break;
                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }


        #endregion Private Methods

    }
}
