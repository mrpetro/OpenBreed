using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Abstractions
{
    public interface ISystemInitializer
    {
        #region Public Methods

        void Initialize(IServiceProvider serviceProvider, ISystem system);

        #endregion Public Methods
    }
}