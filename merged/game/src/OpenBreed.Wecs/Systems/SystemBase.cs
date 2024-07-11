using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems
{
    public abstract class SystemBase<TSystem> : SystemBase where TSystem : ISystem
    {
        #region Protected Constructors

        protected SystemBase()
        {
        }

        #endregion Protected Constructors
    }

    public abstract class SystemBase : ISystem
    {
        #region Protected Constructors

        protected SystemBase()
        {
        }

        #endregion Protected Constructors
    }
}