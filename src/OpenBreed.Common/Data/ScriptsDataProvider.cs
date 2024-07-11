using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Model.Scripts;
using OpenBreed.Model.Texts;
using System;

namespace OpenBreed.Common.Data
{
    public class ScriptsDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;

        private readonly IModelsProvider modelsProvider;

        #endregion Private Fields

        #region Public Constructors

        public ScriptsDataProvider(IModelsProvider modelsProvider, IRepositoryProvider repositoryProvider)
        {
            this.modelsProvider = modelsProvider;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public ScriptModel GetScript(IDbScript dbScript, bool refresh = false)
        {
            switch (dbScript)
            {
                case IDbScriptFromFile dbScriptFromFile:
                    return modelsProvider.GetModel<IDbScriptFromFile, ScriptModel>(dbScriptFromFile, refresh);
                case IDbScriptEmbedded dbScriptEmbedded:
                    return modelsProvider.GetModel<IDbScriptEmbedded, ScriptModel>(dbScriptEmbedded, refresh);
                default:
                    throw new NotImplementedException(dbScript.GetType().ToString());
            }
        }

        #endregion Public Methods
    }
}