using OpenBreed.Core;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Animation.Systems.Control.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Game.Entities.Actor;
using OpenBreed.Game.Entities.Builders;
using OpenBreed.Game.Helpers;
using OpenBreed.Game.Worlds;
using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;

namespace OpenBreed.Game.States
{
    /// <summary>
    /// Tech Demo Class: Animation
    /// </summary>
    public class StateTechDemo4 : BaseState
    {
        #region Public Fields

        public const string ID = "TECH_DEMO_4";

        public HudWorld HudWorld;

        public GameWorld GameWorld;

        #endregion Public Fields

        #region Private Fields

        private static byte[] mapA = new byte[] {
            3,3,3,3,3,3,3,3,3,3,
            0,0,0,0,0,0,0,0,1,3,
            0,0,0,0,0,0,0,0,1,3,
            3,0,0,1,0,1,0,0,4,3,
            3,0,0,2,2,2,0,0,2,3,
            3,3,3,3,3,3,3,3,3,3
        };

        private Viewport gameViewport;
        private Viewport hudViewport;

        #endregion Private Fields

        #region Public Constructors

        public StateTechDemo4(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public CameraEntity GameCamera { get; private set; }
        public CameraEntity HudCamera { get; private set; }

        public override string Id { get { return ID; } }

        #endregion Public Properties

        #region Public Methods

        public override void OnResize(Rectangle clientRectangle)
        {
            base.OnResize(clientRectangle);

            gameViewport.X = clientRectangle.X + 25;
            gameViewport.Y = clientRectangle.Y + 25;
            gameViewport.Width = clientRectangle.Width - 50;
            gameViewport.Height = clientRectangle.Height - 50;

            hudViewport.X = clientRectangle.X + 25;
            hudViewport.Y = clientRectangle.Y + 25;
            hudViewport.Width = clientRectangle.Width - 50;
            hudViewport.Height = clientRectangle.Height - 50;
        }

        public override void Update(float dt)
        {
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            Viewport hoverViewport = null;

            if (gameViewport.TestScreenCoords(Core.Inputs.CursorPos))
                hoverViewport = gameViewport;
            else
                hoverViewport = null;

            if (hoverViewport != null)
            {
                hoverViewport.Camera.Zoom = Tools.GetZoom(Core, hoverViewport.Camera.Zoom);

                if (mouseState.IsButtonDown(MouseButton.Middle))
                {
                    var transf = hoverViewport.Camera.GetTransform();
                    transf.Invert();
                    var delta4 = Vector4.Transform(transf, new Vector4(Core.Inputs.CursorDelta));
                    var delta2 = new Vector2(-delta4.X, -delta4.Y);

                    hoverViewport.Camera.Position.Value += delta2;
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnEnter()
        {
            InitializeWorld();
            InitializeHud();

            Core.Inputs.KeyDown += Inputs_KeyDown;

            Core.Rendering.Viewports.Add(gameViewport);
            Core.Rendering.Viewports.Add(hudViewport);

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
            HudWorld.RemoveAllEntities();
            Core.Worlds.Remove(HudWorld);

            Core.Rendering.Viewports.Remove(hudViewport);
            Core.Rendering.Viewports.Remove(gameViewport);

            Core.Inputs.KeyDown -= Inputs_KeyDown;
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

        private void InitializeHud()
        {
            HudWorld = new HudWorld(Core);

            var cameraBuilder = new CameraBuilder(Core);

            cameraBuilder.SetPosition(new Vector2(320, 280));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            HudCamera = (CameraEntity)cameraBuilder.Build();
            HudWorld.AddEntity(HudCamera);

            hudViewport = (Viewport)Core.Rendering.Viewports.Create(0, 0, 540, 380);
            hudViewport.Camera = HudCamera;

            var algerian50 = Core.Rendering.Fonts.Create("ALGERIAN", 50);
            var arial12 = Core.Rendering.Fonts.Create("ARIAL", 12);

            var textEntity = Core.Entities.Create();
            textEntity.Add(Position.Create(0, 0));
            textEntity.Add(Text.Create(algerian50.Id, Vector2.Zero, "Alice has a cat!"));
            HudWorld.AddEntity(textEntity);

            var fpsEntity = Core.Entities.Create();

            fpsEntity.Add(Position.Create(0, 400));
            fpsEntity.Add(Text.Create(arial12.Id, Vector2.Zero, "0 fps"));
            HudWorld.AddEntity(fpsEntity);

            Core.Worlds.Add(HudWorld);
        }

        private void InitializeWorld()
        {
            GameWorld = new GameWorld(Core);

            var cameraBuilder = new CameraBuilder(Core);

            //Resources
            cameraBuilder.SetPosition(new Vector2(64, 64));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            GameCamera = (CameraEntity)cameraBuilder.Build();
            GameWorld.AddEntity(GameCamera);

            gameViewport = (Viewport)Core.Rendering.Viewports.Create(50, 50, 540, 380);
            gameViewport.Camera = GameCamera;

            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas("Atlases/Tiles/16/Test");

            var actor = ActorHelper.CreateActor(Core, new Vector2(64, 288));
            actor.Add(new KeyboardControl(Key.Up, Key.Down, Key.Left, Key.Right, Key.ControlRight));
            actor.Add(TextHelper.Create(Core, new Vector2(-10, 10), "Hero"));

            var stateMachine = ActorHelper.CreateMovementFSM(actor);
            stateMachine.SetInitialState("Standing_Down");

            GameWorld.AddEntity(actor);

            var rnd = new Random();

            var ymax = mapA.Length / 10;

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

            Core.Worlds.Add(GameWorld);
        }

        #endregion Private Methods
    }
}