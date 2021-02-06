using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Model.Scripts;
using System;

namespace OpenBreed.Common.Data
{
    public class ScriptsDataProvider
    {
        #region Private Fields

        private readonly IUnitOfWork unitOfWork;

        #endregion Private Fields

        #region Public Constructors

        public ScriptsDataProvider(DataProvider provider, IUnitOfWork unitOfWork)
        {
            Provider = provider;
            this.unitOfWork = unitOfWork;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties

        #region Public Methods

        public ScriptModel GetScript(string id)
        {
            var entry = unitOfWork.GetRepository<IScriptEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Script error: " + id);

            return GetModel(entry);
        }

        #endregion Public Methods

        #region Private Methods

        private ScriptModel GetModelImpl(IScriptFromFileEntry entry)
        {
            return ScriptsDataHelper.FromText(Provider, entry);
        }

        private ScriptModel GetModelImpl(IScriptEmbeddedEntry entry)
        {
            return ScriptsDataHelper.FromBinary(Provider, entry);
        }

        private ScriptModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        #endregion Private Methods
    }
}