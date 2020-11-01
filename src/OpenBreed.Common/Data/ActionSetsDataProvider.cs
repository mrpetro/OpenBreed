
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Model.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    public class ActionSetsDataProvider
    {

        #region Public Constructors

        public ActionSetsDataProvider(DataProvider provider)
        {
            Provider = provider;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties

        #region Public Methods

        private ActionSetModel GetModelImpl(IActionSetEntry entry)
        {
            return ActionSetsDataHelper.FromEmbeddedData(Provider, entry);
        }

        private ActionSetModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        public ActionSetModel GetActionSet(string id)
        {
            var entry = Provider.GetRepository<IActionSetEntry>().GetById(id);
            if (entry == null)
                throw new Exception("ActionSet error: " + id);

            return GetModel(entry);
        }

        #endregion Public Methods

    }
}
