using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Database.Interface;
using OpenBreed.Model.Scripts;
using OpenTK;
using System;
using System.Drawing;

namespace OpenBreed.Game
{
    internal class Game : CoreBase
    {
        #region Private Fields

        private IDatabase database;
        private IUnitOfWork unitOfWork;
        private readonly LogConsolePrinter logConsolePrinter;

        #endregion Private Fields

        #region Public Constructors

        public Game(IDatabase database)
        {
            this.database = database;

            Logging = new DefaultLogger();
            logConsolePrinter = new LogConsolePrinter(Logging);
            logConsolePrinter.StartPrinting();

            Client = new GameWindowClient(this, 800, 600, "OpenBreed");
        }

        #endregion Public Constructors

        #region Public Properties

        public override IRenderModule Rendering => throw new NotImplementedException();

        public override IAudioModule Sounds => throw new NotImplementedException();

        public override AnimMan Animations => throw new NotImplementedException();

        public override ILogger Logging { get; }

        public override JobMan Jobs => throw new NotImplementedException();

        public override FsmMan StateMachines => throw new NotImplementedException();

        public override PlayersMan Players => throw new NotImplementedException();

        public override ItemsMan Items => throw new NotImplementedException();

        public override InputsMan Inputs => throw new NotImplementedException();

        public override IScriptMan Scripts => throw new NotImplementedException();

        public override Matrix4 ClientTransform { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        public override float ClientRatio => throw new NotImplementedException();

        public override Rectangle ClientRectangle => throw new NotImplementedException();

        #endregion Public Properties

        #region Public Methods

        public override void Exit()
        {
            throw new NotImplementedException();
        }

        public override void Update(float dt)
        {
            throw new NotImplementedException();
        }

        public override void Run()
        {
            var unitOfWork = database.CreateUnitOfWork();
            var provider = new DataProvider(unitOfWork, Logging);

            if (provider.TryGetData<ScriptModel>("Entry.lua", out ScriptModel entryScript, out string msg))
            {
            }

            //MainLoophere
        }

        #endregion Public Methods
    }
}