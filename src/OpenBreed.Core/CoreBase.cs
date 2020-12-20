using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Builders;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Components;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Systems;
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
            manCollection.AddSingleton<ICore>(this);

            Commands = manCollection.GetManager<ICommandsMan>();
            Events = new EventsMan(this);
            Entities = manCollection.GetManager<IEntityMan>();
            Worlds = new WorldMan(this);
            this.manCollection = manCollection;
        }

        #endregion Protected Constructors

        #region Public Properties

        public IEntityMan Entities { get; }
        public ICommandsMan Commands { get; }
        public EventsMan Events { get; }
        public WorldMan Worlds { get; }

        public abstract Rectangle ClientRectangle { get; }

        public abstract IRenderModule Rendering { get; }

        public abstract IAudioModule Sounds { get; }

        public abstract AnimMan Animations { get; }

        public abstract ILogger Logging { get; }

        public abstract JobMan Jobs { get; }

        public abstract FsmMan StateMachines { get; }

        public abstract EntityFactory EntityFactory { get; }

        public abstract PlayersMan Players { get; }

        public abstract ItemsMan Items { get; }

        public abstract InputsMan Inputs { get; }

        public abstract IScriptMan Scripts { get; }

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

        public TBuilder GetBuilder<TBuilder>() where TBuilder : IComponentBuilder
        {
            throw new NotImplementedException();
        }

        public T GetSystemByEntityId<T>(int entityId) where T : IWorldSystem
        {
            var entity = Entities.GetById(entityId);
            if (entity.World == null)
                return default(T);
            var system = entity.World.GetSystem<T>();
            if (system == null)
                return default(T);

            return system;
        }

        public T GetSystemByWorldId<T>(int worldId) where T : IWorldSystem
        {
            var world = Worlds.GetById(worldId);
            if (world == null)
                return default(T);
            var system = world.GetSystem<T>();
            if (system == null)
                return default(T);

            return system;
        }

        #endregion Public Methods

        #region Protected Methods

        protected void RegisterModule(ICoreModule module)
        {
            modules.Add(module.GetType(), module);
        }

        #endregion Protected Methods
    }
}