using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools;
using OpenBreed.Core;
using OpenBreed.Core.Components;
using OpenBreed.Core.Components.Xml;
using OpenBreed.Core.Entities.Xml;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Components.Xml;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Components.Xml;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Database.Interface;
using OpenTK;
using System;
using System.Drawing;

namespace OpenBreed.Game
{
    internal class Game : CoreBase
    {
        #region Private Fields

        private readonly IScriptMan scriptMan;
        private readonly IDatabase database;
        private readonly IUnitOfWork unitOfWork;
        private readonly LogConsolePrinter logConsolePrinter;
        private readonly IVariableMan variables;
        private readonly DataProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public Game(IManagerCollection manCollection, ICoreModulesFactory modulesFactory) :
            base(manCollection)
        {
            scriptMan = manCollection.GetManager<IScriptMan>();
            this.database = manCollection.GetManager<IDatabase>();
            this.variables = manCollection.GetManager<IVariableMan>();
            Inputs = manCollection.GetManager<IInputsMan>();
            logConsolePrinter = new LogConsolePrinter(Logging);
            logConsolePrinter.StartPrinting();

            Animations = manCollection.GetManager<IAnimMan>();

            Sounds = modulesFactory.CreateAudioModule(this);
            Rendering = modulesFactory.CreateVideoModule(this);

            VideoSystemsFactory = new VideoSystemsFactory(this);

            Inputs = new InputsMan(this);
            EntityFactory = new EntityFactory(this);

            this.unitOfWork = this.database.CreateUnitOfWork();
            this.dataProvider = new DataProvider(unitOfWork, Logging, this.variables);
            Client = new GameWindowClient(this, 800, 600, "OpenBreed");
        }

        #endregion Public Constructors

        #region Public Properties

        public override IRenderModule Rendering { get; }
        public override EntityFactory EntityFactory { get; }
        public override IAudioModule Sounds { get; }
        public override IAnimMan Animations { get; }
        public override JobMan Jobs => throw new NotImplementedException();
        public override IFsmMan StateMachines => throw new NotImplementedException();
        public override IPlayersMan Players => throw new NotImplementedException();
        public override IInputsMan Inputs { get; }

        public override Matrix4 ClientTransform
        {
            get => Client.ClientTransform;
            protected set => ClientTransform = value;
        }

        public override float ClientRatio => Client.ClientRatio;
        public override Rectangle ClientRectangle => Client.ClientRectangle;

        #endregion Public Properties

        #region Internal Properties

        internal VideoSystemsFactory VideoSystemsFactory { get; }

        #endregion Internal Properties

        #region Public Methods

        public override void Exit()
        {
            Client.Exit();
        }

        public override void Load()
        {
            RegisterXmlComponents();
            RegisterComponentFactories();

            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"D:\Projects\DB\Templates\Logo1.xml");

            var entity = EntityFactory.Create(entityTemplate);

            Rendering.ScreenWorld = ScreenWorldHelper.CreateWorld(this);

            GameWorldHelper.Create(this);

            var entryScript = dataProvider.Scripts.GetScript("Scripts.Entry.lua");

            //var templateScript = dataProvider.EntityTemplates.GetEntityTemplate("EntityTemplates.Logo1.lua");

            scriptMan.RunString(entryScript.Script);
        }

        public override void Update(float dt)
        {
            Commands.ExecuteEnqueued();

            Worlds.Cleanup();

            Rendering.Cleanup();

            //Players.ResetInputs();

            Inputs.Update();
            //Players.ApplyInputs();
            //StateMachine.Update((float)e.Time);
            Worlds.Update(dt);
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

            Client.Run();
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
            EntityFactory.RegisterComponentFactory<XmlPositionComponent>(new PositionComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlVelocityComponent>(new VelocityComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlThrustComponent>(new ThrustComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlSpriteComponent>(new SpriteComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlTextComponent>(new TextComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlAnimationComponent>(new AnimationComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlBodyComponent>(new BodyComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlClassComponent>(new ClassComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlAngularPositionComponent>(new AngularPositionComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlMotionComponent>(new MotionComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlTimerComponent>(new TimerComponentFactory(this));
            EntityFactory.RegisterComponentFactory<XmlFsmComponent>(new FsmComponentFactory(this));
        }

        private void ExposeScriptingApi()
        {
            scriptMan.Expose("Worlds", Worlds);
            scriptMan.Expose("Entities", Entities);
            scriptMan.Expose("Commands", Commands);
            scriptMan.Expose("Inputs", Inputs);
            scriptMan.Expose("Logging", Logging);
            scriptMan.Expose("DataProvider", dataProvider);
            //Scripts.Expose("Players", Players);

            //Scripts.RunFile(@"Content\Scripts\start.lua");
            scriptMan.RunFile(@"D:\Projects\DB\Templates\Logo1.lua");

            //var xmlTemplateReader = new XmlComponentReader(@"D:\Projects\DB\Templates\Logo1.xml");
        }

        #endregion Private Methods
    }
}