using OpenBreed.Core;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Animation.Components;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Common.Components.Shapes;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Game.Commands;
using OpenBreed.Game.Components;
using OpenBreed.Game.Components.States;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Entities.Builders;
using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;
using OpenBreed.Core.Entities;
using OpenBreed.Game.Worlds;

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

        public Camera gameCamera { get; private set; }
        public Camera hudCamera { get; private set; }

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
 
                    hoverViewport.Camera.Position += delta2;
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
            Console.WriteLine("This demo shows three viewports with two cameras attached to them.");
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
            var hudWorld = new GameWorld(Core);

            var cameraBuilder = new CameraBuilder(Core);

            cameraBuilder.SetPosition(new Vector2(64, 64));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            hudCamera = (Camera)cameraBuilder.Build();
            hudWorld.AddEntity(hudCamera);


            hudViewport = (Viewport)Core.Rendering.Viewports.Create(50, 50, 540, 380);
            hudViewport.Camera = hudCamera;

            var fontAtlas = Core.Rendering.Fonts.Create("ALGERIAN", 50);

            var textEntity = Core.Entities.Create();
            textEntity.Add(new Position(40, 50));
            textEntity.Add(Core.Rendering.CreateText(fontAtlas.Id, "Alice has a cat!"));

            hudWorld.AddEntity(textEntity);


            Core.Worlds.Add(hudWorld);
        }

        private void InitializeWorld()
        {
            var gameWorld = new GameWorld(Core);

            var cameraBuilder = new CameraBuilder(Core);

            //Resources
            tileTex = Core.Rendering.Textures.Load(@"Content\TileAtlasTest32bit.bmp");
            spriteTex = Core.Rendering.Textures.Load(@"Content\ArrowSpriteSet.png");
            tileAtlas = Core.Rendering.Tiles.Create(tileTex, 16, 4, 4);
            spriteAtlas = Core.Rendering.Sprites.Create(spriteTex, 32, 32, 8, 1);

            cameraBuilder.SetPosition(new Vector2(64, 64));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            gameCamera = (Camera)cameraBuilder.Build();
            gameWorld.AddEntity(gameCamera);


            gameViewport = (Viewport)Core.Rendering.Viewports.Create(50, 50, 540, 380);
            gameViewport.Camera = gameCamera;

            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas(tileAtlas.Id);

            var sprite = Core.Rendering.CreateSprite(spriteAtlas.Id);
            var animator = ActorHelper.CreateAnimator();
            var stateMachine = ActorHelper.CreateStateMachine();
            stateMachine.SetInitialState("Standing_Right");

            var actor = Core.Entities.Create();
            actor.Add(stateMachine);
            actor.Add(animator);
            actor.Add(sprite);
            actor.Add(new Position(64, 288));
            actor.Add(new Velocity(0, 0));
            actor.Add(new Direction(1, 0));
            actor.Add(new AxisAlignedBoxShape(32, 32));
            actor.Add(new CreatureMovement());
            actor.Add(new DynamicBody());
            actor.Add(new KeyboardCreatureController(Key.Up, Key.Down, Key.Left, Key.Right));

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