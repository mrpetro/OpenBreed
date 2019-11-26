using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Worlds;
using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Sandbox.States
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

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public IEntity Camera1 { get; private set; }

        public IEntity Camera2 { get; private set; }

        public override string Name { get { return ID; } }

        #endregion Public Properties

        #region Public Methods

        public override void Update(float dt)
        {
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            Viewport hoverViewport = null;

            if (viewportLeft.TestClientCoords(Core.Inputs.CursorPos))
                hoverViewport = viewportLeft;
            else if (viewportRight.TestClientCoords(Core.Inputs.CursorPos))
                hoverViewport = viewportRight;
            else
                hoverViewport = null;

            if (hoverViewport != null)
            {
                hoverViewport.Zoom = Tools.GetZoom(Core, hoverViewport.Zoom);

                if (mouseState.IsButtonDown(MouseButton.Middle))
                    hoverViewport.ScrollBy(Core.Inputs.CursorDelta);
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
            Console.WriteLine("Keyboard arrows = Control arrow actor");
        }

        protected override void OnLeave()
        {
            DeinitializeAll();
        }

        #endregion Protected Methods

        #region Private Methods

        private void DeinitializeAll()
        {
            WorldA.RemoveAllEntities();
            WorldB.RemoveAllEntities();
            Core.Worlds.Remove(WorldA);
            Core.Worlds.Remove(WorldB);

            Core.Rendering.Viewports.Remove(viewportLeft);
            Core.Rendering.Viewports.Remove(viewportRight);

            Core.Inputs.KeyDown -= Inputs_KeyDown;
            Core.Players.LooseAllControls();
        }

        private void InitializeAll()
        {
            WorldA = GameWorldHelper.CreateGameWorld(Core, "DEMO2_A");
            WorldB = GameWorldHelper.CreateGameWorld(Core, "DEMO2_B");
            var cameraBuilder = new CameraBuilder(Core);

            //Resources
            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            Camera1 = cameraBuilder.Build();
            WorldA.AddEntity(Camera1);

            cameraBuilder.SetPosition(new Vector2(64, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            Camera2 = cameraBuilder.Build();
            WorldB.AddEntity(Camera2);

            viewportLeft = (Viewport)Core.Rendering.Viewports.Create(0.05f, 0.05f, 0.40f, 0.90f);
            viewportLeft.CameraEntity = Camera1;

            viewportRight = (Viewport)Core.Rendering.Viewports.Create(0.55f, 0.05f, 0.40f, 0.90f);

            viewportRight.CameraEntity = Camera2;

            Core.Inputs.KeyDown += Inputs_KeyDown;

            Core.Rendering.Viewports.Add(viewportLeft);
            Core.Rendering.Viewports.Add(viewportRight);
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

        private void InitializeWorldA()
        {
            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas("Atlases/Tiles/16/Test");

            var actor = ActorHelper.CreateActor(Core, new Vector2(20, 20));
            actor.Add(new WalkingControl());

            var player1 = Core.Players.GetByName("P1");
            player1.AssumeControl(actor);
            var player2 = Core.Players.GetByName("P2");
            player2.AssumeControl(actor);

            var movementSm = ActorHelper.CreateMovementFSM(actor);
            movementSm.SetInitialState("Standing");

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
            actor.Add(new WalkingControl());

            var player1 = Core.Players.GetByName("P1");
            player1.AssumeControl(actor);
            var player2 = Core.Players.GetByName("P2");
            player2.AssumeControl(actor);

            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas("Atlases/Tiles/16/Test");

            var movementFsm = ActorHelper.CreateMovementFSM(actor);
            var rotateFsm = ActorHelper.CreateRotationFSM(actor);
            movementFsm.SetInitialState("Standing");
            rotateFsm.SetInitialState("Idle");

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