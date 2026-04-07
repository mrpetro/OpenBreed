using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Abstractions
{
    public interface IUpdateContext
    {
        #region Public Properties

        IServiceProvider ServiceProvider { get; }

        #endregion Public Properties
    }
}