using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;
using OpenBreed.Common.Actions;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorActionsToolVM : BaseViewModel
    {
        #region Private Fields

        private ActionSetVM _actionSet;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorActionsToolVM(MapEditorVM parent)
        {
            Parent = parent;

            ActionsMan = new MapEditorActionsManVM(this);
            ActionsSelector = new MapEditorActionsSelectorVM(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public ActionSetVM ActionSet
        {
            get { return _actionSet; }
            set { SetProperty(ref _actionSet, value); }
        }

        public MapEditorActionsManVM ActionsMan { get; }
        public MapEditorActionsSelectorVM ActionsSelector { get; }
        public MapEditorVM Parent { get; }

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
                ActionSet = null;
            if (map != null)
                ActionSet = map.ActionSet;
        }

        #endregion Private Methods

    }
}
