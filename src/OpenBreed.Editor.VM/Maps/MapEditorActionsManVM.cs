using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorActionsManVM : BaseViewModel
    {

        #region Private Fields

        private string _actionSetId;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorActionsManVM(MapEditorActionsToolVM parent)
        {
            Parent = parent;
        }

        #endregion Public Constructors

        #region Public Properties

        public MapEditorActionsToolVM Parent { get; }

        public string ActionSetId
        {
            get { return _actionSetId; }
            set { SetProperty(ref _actionSetId, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void Connect()
        {
            Parent.PropertyChanged += MapEditor_PropertyChanged;
        }

        public void SelectActionSetId()
        {

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
            ActionSetId = null;

            if (map == null)
                return;

            if(map.ActionSet == null)
                return;

            ActionSetId = map.ActionSet.Id;
        }

        #endregion Private Methods

    }
}
