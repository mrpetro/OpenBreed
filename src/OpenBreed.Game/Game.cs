using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Input.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Worlds;
using System;

namespace OpenBreed.Game
{
    internal class Game : CoreBase
    {
        #region Private Fields

        private readonly IManagerCollection manCollection2;

        private readonly IScriptMan scriptMan;
        private readonly LogConsolePrinter logConsolePrinter;
        private readonly IRenderingMan renderingMan;
        private readonly IInputsMan inputs;
        private readonly IWorldMan worlds;

        private readonly IPlayersMan players;

        #endregion Private Fields

        #region Public Constructors

        public Game(IManagerCollection manCollection) : base(manCollection)
        {
            this.manCollection2 = manCollection;
            this.scriptMan = manCollection.GetManager<IScriptMan>();
            this.inputs = manCollection.GetManager<IInputsMan>();
            this.worlds = manCollection.GetManager<IWorldMan>();
            this.players = manCollection.GetManager<IPlayersMan>();
            this.renderingMan = manCollection.GetManager<IRenderingMan>();

            logConsolePrinter = new LogConsolePrinter(Logging);
            logConsolePrinter.StartPrinting();
        }

        #endregion Public Constructors

        #region Public Properties

        public override JobsMan Jobs => throw new NotImplementedException();
        public IFsmMan StateMachines => throw new NotImplementedException();

        #endregion Public Properties

        #region Public Methods

        public override void Exit()
        {
            manCollection2.GetManager<IViewClient>().Exit();
        }

        public override void Run()
        {
            ExposeScriptingApi();

            var viewClient = manCollection2.GetManager<IViewClient>();

            viewClient.UpdateFrameEvent += (s, a) => OnUpdateFrame(a);
            viewClient.LoadEvent += (s, a) => OnLoad();

            //TextPresenterSystem.RegisterHandlers(Commands);
            viewClient.Run();
        }

        #endregion Public Methods

        #region Private Methods

        private void OnLoad()
        {
            //var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"D:\Projects\DB\Templates\Logo1.xml");

            //var entity = entityFactory.Create(this, entityTemplate);

            var screenWorld = GetManager<ScreenWorldHelper>();

            renderingMan.ScreenWorld = screenWorld.CreateWorld();

            //var gameWorldHelper = GetManager<GameWorldHelper>();

            //gameWorldHelper.Create();

            var entryScript = manCollection2.GetManager<ScriptsDataProvider>().GetScript("Scripts.Entry.lua");

            var map = manCollection2.GetManager<MapsDataProvider>().GetMap("CRASH LANDING SITE");

            //var templateScript = dataProvider.EntityTemplates.GetEntityTemplate("EntityTemplates.Logo1.lua");

            scriptMan.RunString(entryScript.Script);
        }

        private void OnUpdateFrame(float dt)
        {
            Commands.ExecuteEnqueued();

            worlds.Cleanup();

            renderingMan.Cleanup();

            //Players.ResetInputs();

            inputs.Update();
            //Players.ApplyInputs();
            //StateMachine.Update((float)e.Time);
            worlds.Update(dt);
            //Jobs.Update(dt);
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