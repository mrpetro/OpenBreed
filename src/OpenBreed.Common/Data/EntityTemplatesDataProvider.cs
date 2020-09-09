using OpenBreed.Common.Model.EntityTemplates;
using OpenBreed.Common.Model.Texts;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Database.Interface.Items.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    public class EntityTemplatesDataProvider
    {
        #region Public Constructors

        public EntityTemplatesDataProvider(DataProvider provider)
        {
            Provider = provider;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties

        private EntityTemplateModel GetModelImpl(IEntityTemplateFromFileEntry entry)
        {
            return EntityTemplatesDataHelper.FromText(Provider, entry);
        }

        private EntityTemplateModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        public EntityTemplateModel GetEntityTemplate(string id)
        {
            var entry = Provider.UnitOfWork.GetRepository<IEntityTemplateEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Script error: " + id);

            return GetModel(entry);
        }
    }
}

