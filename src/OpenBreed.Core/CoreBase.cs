using Microsoft.Extensions.Hosting;
using OpenBreed.Common;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Core.Abstractions;

namespace OpenBreed.Core
{
    public abstract class CoreBase : ICore
    {
        #region Protected Fields

        protected readonly IHost host;

        #endregion Protected Fields

        #region Protected Constructors

        protected CoreBase(IHost host)
        {
            this.host = host;
        }

        #endregion Protected Constructors

        #region Public Methods

        public abstract void Run();

        public abstract void Exit();

        #endregion Public Methods
    }
}