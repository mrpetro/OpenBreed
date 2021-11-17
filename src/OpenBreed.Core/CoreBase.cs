using OpenBreed.Common;

namespace OpenBreed.Core
{
    public abstract class CoreBase : ICore
    {
        #region Protected Fields

        protected readonly IManagerCollection manCollection;

        #endregion Protected Fields

        #region Protected Constructors

        protected CoreBase(IManagerCollection manCollection)
        {
            this.manCollection = manCollection;
        }

        #endregion Protected Constructors

        #region Public Methods

        public abstract void Run();

        public abstract void Exit();

        public TManager GetManager<TManager>() => manCollection.GetManager<TManager>();

        #endregion Public Methods
    }
}