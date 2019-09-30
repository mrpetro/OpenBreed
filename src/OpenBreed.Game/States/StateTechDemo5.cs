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
using OpenBreed.Game.Entities.Door;
using OpenBreed.Game.Entities.Box;
using OpenBreed.Game.Entities.Projectile;

namespace OpenBreed.Game.States
{
    /// <summary>
    /// Tech Demo Class: Animation
    /// </summary>
    public class StateTechDemo5 : BaseState
    {
        #region Public Fields

        public const string ID = "TECH_DEMO_5";

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

        #endregion Private Fields

        #region Public Constructors

        public StateTechDemo5(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public CameraEntity GameCamera { get; private set; }
        public CameraEntity HudCamera { get; private set; }

        public override string Name { get { return ID; } }

        #endregion Public Properties

        #region Public Methods

        public override void OnResize(Rectangle clientRectangle)
        {
            base.OnResize(clientRectangle);

            gameViewport.X = clientRectangle.X + 25;
            gameViewport.Y = clientRectangle.Y + 25;
            gameViewport.Width = clientRectangle.Width  - 50;
            gameViewport.Height = clientRectangle.Height - 50;
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
            InitializeWorld();

            Core.Inputs.KeyDown += Inputs_KeyDown;
            Core.Rendering.Viewports.Add(gameViewport);

            Console.Clear();
            Console.WriteLine("---------- Door entities --------");
            Console.WriteLine("This demo shows door entites with usage of FSM pattern. Actor can open doors by touching them.");
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
        }
        public GameWorld GameWorld;

        #endregion Protected Methods

        #region Private Methods

        private void InitializeWorld()
        {
            GameWorld = new GameWorld(Core);

            var cameraBuilder = new CameraBuilder(Core);

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

   
            var movementSm = ActorHelper.CreateMovementFSM(actor);
            movementSm.SetInitialState("Standing");
            GameWorld.AddEntity(actor);

            //ProjectileHelper.AddProjectile(Core, GameWorld, 100, 100, 0, 0);


            var rnd = new Random();

            //for (int i = 0; i < 5; i++)
            //{
            //    var ball = BoxHelper.CreateBox(Core, new Vector2(rnd.Next(16, 64), rnd.Next(16, 64)),
            //                                          new Vector2(rnd.Next(70, 700), rnd.Next(70, 700)),
            //                                          new Vector2(rnd.Next(-50, 50), rnd.Next(-50, 50)));

            //    gameWorld.AddEntity(ball);
            //}

            for (int i = 0; i < 10; i++)
                DoorHelper.AddHorizontalDoor(GameWorld, rnd.Next(1, 20) * 3, rnd.Next(1, 20) * 3);

            for (int i = 0; i < 10; i++)
                DoorHelper.AddVerticalDoor(Core, GameWorld, rnd.Next(1, 20) * 3, rnd.Next(1, 20) * 3);

            //blockBuilder.SetPosition(new Vector2(5 * 16, 10 * 16));
            //blockBuilder.SetTileId(2);
            //GameWorld.AddEntity(blockBuilder.Build());

            //DoorHelper.AddHorizontalDoor(Core, GameWorld,  3,  3);

            for (int x = 0; x < 64; x++)
            {
                blockBuilder.SetPosition(new Vector2(x * 16, 0));
                blockBuilder.SetTileId(9);
                GameWorld.AddEntity(blockBuilder.Build());

                blockBuilder.SetPosition(new Vector2(x * 16, 62 * 16));
                blockBuilder.SetTileId(9);
                GameWorld.AddEntity(blockBuilder.Build());
            }

            for (int y = 1; y < 63; y++)
            {
                blockBuilder.SetPosition(new Vector2(0, y * 16));
                blockBuilder.SetTileId(9);
                GameWorld.AddEntity(blockBuilder.Build());

                blockBuilder.SetPosition(new Vector2(62 * 16, y * 16));
                blockBuilder.SetTileId(9);
                GameWorld.AddEntity(blockBuilder.Build());
            }

            Core.Worlds.Add(GameWorld);
        }

        #endregion Private Methods
    }
}