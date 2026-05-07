using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Abstractions.Services
{
    public delegate void EntityClassLoadCallback(string id, string parentId);

    public interface IEntityClassesLoader
    {
        #region Public Methods

        void Load(EntityClassLoadCallback callback);

        #endregion Public Methods
    }
}