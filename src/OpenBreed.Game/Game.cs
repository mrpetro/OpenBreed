using OpenBreed.Common;
using OpenBreed.Core;
using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Systems;
using OpenTK;
using System;

namespace OpenBreed.Game
{
    internal class Game : ICore
    {
        #region Private Fields

        private IDatabase database;
        private IUnitOfWork unitOfWork;

        #endregion Private Fields

        #region Public Constructors

        public Game(IDatabase database)
        {
            this.database = database;
        }

        #endregion Public Constructors

        #region Public Properties

        public IRenderModule Rendering => throw new NotImplementedException();

        public IAudioModule Sounds => throw new NotImplementedException();

        public AnimMan Animations => throw new NotImplementedException();

        public ILogMan Logging => throw new NotImplementedException();

        public JobMan Jobs => throw new NotImplementedException();

        public EntityMan Entities => throw new NotImplementedException();

        public FsmMan StateMachines => throw new NotImplementedException();

        public PlayersMan Players => throw new NotImplementedException();

        public ItemsMan Items => throw new NotImplementedException();

        public InputsMan Inputs => throw new NotImplementedException();

        public WorldMan Worlds => throw new NotImplementedException();

        public CommandsMan Commands => throw new NotImplementedException();

        public EventsMan Events => throw new NotImplementedException();

        public IScriptMan Scripts => throw new NotImplementedException();

        public Matrix4 ClientTransform => throw new NotImplementedException();

        public System.Drawing.Rectangle ClientRectangle => throw new NotImplementedException();

        public float ClientRatio => throw new NotImplementedException();

        #endregion Public Properties

        #region Public Methods

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public TBuilder GetBuilder<TBuilder>() where TBuilder : IComponentBuilder
        {
            throw new NotImplementedException();
        }

        public T GetModule<T>() where T : ICoreModule
        {
            throw new NotImplementedException();
        }

        public T GetSystemByEntityId<T>(int entityId) where T : IWorldSystem
        {
            throw new NotImplementedException();
        }

        public T GetSystemByWorldId<T>(int worldId) where T : IWorldSystem
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            unitOfWork = database.CreateUnitOfWork();




            //MainLoophere
        }

        #endregion Public Methods
    }
}