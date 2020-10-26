﻿using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Model.Scripts;
using OpenBreed.Model.Texts;
using OpenBreed.Core;
using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Systems;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Drawing;
using OpenBreed.Common.Logging;

namespace OpenBreed.Game
{
    internal class Game : CoreBase
    {


        #region Private Fields

        private IDatabase database;
        private IUnitOfWork unitOfWork;

        #endregion Private Fields

        #region Public Constructors

        public Game(IDatabase database)// : 
           // base(800, 600, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 8), "OpenBreed")
        {
            this.database = database;
        }

        public override IRenderModule Rendering => throw new NotImplementedException();

        public override IAudioModule Sounds => throw new NotImplementedException();

        public override AnimMan Animations => throw new NotImplementedException();

        public override ILogger Logging => throw new NotImplementedException();

        public override JobMan Jobs => throw new NotImplementedException();

        public override FsmMan StateMachines => throw new NotImplementedException();

        public override PlayersMan Players => throw new NotImplementedException();

        public override ItemsMan Items => throw new NotImplementedException();

        public override InputsMan Inputs => throw new NotImplementedException();

        public override IScriptMan Scripts => throw new NotImplementedException();

        public override Matrix4 ClientTransform { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        public override float ClientRatio => throw new NotImplementedException();

        public override Rectangle ClientRectangle => throw new NotImplementedException();

        public override void Exit()
        {
            throw new NotImplementedException();
        }

        #endregion Public Constructors

        #region Public Properties

        public override void Update(float dt)
        {
            throw new NotImplementedException();
        }

        public override void Run()
        {
            var unitOfWork = database.CreateUnitOfWork();
            var provider = new DataProvider(unitOfWork, Logging);

            if (provider.TryGetData<ScriptModel>("Entry", out ScriptModel entryScript, out string msg))
            {
            }


            //MainLoophere
        }

        #endregion Public Methods
    }
}