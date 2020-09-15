using OpenBreed.Model.Scripts;
using OpenBreed.Model.Texts;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Interface.Items.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    public class ScriptsDataProvider
    {
        #region Public Constructors

        public ScriptsDataProvider(DataProvider provider)
        {
            Provider = provider;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties

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

        public ScriptModel GetScript(string id)
        {
            var entry = Provider.UnitOfWork.GetRepository<IScriptEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Script error: " + id);

            return GetModel(entry);
        }
    }
}

