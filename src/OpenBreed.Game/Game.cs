using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Xml;
using OpenBreed.Input.Interface;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Animation;
using OpenBreed.Wecs.Components.Animation.Xml;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Xml;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Components.Physics.Xml;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Components.Rendering.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System;
using System.Drawing;

namespace OpenBreed.Game
{
    internal class Game : CoreBase
    {
        private readonly IManagerCollection manCollection2;
        #region Private Fields

        private readonly IScriptMan scriptMan;
        private readonly LogConsolePrinter logConsolePrinter;
        private readonly IVariableMan variables;
        private readonly IModelsProvider modelsProvider;
        private readonly IRenderingMan renderingMan;
        private readonly IEntityMan entities;
        private readonly IInputsMan inputs;
        private readonly IAnimMan animations;
        private readonly IWorldMan worlds;
        private readonly IEntityFactory entityFactory;

        private readonly IPlayersMan players;

        #endregion Private Fields

        #region Public Constructors

        public Game(IManagerCollection manCollection) : base(manCollection)
        {
            this.manCollection2 = manCollection;
            this.scriptMan = manCollection.GetManager<IScriptMan>();
            this.variables = manCollection.GetManager<IVariableMan>();
            this.entities = manCollection.GetManager<IEntityMan>();
            this.inputs = manCollection.GetManager<IInputsMan>();
            this.animations = manCollection.GetManager<IAnimMan>();
            this.entityFactory = manCollection.GetManager<IEntityFactory>();
            this.worlds = manCollection.GetManager<IWorldMan>();
            this.players = manCollection.GetManager<IPlayersMan>();
            this.modelsProvider = manCollection.GetManager<IModelsProvider>();
            this.renderingMan = manCollection.GetManager<IRenderingMan>();

            logConsolePrinter = new LogConsolePrinter(Logging);
            logConsolePrinter.StartPrinting();
        }

        #endregion Public Constructors

        #region Public Properties

        public override JobMan Jobs => throw new NotImplementedException();
        public IFsmMan StateMachines => throw new NotImplementedException();

        #endregion Public Properties

        #region Internal Properties

        #endregion Internal Properties

        #region Public Methods

        public override void Exit()
        {
            manCollection2.GetManager<IViewClient>().Exit();
        }

        public override void Load()
        {
            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"D:\Projects\DB\Templates\Logo1.xml");

            var entity = entityFactory.Create(this, entityTemplate);

            var screenWorld = GetManager<ScreenWorldHelper>();

            renderingMan.ScreenWorld = screenWorld.CreateWorld();

            var gameWorldHelper = GetManager<GameWorldHelper>();

            gameWorldHelper.Create();

            var entryScript = manCollection2.GetManager<ScriptsDataProvider>().GetScript("Scripts.Entry.lua");

            var map = manCollection2.GetManager<MapsDataProvider>().GetMap("CRASH LANDING SITE");

            //var templateScript = dataProvider.EntityTemplates.GetEntityTemplate("EntityTemplates.Logo1.lua");

            scriptMan.RunString(entryScript.Script);
        }

        public void OnUpdateFrame(float dt)
        {
            Commands.ExecuteEnqueued(this);

            worlds.Cleanup();

            renderingMan.Cleanup();

            //Players.ResetInputs();

            inputs.Update();
            //Players.ApplyInputs();
            //StateMachine.Update((float)e.Time);
            worlds.Update(this, dt);
            //Jobs.Update(dt);
        }

        public override void Run()
        {
            ExposeScriptingApi();

            SpriteSystem.RegisterHandlers(Commands);
            TileSystem.RegisterHandlers(Commands);
            TextPresenterSystem.RegisterHandlers(Commands);
            ViewportSystem.RegisterHandlers(Commands);
            TextSystem.RegisterHandlers(Commands);

            manCollection2.GetManager<IViewClient>().Run();
        }

        #endregion Public Methods

        #region Private Methods

        private void ExposeScriptingApi()
        {
            //Scripts.RunFile(@"Content\Scripts\start.lua");
            scriptMan.RunFile(@"D:\Projects\DB\Templates\Logo1.lua");

            //var xmlTemplateReader = new XmlComponentReader(@"D:\Projects\DB\Templates\Logo1.xml");
        }

        #endregion Private Methods
    }
}