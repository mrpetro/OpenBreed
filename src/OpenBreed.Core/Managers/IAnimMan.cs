using OpenBreed.Core.Modules.Animation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Managers
{
    public interface IAnimMan
    {
        #region Public Methods

        IAnimation GetById(int id);
        IAnimation GetByName(string name);

        IAnimation<T> Create<T>(string name);

        #endregion Public Methods
    }
}
