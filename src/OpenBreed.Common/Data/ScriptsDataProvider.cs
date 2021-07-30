using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Model.Scripts;
using System;

namespace OpenBreed.Common.Data
{
    public class ScriptsDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;

        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public ScriptsDataProvider(IModelsProvider dataProvider, IRepositoryProvider repositoryProvider)
        {
            this.dataProvider = dataProvider;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public ScriptModel GetScript(string id)
        {
            var entry = repositoryProvider.GetRepository<IDbScript>().GetById(id);
            if (entry == null)
                throw new Exception("Script error: " + id);

            return GetModel(entry);
        }

        #endregion Public Methods

        #region Private Methods

        private ScriptModel GetModelImpl(IDbScriptFromFile entry)
        {
            return ScriptsDataHelper.FromText(dataProvider, entry);
        }

        private ScriptModel GetModelImpl(IDbScriptEmbedded entry)
        {
            return ScriptsDataHelper.FromBinary(dataProvider, entry);
        }

        private ScriptModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        #endregion Private Methods
    }
}