using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Worlds;
using OpenTK;
using OpenTK.Input;
using System;

namespace OpenBreed.Sandbox.States
{
    /// <summary>
    /// Tech Demo Class: Animation
    /// </summary>
    public class StateTechDemo5 : BaseState
    {
        #region Public Fields

        public const string ID = "TECH_DEMO_5";

        public World GameWorld;

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

        public IEntity GameCamera { get; private set; }
        public IEntity HudCamera { get; private set; }

        public override string Name { get { return ID; } }

        #endregion Public Properties

        #region Public Methods

        public override void Update(float dt)
        {
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            Viewport hoverViewport = null;

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

        #endregion Public Methods

        #region Protected Methods

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

        public static void SetupWorld(World world)
        {
            SandBoxHelper.SetupMap(world);

            var rnd = new Random();

            for (int i = 0; i < 10; i++)
                DoorHelper.AddHorizontalDoor(world, rnd.Next(1, 20) * 3, rnd.Next(1, 20) * 3);

            for (int i = 0; i < 10; i++)
                DoorHelper.AddVerticalDoor(world, rnd.Next(1, 20) * 3, rnd.Next(1, 20) * 3);
        }

        private void InitializeWorld()
        {
            GameWorld = GameWorldHelper.CreateGameWorld(Core, "DEMO5");
            SetupWorld(GameWorld);

            var cameraBuilder = new CameraBuilder(Core);

            cameraBuilder.SetPosition(new Vector2(64, 64));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            GameCamera = cameraBuilder.Build();
            GameWorld.AddEntity(GameCamera);

            gameViewport = (Viewport)Core.Rendering.Viewports.Create(0.05f, 0.05f, 0.90f, 0.90f);
            gameViewport.CameraEntity = GameCamera;

            var actor = ActorHelper.CreateActor(Core, new Vector2(64, 288));
            actor.Add(new WalkingControl());
            actor.Add(TextHelper.Create(Core, new Vector2(0, 32), "Hero"));

            var player1 = Core.Players.GetByName("P1");
            player1.AssumeControl(actor);
            var player2 = Core.Players.GetByName("P2");
            player2.AssumeControl(actor);

            var movementFsm = ActorHelper.CreateMovementFSM(actor);
            var rotateFsm = ActorHelper.CreateRotationFSM(actor);
            movementFsm.SetInitialState("Standing");
            rotateFsm.SetInitialState("Idle");
            GameWorld.AddEntity(actor); 
        }

        #endregion Private Methods
    }
}