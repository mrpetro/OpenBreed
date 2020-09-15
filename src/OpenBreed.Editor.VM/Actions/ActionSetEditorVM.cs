using OpenBreed.Common;
using OpenBreed.Model.Actions;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Actions
{
    public class ActionSetEditorVM : EntryEditorBaseVM<IActionSetEntry, ActionSetVM>
    {

        #region Public Constructors

        public ActionSetEditorVM(IRepository repository) : base(repository)
        {
            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName { get { return "Action Set Editor"; } }

        public int SelectedIndex { get; private set; }

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateEntry(ActionSetVM source, IActionSetEntry target)
        {
            base.UpdateEntry(source, target);
        }

        #endregion Protected Methods

        #region Private Methods

        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Editable):
                    SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods

    }
}
