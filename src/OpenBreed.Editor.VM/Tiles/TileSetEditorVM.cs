using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Tiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Tiles
{
    public class TileSetEditorVM : BaseViewModel
    {

        #region Private Fields

        private TileSetVM _currentTile;
        private string _title;

        #endregion Private Fields

        #region Public Constructors

        public TileSetViewerVM TileSetViewer { get; }

        public TileSetEditorVM(EditorVM root)
        {
            Root = root;

            TileSetViewer = new TileSetViewerVM();

            PropertyChanged += This_PropertyChanged;
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentTileSet):
                    if (CurrentTileSet != null)
                        Title = CurrentTileSet.Name;
                    else
                        Title = "No tile set";

                    TileSetViewer.CurrentTileSet = CurrentTileSet;
                    break;
                default:
                    break;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public TileSetVM CurrentTileSet
        {
            get { return _currentTile; }
            set { SetProperty(ref _currentTile, value); }
        }

        public EditorVM Root { get; }

        public int SelectedIndex { get; private set; }

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

        internal void TryLoad(string name)
        {
            var tileSet = Root.CreateTileSet();
            tileSet.Load(name);
            CurrentTileSet = tileSet;
        }

        #endregion Internal Methods

    }
}
