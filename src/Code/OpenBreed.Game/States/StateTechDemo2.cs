using OpenBreed.Core;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Common.Components.Shapes;
using OpenBreed.Core.Systems.Physics.Components;
using OpenBreed.Core.Systems.Rendering;
using OpenBreed.Core.Systems.Rendering.Components;
using OpenBreed.Core.Systems.Rendering.Entities;
using OpenBreed.Core.Systems.Rendering.Entities.Builders;
using OpenBreed.Core.Systems.Rendering.Helpers;
using OpenBreed.Game.Components;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Entities.Builders;
using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;

namespace OpenBreed.Game.States
{
    /// <summary>
    /// Tech Demo Class: Multi-worlds
    /// </summary>
    public class StateTechDemo2 : BaseState
    {
        #region Public Fields

        public const string ID = "TECH_DEMO_2";

        public World WorldA;

        public World WorldB;

        #endregion Public Fields

        #region Private Fields

        private static byte[] mapA = new byte[] {
            0,0,8,8,8,8,8,8,0,0,
            0,8,8,8,8,8,8,8,8,0,
            8,8,0,0,0,0,0,0,8,8,
            8,8,8,8,8,8,8,8,8,8,
            8,8,8,8,8,8,8,8,8,8,
            8,8,0,0,0,0,0,0,8,8,
            8,8,0,0,0,0,0,0,8,8,
            8,8,0,0,0,0,0,0,8,8
        };

        private static byte[] mapB = new byte[] {
            7,7,7,7,7,7,7,7,0,0,
            7,7,0,0,0,0,0,7,7,7,
            7,7,0,0,0,0,0,7,7,7,
            7,7,7,7,7,7,7,7,7,0,
            7,7,7,7,7,7,7,7,7,0,
            7,7,0,0,0,0,0,7,7,7,
            7,7,0,0,0,0,0,7,7,7,
            7,7,7,7,7,7,7,7,0,0
        };

        private ITexture tileTex;
        private ITexture spriteTex;
        private TileAtlas tileAtlas;
        private SpriteAtlas spriteAtlas;
        private int px;
        private int py;
        private Viewport viewportLeft;
        private Viewport viewportRight;

        #endregion Private Fields

        #region Public Constructors

        public StateTechDemo2(ICore core)
        {
            Core = core;

            InitializeAll();

            InitializeWorldA();
            InitializeWorldB();
        }

        private void InitializeAll()
        {
            WorldA = new World(Core);
            WorldB = new World(Core);
            var cameraBuilder = new CameraBuilder(Core);

            //Resources
            tileTex = Core.Rendering.GetTexture(@"Content\TileAtlasTest32bit.bmp");
            spriteTex = Core.Rendering.GetTexture(@"Content\ArrowSpriteSet.png");
            tileAtlas = new TileAtlas(tileTex, 16, 4, 4);
            spriteAtlas = new SpriteAtlas(spriteTex, 32, 8, 1);

            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            Camera1 = (Camera)cameraBuilder.Build();
            WorldA.AddEntity(Camera1);

            cameraBuilder.SetPosition(new Vector2(64, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            Camera2 = (Camera)cameraBuilder.Build();
            WorldB.AddEntity(Camera2);

            viewportLeft = new Viewport(50, 50, 540, 380);
            viewportLeft.Camera = Camera1;

            viewportRight = new Viewport(50, 50, 540, 380);
            viewportRight.Camera = Camera2;

            Core.Worlds.Add(WorldA);
            Core.Worlds.Add(WorldB);
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public Camera Camera1 { get; private set; }

        public Camera Camera2 { get; private set; }

        public override string Id { get { return ID; } }

        #endregion Public Properties

        #region Public Methods

        public override void OnResize(Rectangle clientRectangle)
        {
            base.OnResize(clientRectangle);

            viewportLeft.X = clientRectangle.X + 25;
            viewportLeft.Y = clientRectangle.Y + 25;
            viewportLeft.Width = clientRectangle.Width / 2 - 50;
            viewportLeft.Height = clientRectangle.Height - 50;

            viewportRight.X = clientRectangle.X + 25 + clientRectangle.Width / 2;
            viewportRight.Y = clientRectangle.Y + 25;
            viewportRight.Width = clientRectangle.Width / 2 - 50;
            viewportRight.Height = clientRectangle.Height - 50;
        }

        public override void ProcessInputs(FrameEventArgs e)
        {
            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Key.Escape))
                ChangeState(MenuState.ID);

            var mouseState = Mouse.GetState();

            Viewport hoverViewport = null;

            if (viewportLeft.TestScreenCoords(Core.CursorPos))
                hoverViewport = viewportLeft;
            else if (viewportRight.TestScreenCoords(Core.CursorPos))
                hoverViewport = viewportRight;
            else
                hoverViewport = null;

            if (hoverViewport != null)
            {
                hoverViewport.Camera.Zoom = Tools.GetZoom(Core, hoverViewport.Camera.Zoom);

                if (mouseState.IsButtonDown(MouseButton.Middle))
                {
                    var transf = hoverViewport.Camera.GetTransform();
                    transf.Invert();
                    var delta4 = Vector4.Transform(transf, new Vector4(Core.CursorDelta));
                    var delta2 = new Vector2(-delta4.X, -delta4.Y);

                    hoverViewport.Camera.Position += delta2;
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnEnter()
        {
            Core.Inputs.KeyDown += Inputs_KeyDown;

            Core.Viewports.Add(viewportLeft);
            Core.Viewports.Add(viewportRight);

            Console.Clear();
            Console.WriteLine("---------- Multi-worlds --------");
            Console.WriteLine("This demo shows two separate worlds, one per viewport");
            Console.WriteLine("Constrols:");
            Console.WriteLine("RMB + Move mouse cursor = Camera control over hovered viewport");
            Console.WriteLine("Keyboard arrows  = Control arrow actor");
        }

        private void Inputs_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            var pressedKey = e.Key.ToString();

            if (pressedKey.StartsWith("Number"))
            {
                pressedKey = pressedKey.Replace("Number", "");
                Core.StateMachine.SetNextState($"TECH_DEMO_{pressedKey}");
            }
        }

        protected override void OnLeave()
        {
            Core.Viewports.Remove(viewportLeft);
            Core.Viewports.Remove(viewportRight);

            Core.Inputs.KeyDown -= Inputs_KeyDown;
        }

        #endregion Protected Methods

        #region Private Methods

        private void InitializeWorldA()
        {
            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas(tileAtlas);

            var actorBuilder = new WorldActorBuilder(Core);
            actorBuilder.SetSpriteAtlas(spriteAtlas);
            actorBuilder.SetSprite(new Sprite(spriteAtlas));
            actorBuilder.SetPosition(new DynamicPosition(20, 20));
            actorBuilder.SetDirection(new Direction(1, 0));
            actorBuilder.SetShape(new AxisAlignedBoxShape(32, 32));
            actorBuilder.SetAnimator(new CreatureAnimator());
            actorBuilder.SetMovement(new CreatureMovement());
            actorBuilder.SetBody(new DynamicBody());

            actorBuilder.SetController(new KeyboardCreatureController(Key.Up, Key.Down, Key.Left, Key.Right));
            WorldA.AddEntity((WorldActor)actorBuilder.Build());

            var rnd = new Random();

            var ymax = mapA.Length / 10;

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < ymax; y++)
                {
                    var v = mapA[x + y * 10];

                    if (v > 0)
                    {
                        blockBuilder.SetIndices(x + 5, 10 - y + 5);
                        blockBuilder.SetTileId(v);
                        WorldA.AddEntity((WorldBlock)blockBuilder.Build());
                    }
                }
            }
        }

        private void InitializeWorldB()
        {
            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas(tileAtlas);

            var actorBuilder = new WorldActorBuilder(Core);
            actorBuilder.SetSpriteAtlas(spriteAtlas);
            actorBuilder.SetSprite(new Sprite(spriteAtlas));
            actorBuilder.SetPosition(new DynamicPosition(50, 20));
            actorBuilder.SetDirection(new Direction(1, 0));
            actorBuilder.SetShape(new AxisAlignedBoxShape(32, 32));
            actorBuilder.SetAnimator(new CreatureAnimator());
            actorBuilder.SetMovement(new CreatureMovement());
            actorBuilder.SetBody(new DynamicBody());

            actorBuilder.SetController(new KeyboardCreatureController(Key.Up, Key.Down, Key.Left, Key.Right));
            WorldB.AddEntity((WorldActor)actorBuilder.Build());

            var rnd = new Random();

            var ymax = mapB.Length / 10;

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < ymax; y++)
                {
                    var v = mapB[x + y * 10];

                    if (v > 0)
                    {
                        blockBuilder.SetIndices(x + 5, y + 5);
                        blockBuilder.SetTileId(v);
                        WorldB.AddEntity((WorldBlock)blockBuilder.Build());
                    }
                }
            }
        }

        #endregion Private Methods
    }
}