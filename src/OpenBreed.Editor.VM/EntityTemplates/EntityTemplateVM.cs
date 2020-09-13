using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Common.Model.EntityTemplates;
using OpenBreed.Common.Data;

namespace OpenBreed.Editor.VM.EntityTemplates
{
    public class EntityTemplateVM : EditableEntryVM
    {

        #region Private Fields

        private string _entityTemplate;
        private string _dataRef;

        #endregion Private Fields

        #region Public Constructors

        public EntityTemplateVM()
        {
            Initialize();
        }

        private void Initialize()
        {

        }

        #endregion Public Constructors

        #region Public Properties

        public string DataRef
        {
            get { return _dataRef; }
            set { SetProperty(ref _dataRef, value); }
        }

        public string EntityTemplate
        {
            get { return _entityTemplate; }
            set { SetProperty(ref _entityTemplate, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void FromModel(EntityTemplateModel model)
        {
            EntityTemplate = model.EntityTemplate;
        }

        #endregion Public Methods

        #region Internal Methods

        internal override void FromEntry(IEntry entry)
        {
            base.FromEntry(entry);
            FromEntry((IEntityTemplateEntry)entry);
        }

        internal virtual void FromEntry(IEntityTemplateEntry entry)
        {
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((IEntityTemplateEntry)entry);
        }
        internal virtual void ToEntry(IEntityTemplateEntry entry)
        {
        }

        #endregion Internal Methods

    }
}
