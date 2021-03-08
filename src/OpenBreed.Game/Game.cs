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
        private readonly IDatabase database;
        private readonly IUnitOfWork unitOfWork;
        private readonly LogConsolePrinter logConsolePrinter;
        private readonly IVariableMan variables;
        private readonly IModelsProvider modelsProvider;
        private readonly IRenderingMan renderingMan;
        private readonly IEntityMan entities;
        private readonly IInputsMan inputs;
        private readonly IAnimMan animations;
        private readonly IWorldMan worlds;
        private readonly IViewClient clientMan;
        private readonly IEntityFactory entityFactory;

        private readonly IPlayersMan players;

        #endregion Private Fields

        #region Public Constructors

        public Game(IManagerCollection manCollection) : base(manCollection)
        {
            this.manCollection2 = manCollection;
            this.scriptMan = manCollection.GetManager<IScriptMan>();
            this.database = manCollection.GetManager<IDatabase>();
            this.variables = manCollection.GetManager<IVariableMan>();
            this.entities = manCollection.GetManager<IEntityMan>();
            this.inputs = manCollection.GetManager<IInputsMan>();
            this.animations = manCollection.GetManager<IAnimMan>();
            this.entityFactory = manCollection.GetManager<IEntityFactory>();
            this.worlds = manCollection.GetManager<IWorldMan>();
            this.players = manCollection.GetManager<IPlayersMan>();
            this.unitOfWork = manCollection.GetManager<IUnitOfWork>();
            this.modelsProvider = manCollection.GetManager<IModelsProvider>();
            this.renderingMan = manCollection.GetManager<IRenderingMan>();

            logConsolePrinter = new LogConsolePrinter(Logging);
            logConsolePrinter.StartPrinting();

            clientMan = new OpenTKWindowClient(800, 600, "OpenBreed");
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
            clientMan.Exit();
        }

        public override void Load()
        {
            RegisterXmlComponents();
            RegisterComponentFactories();

            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"D:\Projects\DB\Templates\Logo1.xml");

            var entity = entityFactory.Create(this, entityTemplate);

            renderingMan.ScreenWorld = ScreenWorldHelper.CreateWorld(this);

            GameWorldHelper.Create(this);

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

            clientMan.Run();
        }

        #endregion Public Methods

        #region Private Methods

        private void RegisterXmlComponents()
        {
            XmlComponentsList.RegisterComponentType<XmlPositionComponent>();
            XmlComponentsList.RegisterComponentType<XmlVelocityComponent>();
            XmlComponentsList.RegisterComponentType<XmlThrustComponent>();
            XmlComponentsList.RegisterComponentType<XmlSpriteComponent>();
            XmlComponentsList.RegisterComponentType<XmlTextComponent>();
            XmlComponentsList.RegisterComponentType<XmlAnimationComponent>();
            XmlComponentsList.RegisterComponentType<XmlBodyComponent>();
            XmlComponentsList.RegisterComponentType<XmlClassComponent>();
            XmlComponentsList.RegisterComponentType<XmlAngularPositionComponent>();
            XmlComponentsList.RegisterComponentType<XmlMotionComponent>();
            XmlComponentsList.RegisterComponentType<XmlTimerComponent>();
            XmlComponentsList.RegisterComponentType<XmlFsmComponent>();
        }

        private void RegisterComponentFactories()
        {
            entityFactory.RegisterComponentFactory<XmlPositionComponent>(new PositionComponentFactory(this));
            entityFactory.RegisterComponentFactory<XmlVelocityComponent>(new VelocityComponentFactory(this));
            entityFactory.RegisterComponentFactory<XmlThrustComponent>(new ThrustComponentFactory(this));
            entityFactory.RegisterComponentFactory<XmlSpriteComponent>(new SpriteComponentFactory(this));
            entityFactory.RegisterComponentFactory<XmlTextComponent>(new TextComponentFactory(this));
            entityFactory.RegisterComponentFactory<XmlAnimationComponent>(new AnimationComponentFactory(this));
            entityFactory.RegisterComponentFactory<XmlBodyComponent>(new BodyComponentFactory(this));
            entityFactory.RegisterComponentFactory<XmlClassComponent>(new ClassComponentFactory(this));
            entityFactory.RegisterComponentFactory<XmlAngularPositionComponent>(new AngularPositionComponentFactory(this));
            entityFactory.RegisterComponentFactory<XmlMotionComponent>(new MotionComponentFactory(this));
            entityFactory.RegisterComponentFactory<XmlTimerComponent>(new TimerComponentFactory(this));
            entityFactory.RegisterComponentFactory<XmlFsmComponent>(new FsmComponentFactory(this));
        }

        private void ExposeScriptingApi()
        {
            //Scripts.RunFile(@"Content\Scripts\start.lua");
            scriptMan.RunFile(@"D:\Projects\DB\Templates\Logo1.lua");

            //var xmlTemplateReader = new XmlComponentReader(@"D:\Projects\DB\Templates\Logo1.xml");
        }

        #endregion Private Methods
    }
}