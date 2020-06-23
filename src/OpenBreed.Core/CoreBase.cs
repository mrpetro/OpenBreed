using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Systems;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenBreed.Core
{
    public abstract class CoreBase : GameWindow, ICore, ICommandExecutor
    {
        #region Private Fields

        private CommandHandler commandHandler;
        private MsgHandlerRelay msgHandlerRelay;

        private Dictionary<Type, ICoreModule> modules = new Dictionary<Type, ICoreModule>();

        #endregion Private Fields

        #region Protected Constructors

        protected CoreBase(int width, int height, GraphicsMode mode, string title) : base(width, height, mode, title)
        {
            Commands = new CommandsMan(this);
            Events = new EventsMan(this);
            Entities = new EntityMan(this);
            Worlds = new WorldMan(this);

            commandHandler = new CommandHandler(this);
            msgHandlerRelay = new MsgHandlerRelay(commandHandler);

            //RegisterHandler(PauseWorldCommand.TYPE, commandHandler);
            //RegisterHandler(RemoveEntityCommand.TYPE, commandHandler);
            //RegisterHandler(AddEntityCommand.TYPE, commandHandler);
        }

        #endregion Protected Constructors

        #region Public Properties

        public EntityMan Entities { get; }
        public CommandsMan Commands { get; }
        public EventsMan Events { get; }
        public WorldMan Worlds { get; }

        public abstract IRenderModule Rendering { get; }

        public abstract IAudioModule Sounds { get; }

        public abstract AnimMan Animations { get; }

        public abstract ILogMan Logging { get; }

        public abstract JobMan Jobs { get; }

        public abstract FsmMan StateMachines { get; }

        public abstract PlayersMan Players { get; }

        public abstract ItemsMan Items { get; }

        public abstract InputsMan Inputs { get; }

        public abstract IScriptMan Scripts { get; }

        public abstract Matrix4 ClientTransform { get; protected set; }

        public abstract float ClientRatio { get; }

        #endregion Public Properties

        #region Public Methods

        public T GetModule<T>() where T : ICoreModule
        {
            return (T)modules[typeof(T)];
        }

        public bool HandleCmd(IMsg msg)
        {
            return msgHandlerRelay.Handle(msg);
        }

        public bool ExecuteCommand(ICommand cmd)
        {
            switch (cmd.Type)
            {
                default:
                    return false;
            }
        }

        public TBuilder GetBuilder<TBuilder>() where TBuilder : IComponentBuilder
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Protected Methods

        protected void RegisterModule(ICoreModule module)
        {
            modules.Add(module.GetType(), module);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            commandHandler.ExecuteEnqueued();
        }

        #endregion Protected Methods

        #region Private Methods


        #endregion Private Methods
    }
}