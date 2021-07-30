using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Actions
{
    public interface IDbActionSet : IDbEntry
    {

        #region Public Properties

        List<IDbAction> Actions { get; }

        #endregion Public Properties

        #region Public Methods

        IDbAction NewItem();

        #endregion Public Methods

    }
}
