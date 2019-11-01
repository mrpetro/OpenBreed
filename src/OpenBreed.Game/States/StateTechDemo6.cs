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
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Game.Entities.Pickable;

namespace OpenBreed.Game.States
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

            var actor = ActorHelper.CreateActor(Core, new Vector2(64, 288));
            actor.Add(new WalkingControl());
            actor.Add(new AttackControl());
            actor.Add(new InventoryComponent(new Bag[] { new Bag() }));

            actor.Add(TextHelper.Create(Core, new Vector2(-10, 10), "Hero"));

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

            var rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                PickableHelper.AddItem(Core, GameWorld, rnd.Next(5, 60), rnd.Next(5, 60));
            }

            SandBoxHelper.SetupMap(GameWorld);

            Core.Worlds.Add(GameWorld);
        }

        #endregion Private Methods
    }
}