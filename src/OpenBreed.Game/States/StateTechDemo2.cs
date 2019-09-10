using OpenBreed.Core;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Entities.Builders;
using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;
using OpenBreed.Core.Entities;
using OpenBreed.Game.Worlds;
using OpenBreed.Core.Modules.Animation.Systems.Control.Components;
using OpenBreed.Game.Entities.Actor;
using OpenBreed.Core.Common;

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

        private Viewport viewportLeft;
        private Viewport viewportRight;

        #endregion Private Fields

        #region Public Constructors

        public StateTechDemo2(ICore core)
        {
            Core = core;
        }

        private void DeinitializeAll()
        {
            WorldA.RemoveAllEntities();
            WorldB.RemoveAllEntities();
            Core.Worlds.Remove(WorldA);
            Core.Worlds.Remove(WorldB);

            Core.Rendering.Viewports.Remove(viewportLeft);
            Core.Rendering.Viewports.Remove(viewportRight);

            Core.Inputs.KeyDown -= Inputs_KeyDown;
        }

        private void InitializeAll()
        {
            WorldA = new GameWorld(Core);
            WorldB = new GameWorld(Core);
            var cameraBuilder = new CameraBuilder(Core);

            //Resources
            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            Camera1 = (CameraEntity)cameraBuilder.Build();
            WorldA.AddEntity(Camera1);

            cameraBuilder.SetPosition(new Vector2(64, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            Camera2 = (CameraEntity)cameraBuilder.Build();
            WorldB.AddEntity(Camera2);

            viewportLeft = (Viewport)Core.Rendering.Viewports.Create(50, 50, 540, 380);
            viewportLeft.Camera = Camera1;

            viewportRight = (Viewport)Core.Rendering.Viewports.Create(50, 50, 540, 380);
            viewportRight.Camera = Camera2;

            Core.Worlds.Add(WorldA);
            Core.Worlds.Add(WorldB);

            Core.Inputs.KeyDown += Inputs_KeyDown;

            Core.Rendering.Viewports.Add(viewportLeft);
            Core.Rendering.Viewports.Add(viewportRight);
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public CameraEntity Camera1 { get; private set; }

        public CameraEntity Camera2 { get; private set; }

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

        public override void Update(float dt)
        {
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            Viewport hoverViewport = null;

            if (viewportLeft.TestScreenCoords(Core.Inputs.CursorPos))
                hoverViewport = viewportLeft;
            else if (viewportRight.TestScreenCoords(Core.Inputs.CursorPos))
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
            InitializeAll();

            InitializeWorldA();
            InitializeWorldB();

            Console.Clear();
            Console.WriteLine("---------- Multi-worlds --------");
            Console.WriteLine("This demo shows two separate worlds, one per viewport");
            Console.WriteLine("Constrols:");
            Console.WriteLine("MMB + Move mouse cursor over hovered viewport = Camera scroll control");
            Console.WriteLine("Mouse wheel over hovered viewport = Camera zoom control");
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
            DeinitializeAll();
        }

        #endregion Protected Methods

        #region Private Methods

        private void InitializeWorldA()
        {
            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas("Atlases/Tiles/16/Test");

            var actor = ActorHelper.CreateActor(Core, new Vector2(20, 20));
            actor.Add(new KeyboardControl(Key.Up, Key.Down, Key.Left, Key.Right));

            var stateMachine = ActorHelper.CreateMovementFSM(actor);
            stateMachine.SetInitialState("Standing_Right");

            WorldA.AddEntity(actor);

            var rnd = new Random();

            var ymax = mapA.Length / 10;

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < ymax; y++)
                {
                    var v = mapA[x + y * 10];

                    if (v > 0)
                    {
                        blockBuilder.SetPosition(new Vector2((x + 5) * 16, (16 - y + 5) * 16));
                        blockBuilder.SetTileId(v);
                        WorldA.AddEntity(blockBuilder.Build());
                    }
                }
            }
        }

        private void InitializeWorldB()
        {
            var actor = ActorHelper.CreateActor(Core, new Vector2(50, 20));
            actor.Add(new KeyboardControl(Key.Up, Key.Down, Key.Left, Key.Right));

            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas("Atlases/Tiles/16/Test");

            var stateMachine = ActorHelper.CreateMovementFSM(actor);
            stateMachine.SetInitialState("Standing_Down");

            WorldB.AddEntity(actor);

            var rnd = new Random();

            var ymax = mapB.Length / 10;

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < ymax; y++)
                {
                    var v = mapB[x + y * 10];

                    if (v > 0)
                    {
                        blockBuilder.SetPosition(new Vector2((x + 5) * 16, (y + 5) * 16));
                        blockBuilder.SetTileId(v);
                        WorldB.AddEntity(blockBuilder.Build());
                    }
                }
            }
        }

        #endregion Private Methods
    }
}