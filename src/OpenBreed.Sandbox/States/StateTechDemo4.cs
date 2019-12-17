using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Worlds;
using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Sandbox.States
{
    /// <summary>
    /// Tech Demo Class: Animation
    /// </summary>
    public class StateTechDemo4 : BaseState
    {
        #region Public Fields

        public const string ID = "TECH_DEMO_4";

        public World GameWorld;

        #endregion Public Fields

        #region Private Fields

        private static byte[] mapA = new byte[] {
            10,10,10,10,10,10,10,10,10,15,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 11,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 11,
            11,0, 0,11, 0,11, 0, 0, 0, 11,
            11,0, 0,11, 0,11, 0, 0, 0, 11,
            10,10,10,10,10,10,10,10,10,7
        };

        private Viewport gameViewport;

        #endregion Private Fields

        #region Public Constructors

        public StateTechDemo4(Program core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public Program Core { get; }

        public IEntity GameCamera { get; private set; }

        public override string Name { get { return ID; } }

        #endregion Public Properties

        #region Public Methods

        private Viewport hoverViewport = null;

        public override void Update(float dt)
        {
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (gameViewport.TestClientCoords(Core.Inputs.CursorPos))
                hoverViewport = gameViewport;
            else
                hoverViewport = null;

            if (hoverViewport != null)
            {
                hoverViewport.Zoom = Tools.GetZoom(Core, hoverViewport.Zoom);

                if (mouseState.IsButtonDown(MouseButton.Middle))
                    hoverViewport.ScrollBy(Core.Inputs.CursorDelta);
            }
        }

        private void Inputs_MouseMove(object sender, MouseMoveEventArgs e)
        {
            //Console.WriteLine($"ScreenPos: {CoreTools.ToConsole(Core.Inputs.CursorPos)}");

            if (hoverViewport != null)
            {
                //Console.WriteLine($"ViewportPos: {CoreTools.ToConsole(hoverViewport.FromClientPoint(Core.Inputs.CursorPos))}");
                Console.WriteLine($"WorldPos: {CoreTools.ToConsole(hoverViewport.ClientToWorldPoint(Core.Inputs.CursorPos))}");
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnEnter()
        {
            InitializeWorld();

            Core.Inputs.MouseMove += Inputs_MouseMove;
            Core.Inputs.KeyDown += Inputs_KeyDown;

            Console.Clear();
            Console.WriteLine("---------- Fonts & Texts --------");
            Console.WriteLine("This demo shows typical usage of fonts and texts on the screen.");
            Console.WriteLine("Constrols:");
            Console.WriteLine("RMB + Move mouse cursor = Camera control over hovered viewport");
            Console.WriteLine("Keyboard arrows  = Control arrow actor");
        }

        protected override void OnLeave()
        {
            GameWorld.RemoveAllEntities();
            Core.Worlds.Remove(GameWorld);

            Core.Rendering.Viewports.Remove(gameViewport);

            Core.Inputs.KeyDown -= Inputs_KeyDown;
            Core.Inputs.MouseMove -= Inputs_MouseMove;
            Core.Players.LooseAllControls();
        }

        #endregion Protected Methods

        #region Private Methods

        private void Inputs_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            var pressedKey = e.Key.ToString();

            if (pressedKey.StartsWith("Number"))
            {
                pressedKey = pressedKey.Replace("Number", "");
                Core.StateMachine.SetNextState($"TECH_DEMO_{pressedKey}");
            }
        }

        private void InitializeWorld()
        {
            GameWorld = GameWorldHelper.CreateGameWorld(Core, "DEMO4");

            var cameraBuilder = new CameraBuilder(Core);

            //Resources
            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1.0f);
            GameCamera = cameraBuilder.Build();
            GameWorld.AddEntity(GameCamera);

            gameViewport = (Viewport)Core.Rendering.Viewports.Create(0.05f, 0.05f, 0.9f, 0.9f);
            Core.Rendering.Viewports.Add(gameViewport);
            gameViewport.DrawBorder = true;
            gameViewport.Clipping = true;
            //gameViewport.DrawBackgroud = true;
            //gameViewport.BackgroundColor = new OpenTK.Graphics.Color4(255, 0, 0, 255);
            gameViewport.CameraEntity = GameCamera;

            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas("Atlases/Tiles/16/Test");

            var actor = ActorHelper.CreateActor(Core, new Vector2(64, 288));
            actor.Add(new WalkingControl());
            actor.Add(TextHelper.Create(Core, new Vector2(-10, 10), "Hero"));

            var player1 = Core.Players.GetByName("P1");
            player1.AssumeControl(actor);
            var player2 = Core.Players.GetByName("P2");
            player2.AssumeControl(actor);


            var movementFsm = ActorHelper.CreateMovementFSM(actor);
            var rotateFsm = ActorHelper.CreateRotationFSM(actor);
            movementFsm.SetInitialState("Standing");
            rotateFsm.SetInitialState("Idle");

            GameWorld.AddEntity(actor);

            var rnd = new Random();

            var ymax = mapA.Length / 10;

            SandBoxHelper.SetupMap(GameWorld);

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < ymax; y++)
                {
                    var v = mapA[x + y * 10];

                    if (v > 0)
                    {
                        blockBuilder.SetPosition(new Vector2(x * 16 + 5 * 16, y * 16 + 5 * 16));
                        blockBuilder.SetTileId(v);
                        GameWorld.AddEntity(blockBuilder.Build());
                    }
                }
            }

        }

        #endregion Private Methods
    }
}