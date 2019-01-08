using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common
{
    public enum DatabaseMode
    {
        Create,
        Read,
        Update
    }

    public interface IDatabase
    {

        #region Public Properties

        string Name { get; }
        DatabaseMode Mode { get; }

        #endregion Public Properties

        #region Public Methods

        IUnitOfWork CreateUnitOfWork();

        void Save();

        #endregion Public Methods

    }
}
