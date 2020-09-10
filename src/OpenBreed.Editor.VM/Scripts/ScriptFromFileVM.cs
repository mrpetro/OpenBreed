using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Data;
using OpenBreed.Common.Model.Maps;
using OpenBreed.Common.Model.Maps.Blocks;
using OpenBreed.Common.Model.Scripts;
using OpenBreed.Common.Model.Texts;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Xml.Items.Assets;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Scripts
{
    public class ScriptFromFileVM : ScriptVM
    {

        #region Private Fields

        private bool _editEnabled;

        #endregion Private Fields

        #region Public Constructors

        public ScriptFromFileVM()
        {
            PropertyChanged += This_PropertyChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool EditEnabled
        {
            get { return _editEnabled; }
            set { SetProperty(ref _editEnabled, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        internal override void FromEntry(IEntry entry)
        {
            base.FromEntry(entry);
            FromEntry((IScriptFromFileEntry)entry);
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((IScriptFromFileEntry)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void FromEntry(IScriptFromFileEntry entry)
        {
            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.Scripts.GetScript(entry.Id);

            if (model != null)
                Script = model.Script;

            DataRef = entry.DataRef;
        }

        private void ToEntry(IScriptFromFileEntry source)
        {
            var model = ServiceLocator.Instance.GetService<DataProvider>().GetData<TextModel>(DataRef);

            model.Text = Script;

            source.DataRef = DataRef;
        }

        private void This_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(DataRef):
                    EditEnabled = ValidateSettings();
                    break;
                default:
                    break;
            }
        }

        private bool ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(DataRef))
                return false;

            return true;
        }

        #endregion Private Methods
    }
}
