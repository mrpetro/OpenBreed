using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Model.EntityTemplates;
using System;

namespace OpenBreed.Common.Data
{
    public class EntityTemplatesDataProvider
    {
        #region Private Fields

        private readonly IWorkspaceMan workspaceMan;

        #endregion Private Fields

        #region Public Constructors

        public EntityTemplatesDataProvider(IDataProvider provider, IWorkspaceMan workspaceMan)
        {
            Provider = provider;
            this.workspaceMan = workspaceMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public IDataProvider Provider { get; }

        #endregion Public Properties

        #region Public Methods

        public EntityTemplateModel GetEntityTemplate(string id)
        {
            var entry = workspaceMan.UnitOfWork.GetRepository<IEntityTemplateEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Script error: " + id);

            return GetModel(entry);
        }

        #endregion Public Methods

        #region Private Methods

        private EntityTemplateModel GetModelImpl(IEntityTemplateFromFileEntry entry)
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