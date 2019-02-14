using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Common.Actions;

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

            Parent.PropertyChanged += Parent_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public string ActionSetId
        {
            get { return _actionSetId; }
            set { SetProperty(ref _actionSetId, value); }
        }

        public Action<RefSelectorVM> OpenRefIdSelectorAction { get; set; }
        public MapEditorActionsToolVM Parent { get; }

        #endregion Public Properties

        #region Public Methods

        public void SelectActionSetId()
        {
            var refSelector = new RefSelectorVM<IActionSetEntry>();
            refSelector.CurrentEntryId = ActionSetId;
            OpenRefIdSelectorAction?.Invoke(refSelector);

            if (refSelector.SelectedEntryId == null)
                return;

            ActionSetId = refSelector.SelectedEntryId;

            Parent.Load(ActionSetId);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnActionSetChanged(ActionSetVM actionSet)
        {
            ActionSetId = null;

            if (actionSet == null)
                return;

            ActionSetId = actionSet.Id;
        }

        private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Parent.ActionSet):
                    OnActionSetChanged(Parent.ActionSet);
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods

    }
}
