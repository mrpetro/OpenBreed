using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Data;
using OpenBreed.Common.Model.Maps;
using OpenBreed.Common.Model.Maps.Blocks;
using OpenBreed.Common.Model.EntityTemplates;
using OpenBreed.Common.Model.Texts;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Database.Xml.Items.Assets;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.EntityTemplates
{
    public class EntityTemplateFromFileVM : EntityTemplateVM
    {

        #region Private Fields

        private bool _editEnabled;

        #endregion Private Fields

        #region Public Constructors

        public EntityTemplateFromFileVM()
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
            FromEntry((IEntityTemplateFromFileEntry)entry);
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((IEntityTemplateFromFileEntry)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void FromEntry(IEntityTemplateFromFileEntry entry)
        {
            var dataProvider = ServiceLocator.Instance.GetService<DataProvider>();

            var model = dataProvider.EntityTemplates.GetEntityTemplate(entry.Id);

            if (model != null)
                EntityTemplate = model.EntityTemplate;

            DataRef = entry.DataRef;
        }

        private void ToEntry(IEntityTemplateFromFileEntry source)
        {
            var model = ServiceLocator.Instance.GetService<DataProvider>().GetData<TextModel>(DataRef);

            model.Text = EntityTemplate;

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
