using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Model.Scripts;
using OpenBreed.Model.Texts;
using OpenBreed.Scripting.Interface;
using System;

namespace OpenBreed.Scripting.Lua
{
    internal class ScriptDataLoader : IScriptDataLoader
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly ScriptsDataProvider scriptsDataProvider;
        private readonly IScriptMan scriptMan;
        #endregion Private Fields

        #region Public Constructors

        public ScriptDataLoader(
            IRepositoryProvider repositoryProvider,
            ScriptsDataProvider scriptsDataProvider,
            IScriptMan scriptMan)
        {
            this.repositoryProvider = repositoryProvider;
            this.scriptsDataProvider = scriptsDataProvider;
            this.scriptMan = scriptMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public IScriptFunc Load(string entryId, params object[] args)
        {
            var entry = repositoryProvider.GetRepository<IDbScript>().GetById(entryId);
            if (entry is null)
            {
                throw new Exception("Script error: " + entryId);
            }

            var model = scriptsDataProvider.GetScript(entry);


            return scriptMan.CompileString(model.Script, entryId);
        }

        public object LoadObject(string entryId) => Load(entryId);

        #endregion Public Methods
    }
}