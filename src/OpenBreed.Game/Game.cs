using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools;
using OpenBreed.Core;
using OpenBreed.Core.Builders;
using OpenBreed.Core.Components;
using OpenBreed.Core.Components.Xml;
using OpenBreed.Core.Entities.Xml;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Components.Xml;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Components.Xml;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Database.Interface;
using OpenBreed.Model.Scripts;
using OpenBreed.Model.Texts;
using OpenTK;
using System;
using System.Drawing;
using System.Xml;

namespace OpenBreed.Game
{
    internal class Game : CoreBase
    {
        #region Private Fields

        private readonly IDatabase database;
        private readonly IUnitOfWork unitOfWork;
        private readonly LogConsolePrinter logConsolePrinter;
        private readonly IVariableMan variables;
        private readonly DataProvider dataProvider;
        #endregion Private Fields

        #region Public Constructors

        internal VideoSystemsFactory VideoSystemsFactory { get; }

        public Game(IManagerCollection manCollection, ICoreModulesFactory modulesFactory) : 
            base(manCollection)
        {
            this.database = manCollection.GetManager<IDatabase>();
            this.variables = manCollection.GetManager<IVariableMan>();
            Logging = manCollection.GetManager<ILogger>();
            logConsolePrinter = new LogConsolePrinter(Logging);
            logConsolePrinter.StartPrinting();

            Sounds = modulesFactory.CreateAudioModule(this);
            Rendering = modulesFactory.CreateVideoModule(this);
            Scripts = new LuaScriptMan(this);

            VideoSystemsFactory = new VideoSystemsFactory(this);

            Inputs = new InputsMan(this);
            EntityFactory = new EntityFactory(this);
            Animations = new AnimMan(this);

            this.unitOfWork = this.database.CreateUnitOfWork();
            this.dataProvider = new DataProvider(unitOfWork, Logging, this.variables);
            Client = new GameWindowClient(this, 800, 600, "OpenBreed");
        }

        #endregion Public Constructors

        #region Public Properties

        public override IRenderModule Rendering { get; }

        public override EntityFactory EntityFactory { get; }

        public override IAudioModule Sounds { get; }

        public override AnimMan Animations { get; }

        public override ILogger Logging { get; }

        public override JobMan Jobs => throw new NotImplementedException();

        public override FsmMan StateMachines => throw new NotImplementedException();

        public override PlayersMan Players => throw new NotImplementedException();

        public override ItemsMan Items => throw new NotImplementedException();

        public override InputsMan Inputs { get; }

        public override IScriptMan Scripts { get; }

        public override Matrix4 ClientTransform
        {
            get => Client.ClientTransform;
            protected set => ClientTransform = value;
        }

        public override float ClientRatio => Client.ClientRatio;

        public override Rectangle ClientRectangle => Client.ClientRectangle;

        #endregion Public Properties

        #region Public Methods

        public override void Exit()
        {
            Client.Exit();
        }

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

            
            Scripts.RunString(entryScript.Script);
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

        private void ExposeScriptingApi()
        {
            Scripts.Expose("Worlds", Worlds);
            Scripts.Expose("Entities", Entities);
            Scripts.Expose("Commands", Commands);
            Scripts.Expose("Inputs", Inputs);
            Scripts.Expose("Logging", Logging);
            Scripts.Expose("DataProvider", dataProvider);
            //Scripts.Expose("Players", Players);

            //Scripts.RunFile(@"Content\Scripts\start.lua");
            Scripts.RunFile(@"D:\Projects\DB\Templates\Logo1.lua");


            //var xmlTemplateReader = new XmlComponentReader(@"D:\Projects\DB\Templates\Logo1.xml");
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
    }
}