using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Props;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorActionsToolVM : BaseViewModel
    {

        #region Private Fields

        private PropSetVM _currentItem;

        private string _title;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorActionsToolVM(MapEditorVM parent)
        {
            Parent = parent;

            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public PropSetVM CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        public MapEditorVM Parent { get; }
        public int SelectedIndex { get; private set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Connect()
        {
            Parent.PropertyChanged += MapEditor_PropertyChanged;
        }

        #endregion Public Methods

        #region Private Methods

        private void MapEditor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var mapEditor = sender as MapEditorVM;

            switch (e.PropertyName)
            {
                case nameof(mapEditor.Editable):
                    OnCurrentMapChanged(mapEditor.Editable);
                    break;
                default:
                    break;
            }
        }

        private void OnCurrentMapChanged(MapVM map)
        {
            if (map == null)
                CurrentItem = null;
            else
                CurrentItem = map.PropSet;
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentItem):
                    SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}
