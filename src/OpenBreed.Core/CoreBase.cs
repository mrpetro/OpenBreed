using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OpenBreed.Core
{
    public abstract class CoreBase : ICore
    {
        #region Private Fields

        private readonly Dictionary<Type, ICoreModule> modules = new Dictionary<Type, ICoreModule>();
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

        public abstract Rectangle ClientRectangle { get; }

        public abstract JobMan Jobs { get; }

        public ICoreClient Client { get; protected set; }

        public abstract Matrix4 ClientTransform { get; protected set; }

        public abstract float ClientRatio { get; }

        #endregion Public Properties

        #region Public Methods

        public abstract void Run();

        public abstract void Exit();

        public TManager GetManager<TManager>() => manCollection.GetManager<TManager>();

        public T GetModule<T>() where T : ICoreModule
        {
            return (T)modules[typeof(T)];
        }

        public abstract void Load();

        public abstract void Update(float dt);

        public bool ExecuteCommand(ICommand cmd)
        {
            switch (cmd.Name)
            {
                default:
                    return false;
            }
        }


        #endregion Public Methods

        #region Protected Methods

        protected void RegisterModule<TModule>(ICoreModule module) where TModule : ICoreModule
        {
            modules.Add(typeof(TModule), module);
        }

        #endregion Protected Methods
    }
}