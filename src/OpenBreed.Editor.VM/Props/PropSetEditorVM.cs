using OpenBreed.Common;
using OpenBreed.Common.Props;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Props
{
    public class PropSetEditorVM : EntryEditorBaseVM<IPropSetEntry, PropSetVM>
    {

        #region Public Constructors

        public PropSetEditorVM(IRepository repository) : base(repository)
        {
            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName { get { return "Property Set Editor"; } }

        public int SelectedIndex { get; private set; }

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateEntry(PropSetVM source, IPropSetEntry target)
        {
            base.UpdateEntry(source, target);
        }

        protected override void UpdateVM(IPropSetEntry source, PropSetVM target)
        {
            var model = DataProvider.GetPropSet(source.Id);

            target.FromModel(model);

            base.UpdateVM(source, target);
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
