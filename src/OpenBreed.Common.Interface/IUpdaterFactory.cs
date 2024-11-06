using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface
{
    public interface IUpdater
    {
    }

    public interface IUpdaterFactory
    {
        #region Public Methods

        IUpdater CreateUpdater(float dt, Action<float> updateCallback);

        #endregion Public Methods
    }
}