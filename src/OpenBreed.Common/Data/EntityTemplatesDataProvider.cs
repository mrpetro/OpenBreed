using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Model.EntityTemplates;
using System;

namespace OpenBreed.Common.Data
{
    public class EntityTemplatesDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;

        #endregion Private Fields

        #region Public Constructors

        public EntityTemplatesDataProvider(IModelsProvider provider, IRepositoryProvider repositoryProvider)
        {
            Provider = provider;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Public Properties

        public IModelsProvider Provider { get; }

        #endregion Public Properties

        #region Public Methods

        public EntityTemplateModel GetEntityTemplate(string id)
        {
            var entry = repositoryProvider.GetRepository<IDbEntityTemplate>().GetById(id);
            if (entry == null)
                throw new Exception("Script error: " + id);

            return GetModel(entry);
        }

        #endregion Public Methods

        #region Private Methods

        private EntityTemplateModel GetModelImpl(IDbEntityTemplateFromFile entry)
        {
            return EntityTemplatesDataHelper.FromText(Provider, entry);
        }

        private EntityTemplateModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        #endregion Private Methods
    }
}