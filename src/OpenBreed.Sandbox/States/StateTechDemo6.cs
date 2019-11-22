using OpenBreed.Core;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Builders;
using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;
using OpenBreed.Core.Entities;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Core.Modules.Animation.Systems.Control.Components;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Box;
using OpenBreed.Sandbox.Entities.Projectile;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Sandbox.Entities.Pickable;
using System.Linq;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Core.Modules.Animation.Helpers;
using OpenBreed.Sandbox.Entities.WorldGate;
using OpenBreed.Core.Common;

namespace OpenBreed.Sandbox.States
{
    /// <summary>
    /// Tech Demo Class: Projectiles
    /// </summary>
    public class StateTechDemo6 : BaseState
    {
        #region Public Fields

        public const string ID = "TECH_DEMO_6";

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

        public StateTechDemo6(ICore core)
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
            Console.WriteLine("---------- Projectile entities --------");
            Console.WriteLine("This demo shows projectiles in use.");
            Console.WriteLine("Constrols:");
            Console.WriteLine("RMB + Move mouse cursor = Camera control over hovered viewport");
            Console.WriteLine("Keyboard Right Ctrl = Shoot projectiles");
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
        public World GameWorld;

        #endregion Protected Methods

        #region Private Methods

        private void InitializeWorld()
        {
            GameWorld = GameWorldHelper.CreateGameWorld(Core, "DEMO6");

            var cameraBuilder = new CameraBuilder(Core);

            cameraBuilder.SetPosition(new Vector2(64, 64));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);

            GameCamera = cameraBuilder.Build();
            GameCamera.Add(new Animator(10.0f, false, -1, FrameTransition.LinearInterpolation));

            GameWorld.AddEntity(GameCamera);


            gameViewport = (Viewport)Core.Rendering.Viewports.Create(0.05f, 0.05f, 0.90f, 0.90f);
            gameViewport.CameraEntity = GameCamera;

            var actor = ActorHelper.CreateActor(Core, new Vector2(450, 288));
            actor.Tag = GameCamera;

            actor.Add(new WalkingControl());
            actor.Add(new AttackControl());

            actor.Add(TextHelper.Create(Core, new Vector2(0, 32), "Hero"));

            Core.Jobs.Execute(new CameraFollowJob(GameCamera, actor));

            var player1 = Core.Players.GetByName("P1");
            player1.AssumeControl(actor);
            var player2 = Core.Players.GetByName("P2");
            player2.AssumeControl(actor);

            var movementFsm = ActorHelper.CreateMovementFSM(actor);
            var atackFsm = ActorHelper.CreateAttackingFSM(actor);
            var rotateFsm = ActorHelper.CreateRotationFSM(actor);
            movementFsm.SetInitialState("Standing");
            atackFsm.SetInitialState("Idle");
            rotateFsm.SetInitialState("Idle");
            GameWorld.AddEntity(actor);

            var worldExit = WorldGateHelper.AddWorldExit(Core, GameWorld, 30, 10, "TECH_DEMO_5", 1);

            var teleportExit = TeleportHelper.AddTeleportExit(Core, GameWorld, 20, 10);
            TeleportHelper.AddTeleportEntry(Core, GameWorld, 10, 10, teleportExit.Id);

            var rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                PickableHelper.AddItem(Core, GameWorld, rnd.Next(5, 60), rnd.Next(5, 60));
            }

            SandBoxHelper.SetupMap(GameWorld);

        }

        #endregion Private Methods
    }
}