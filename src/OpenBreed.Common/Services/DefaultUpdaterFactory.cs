using OpenBreed.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Services
{
    public class DefaultUpdaterFactory : IUpdaterFactory
    {
        #region Public Methods

        public IUpdater CreateUpdater(float ups, Action<float> updateCallback)
        {
            return new DefaultUpdater(ups, updateCallback);
        }

        #endregion Public Methods
    }
}
