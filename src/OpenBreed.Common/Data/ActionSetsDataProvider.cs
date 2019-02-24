using OpenBreed.Common.Actions;
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

        public IActionSetEntry GetActionSet(string id)
        {
            var entry = Provider.UnitOfWork.GetRepository<IActionSetEntry>().GetById(id);
            if (entry == null)
                throw new Exception("ActionSet error: " + id);

            return entry;
        }

        #endregion Public Methods

    }
}
