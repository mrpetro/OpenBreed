using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Model.Actions;
using System;

namespace OpenBreed.Common.Data
{
    public class ActionSetsDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;

        #endregion Private Fields

        #region Public Constructors

        public ActionSetsDataProvider(IModelsProvider modelsProvider, IRepositoryProvider repositoryProvider)
        {
            Provider = modelsProvider;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Public Properties

        public IModelsProvider Provider { get; }

        #endregion Public Properties

        #region Public Methods

        public ActionSetModel GetActionSet(string id)
        {
            var entry = repositoryProvider.GetRepository<IActionSetEntry>().GetById(id);
            if (entry == null)
                throw new Exception("ActionSet error: " + id);

            return GetModel(entry);
        }

        #endregion Public Methods

        #region Private Methods

        private ActionSetModel GetModelImpl(IActionSetEntry entry)
        {
            return ActionSetsDataHelper.FromEmbeddedData(Provider, entry);
        }

        private ActionSetModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        #endregion Private Methods
    }
}