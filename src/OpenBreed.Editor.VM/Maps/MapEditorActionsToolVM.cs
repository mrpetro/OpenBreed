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
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.VM.Maps.Layers;

namespace OpenBreed.Editor.VM.Maps
{
    public class MapEditorActionsToolVM : MapEditorToolVM
    {

        #region Private Fields

        private ActionSetVM _actionSet;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorActionsToolVM(MapEditorVM parent)
        {
            Parent = parent;

            ActionEntryRef = new EntryRefVM(typeof(IActionSetEntry));
            ActionsSelector = new MapEditorActionsSelectorVM(this);

            ActionEntryRef.PropertyChanged += ActionEntryRef_PropertyChanged;

            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public EntryRefVM ActionEntryRef { get; }

        public ActionSetVM ActionSet
        {
            get { return _actionSet; }
            set { SetProperty(ref _actionSet, value); }
        }

        public MapEditorActionsSelectorVM ActionsSelector { get; }

        public MapEditorVM Parent { get; }

        #endregion Public Properties

        #region Internal Methods

        internal void Load(string actionSetEntryId)
        {
            if (actionSetEntryId == null)
                ActionSet = null;
            else
            {
                var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();
                var actionSet = dataProvider.GetActionSet(actionSetEntryId);

                var actionSetVM = new ActionSetVM();
                actionSetVM.FromEntry(actionSet);
                ActionSet = actionSetVM;
            }
        }

        #endregion Internal Methods

        #region Protected Methods

        internal override void OnCursor(MapViewCursorVM cursor)
        {
            if (cursor.Action == CursorActions.Down && cursor.Buttons.HasFlag(CursorButtons.Left))
            {
                var actionLayer = Parent.Editable.Layout.Layers.OfType<MapLayerActionVM>().FirstOrDefault();

                var actionCode = ActionsSelector.SelectedIndex;

                actionLayer.SetCell(cursor.WorldIndexCoords.X, cursor.WorldIndexCoords.Y, actionCode);

            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void ActionEntryRef_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ActionEntryRef.RefId):
                    Load(ActionEntryRef.RefId);
                    break;
                default:
                    break;
            }
        }

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ActionSet):
                    ActionEntryRef.RefId = (ActionSet == null) ? null : ActionSet.Id;
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods

    }
}
