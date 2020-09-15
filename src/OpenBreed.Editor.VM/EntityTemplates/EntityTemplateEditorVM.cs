using OpenBreed.Common;
using OpenBreed.Model.EntityTemplates;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.EntityTemplates
{
    public class EntityTemplateEditorVM : EntryEditorBaseVM<IEntityTemplateEntry, EntityTemplateVM>
    {

        #region Public Constructors

        public EntityTemplateEditorVM(IRepository repository) : base(repository)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName { get { return "Entity Template Editor"; } }

        #endregion Public Properties

        #region Protected Methods

        protected override EntityTemplateVM CreateVM(IEntityTemplateEntry entry)
        {
            if (entry is IEntityTemplateFromFileEntry)
                return new EntityTemplateFromFileVM();
            else
                throw new NotImplementedException();
        }

        protected override void UpdateEntry(EntityTemplateVM source, IEntityTemplateEntry target)
        {
            var model = DataProvider.EntityTemplates.GetEntityTemplate(target.Id);

            model.EntityTemplate = source.EntityTemplate;

            base.UpdateEntry(source, target);
        }

        #endregion Protected Methods

    }
}
