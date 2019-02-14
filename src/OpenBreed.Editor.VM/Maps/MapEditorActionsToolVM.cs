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

        #region Private Methods

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

        #endregion Private Methods

    }
}
