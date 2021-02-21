using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Model.Scripts;
using System;

namespace OpenBreed.Common.Data
{
    public class ScriptsDataProvider
    {
        #region Private Fields

        private readonly IWorkspaceMan workspaceMan;

        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public ScriptsDataProvider(IModelsProvider dataProvider, IWorkspaceMan workspaceMan)
        {
            this.dataProvider = dataProvider;
            this.workspaceMan = workspaceMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public ScriptModel GetScript(string id)
        {
            var entry = workspaceMan.UnitOfWork.GetRepository<IScriptEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Script error: " + id);

            return GetModel(entry);
        }

        #endregion Public Methods

        #region Private Methods

        private ScriptModel GetModelImpl(IScriptFromFileEntry entry)
        {
            return ScriptsDataHelper.FromText(dataProvider, entry);
        }

        private ScriptModel GetModelImpl(IScriptEmbeddedEntry entry)
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