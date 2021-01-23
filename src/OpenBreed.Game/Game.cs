using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools;
using OpenBreed.Components.Physics;
using OpenBreed.Components.Physics.Xml;
using OpenBreed.Core;
using OpenBreed.Components.Common;
using OpenBreed.Components.Common.Xml;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Database.Interface;
using OpenBreed.Components.Rendering;
using OpenBreed.Components.Rendering.Xml;
using OpenBreed.Rendering.OpenGL;
using OpenBreed.Systems.Rendering;
using OpenTK;
using System;
using System.Drawing;
using OpenBreed.Audio.Interface;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Components.Animation.Xml;
using OpenBreed.Components.Animation;
using OpenBreed.Input.Generic;
using OpenBreed.Input.Interface;
using OpenBreed.Scripting.Interface;
using OpenBreed.Ecsw.Entities.Xml;
using OpenBreed.Ecsw;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Xml;
using OpenBreed.Ecsw.Entities;
using OpenBreed.Ecsw.Worlds;
using OpenBreed.Ecsw.Components.Xml;

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

        public Game(IManagerCollection manCollection) :
            base(manCollection)
        {
            scriptMan = manCollection.GetManager<IScriptMan>();
            this.database = manCollection.GetManager<IDatabase>();
            this.variables = manCollection.GetManager<IVariableMan>();
            Inputs = manCollection.GetManager<IInputsMan>();
            logConsolePrinter = new LogConsolePrinter(Logging);
            logConsolePrinter.StartPrinting();

            Animations = manCollection.GetManager<IAnimMan>();

            soundModule = new OpenALModule(this);
            renderingModule = new OpenGLModule(this);

            VideoSystemsFactory = new VideoSystemsFactory(this);
            PhysicsSystemsFactory = new PhysicsSystemsFactory(this);

            Inputs = new InputsMan(this);
            EntityFactory = new EntityFactory(Entities);

            this.unitOfWork = this.database.CreateUnitOfWork();
            this.dataProvider = new DataProvider(unitOfWork, Logging, this.variables);
            Client = new GameWindowClient(this, 800, 600, "OpenBreed");
        }

        #endregion Public Constructors

        #region Public Properties

        public EntityFactory EntityFactory { get; }

        private readonly OpenALModule soundModule;
        private readonly OpenGLModule renderingModule;

        public IAnimMan Animations { get; }
        public override JobMan Jobs => throw new NotImplementedException();
        public IFsmMan StateMachines => throw new NotImplementedException();
        public IPlayersMan Players => throw new NotImplementedException();
        public IEntityMan Entities => throw new NotImplementedException();
        public IWorldMan Worlds => throw new NotImplementedException();
        public IInputsMan Inputs { get; }

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
        internal PhysicsSystemsFactory PhysicsSystemsFactory { get; }

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

            renderingModule.ScreenWorld = ScreenWorldHelper.CreateWorld(this);

            GameWorldHelper.Create(this);

            var entryScript = dataProvider.Scripts.GetScript("Scripts.Entry.lua");

            //var templateScript = dataProvider.EntityTemplates.GetEntityTemplate("EntityTemplates.Logo1.lua");

            scriptMan.RunString(entryScript.Script);
        }

        public override void Update(float dt)
        {
            Commands.ExecuteEnqueued();

            Worlds.Cleanup();

            renderingModule.Cleanup();

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