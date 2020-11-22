using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface
{
    public enum DatabaseMode
    {   
        Unset,
        Create,
        ReadOnly,
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
