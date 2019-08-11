using OpenBreed.Core;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Entities.Builders;
using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;
using OpenBreed.Core.Entities;
using OpenBreed.Game.Worlds;
using OpenBreed.Core.Modules.Animation.Systems.Control.Components;
using OpenBreed.Game.Helpers;
using OpenBreed.Game.Entities.Actor;
using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Game.States
{
    /// <summary>
    /// Tech Demo Class: Animation
    /// </summary>
    public class StateTechDemo4 : BaseState
    {
        #region Public Fields

        public const string ID = "TECH_DEMO_4";

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

        private ITexture tileTex;
        private ITexture spriteTex;
        private ITileAtlas tileAtlas;
        private ISpriteAtlas spriteAtlas;
        private Viewport gameViewport;
        private Viewport hudViewport;

        #endregion Private Fields

        #region Public Constructors

        public StateTechDemo4(ICore core)
        {
            Core = core;

            InitializeWorld();
            InitializeHud();
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
            gameViewport.Width = clientRectangle.Width  - 50;
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

        private void Inputs_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            var pressedKey = e.Key.ToString();

            if (pressedKey.StartsWith("Number"))
            {
                pressedKey = pressedKey.Replace("Number", "");
                Core.StateMachine.SetNextState($"TECH_DEMO_{pressedKey}");
            }
        }

        protected override void OnEnter()
        {
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
            Core.Rendering.Viewports.Remove(hudViewport);
            Core.Rendering.Viewports.Remove(gameViewport);

            Core.Inputs.KeyDown -= Inputs_KeyDown;
        }

        #endregion Protected Methods

        #region Private Methods

        private void InitializeHud()
        {
            var hudWorld = new HudWorld(Core);

            var cameraBuilder = new CameraBuilder(Core);

            cameraBuilder.SetPosition(new Vector2(320, 280));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            HudCamera = (CameraEntity)cameraBuilder.Build();
            hudWorld.AddEntity(HudCamera);


            hudViewport = (Viewport)Core.Rendering.Viewports.Create(0, 0, 540, 380);
            hudViewport.Camera = HudCamera;

            var algerian50 = Core.Rendering.Fonts.Create("ALGERIAN", 50);
            var arial12 = Core.Rendering.Fonts.Create("ARIAL", 12);

            var textEntity = Core.Entities.Create();
            textEntity.Add(Position.Create(0, 0));
            textEntity.Add(Core.Rendering.CreateText(algerian50.Id, Vector2.Zero, "Alice has a cat!"));
            hudWorld.AddEntity(textEntity);

            var fpsEntity = Core.Entities.Create();

            fpsEntity.Add(Position.Create(0, 400));
            fpsEntity.Add(Core.Rendering.CreateText(arial12.Id, Vector2.Zero, "0 fps"));
            hudWorld.AddEntity(fpsEntity);

            Core.Worlds.Add(hudWorld);
        }

        private void InitializeWorld()
        {
            var gameWorld = new GameWorld(Core);

            var cameraBuilder = new CameraBuilder(Core);

            //Resources
            tileTex = Core.Rendering.Textures.GetByAlias("Textures/Tiles/16/Test");
            spriteTex = Core.Rendering.Textures.GetByAlias("Textures/Sprites/Arrow");
            tileAtlas = Core.Rendering.Tiles.Create(tileTex.Id, 16, 4, 4);
            spriteAtlas = Core.Rendering.Sprites.Create(spriteTex.Id, 32, 32, 8, 5);

            cameraBuilder.SetPosition(new Vector2(64, 64));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            GameCamera = (CameraEntity)cameraBuilder.Build();
            gameWorld.AddEntity(GameCamera);


            gameViewport = (Viewport)Core.Rendering.Viewports.Create(50, 50, 540, 380);
            gameViewport.Camera = GameCamera;

            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas(tileAtlas.Id);

            var actor = ActorHelper.CreateActor(Core, new Vector2(64, 288), spriteAtlas);
            actor.Add(new KeyboardControl(Key.Up, Key.Down, Key.Left, Key.Right));
            actor.Add(TextHelper.Create(Core, new Vector2(-10, 10), "Hero"));

            var stateMachine = ActorHelper.CreateStateMachine(actor);
            stateMachine.SetInitialState("Standing_Down");

            gameWorld.AddEntity(actor);

            var rnd = new Random();

            var ymax = mapA.Length / 10;

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < ymax; y++)
                {
                    var v = mapA[x + y * 10];

                    if (v > 0)
                    {
                        blockBuilder.SetIndices(x + 5, y + 5);
                        blockBuilder.SetTileId(v);
                        gameWorld.AddEntity(blockBuilder.Build());
                    }
                }
            }

            Core.Worlds.Add(gameWorld);
        }

        #endregion Private Methods
    }
}