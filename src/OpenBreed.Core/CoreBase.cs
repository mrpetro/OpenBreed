using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Managers;

namespace OpenBreed.Core
{
    public abstract class CoreBase : ICore
    {
        #region Private Fields

        private readonly IManagerCollection manCollection;

        #endregion Private Fields

        #region Protected Constructors

        protected CoreBase(IManagerCollection manCollection)
        {
            this.manCollection = manCollection;
            manCollection.AddSingleton<ICore>(this);

            Commands = manCollection.GetManager<ICommandsMan>();
            Events = manCollection.GetManager<IEventsMan>();
            Logging = manCollection.GetManager<ILogger>();
        }

        #endregion Protected Constructors

        #region Public Properties

        public ICommandsMan Commands { get; }
        public IEventsMan Events { get; }
        public ILogger Logging { get; }

        public abstract JobMan Jobs { get; }

        #endregion Public Properties

        #region Public Methods

        public abstract void Run();

        public abstract void Exit();

        public TManager GetManager<TManager>() => manCollection.GetManager<TManager>();

        public abstract void Load();

        #endregion Public Methods
    }
}