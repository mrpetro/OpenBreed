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

        private readonly List<ISystem> systems = new List<ISystem>();
        private CommandHandler commandHandler;
        private MsgHandlerRelay msgHandlerRelay;

        private Dictionary<Type, ICoreModule> modules = new Dictionary<Type, ICoreModule>();

        #endregion Private Fields

        #region Protected Constructors

        protected CoreBase(int width, int height, GraphicsMode mode, string title) : base(width, height, mode, title)
        {
            commandHandler = new CommandHandler(this);
            msgHandlerRelay = new MsgHandlerRelay(commandHandler);

            Systems = new ReadOnlyCollection<ISystem>(systems);

            RegisterHandler(PauseWorldCommand.TYPE, commandHandler);
            RegisterHandler(RemoveEntityCommand.TYPE, commandHandler);
            RegisterHandler(AddEntityCommand.TYPE, commandHandler);
        }

        #endregion Protected Constructors

        #region Public Properties

        public ReadOnlyCollection<ISystem> Systems { get; }

        public abstract IRenderModule Rendering { get; }

        public abstract IAudioModule Sounds { get; }

        public abstract AnimMan Animations { get; }

        public abstract ILogMan Logging { get; }

        public abstract JobMan Jobs { get; }

        public abstract EntityMan Entities { get; }

        public abstract FsmMan StateMachines { get; }

        public abstract PlayersMan Players { get; }

        public abstract ItemsMan Items { get; }

        public abstract InputsMan Inputs { get; }

        public abstract WorldMan Worlds { get; }

        public abstract CommandsMan Commands { get; }

        public abstract EventsMan Events { get; }

        public abstract IScriptMan Scripts { get; }

        public abstract Matrix4 ClientTransform { get; protected set; }

        public abstract float ClientRatio { get; }

        #endregion Public Properties

        #region Public Methods

        public void RegisterHandler(string msgType, IMsgHandler msgHandler)
        {
            msgHandlerRelay.RegisterHandler(msgType, msgHandler);
        }

        public T GetModule<T>() where T : ICoreModule
        {
            return (T)modules[typeof(T)];
        }

        public bool CanHandle(string msgType)
        {
            return msgHandlerRelay.IsRegistered(msgType);
        }

        public bool HandleCmd(IMsg msg)
        {
            return msgHandlerRelay.Handle(msg);
        }

        public bool ExecuteCommand(ICommand cmd)
        {
            switch (cmd.Type)
            {
                case PauseWorldCommand.TYPE:
                    return HandlePauseWorld((PauseWorldCommand)cmd);

                case RemoveEntityCommand.TYPE:
                    return HandleRemoveEntity((RemoveEntityCommand)cmd);

                case AddEntityCommand.TYPE:
                    return HandleAddEntity((AddEntityCommand)cmd);

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

        private bool HandleRemoveEntity(RemoveEntityCommand cmd)
        {
            var world = Worlds.GetById(cmd.WorldId);
            var entity = Entities.GetById(cmd.EntityId);
            world.RemoveEntity(entity);
            return true;
        }

        private bool HandleAddEntity(AddEntityCommand cmd)
        {


            var world = Worlds.GetById(cmd.WorldId);
            var entity = Entities.GetById(cmd.EntityId);
            world.AddEntity(entity);
            return true;
        }

        private bool HandlePauseWorld(PauseWorldCommand cmd)
        {
            var world = Worlds.GetById(cmd.WorldId);
            world.Pause(cmd.Pause);
            return true;
        }

        #endregion Private Methods
    }
}