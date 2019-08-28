using OpenBreed.Core;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Game.Commands;
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
    /// Tech Demo Class: Pathfinding
    /// </summary>
    public class StateTechDemo3 : BaseState
    {
        #region Public Fields

        public const string ID = "TECH_DEMO_3";

        public World World;

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

        private IEntity actor;
        private Viewport viewport;

        #endregion Private Fields

        #region Public Constructors

        public StateTechDemo3(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public CameraEntity Camera1 { get; private set; }

        public override string Id { get { return ID; } }

        #endregion Public Properties

        #region Public Methods

        public override void OnResize(Rectangle clientRectangle)
        {
            base.OnResize(clientRectangle);

            viewport.X = clientRectangle.X + 25;
            viewport.Y = clientRectangle.Y + 25;
            viewport.Width = clientRectangle.Width  - 50;
            viewport.Height = clientRectangle.Height - 50;
        }

        public override void Update(float dt)
        {
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            Viewport hoverViewport = null;

            if (viewport.TestScreenCoords(Core.Inputs.CursorPos))
                hoverViewport = viewport;
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

                if (mouseState.IsButtonDown(MouseButton.Left))
                {
                    var worldCoords = hoverViewport.GetWorldCoords(Core.Inputs.CursorPos);
                    var moveToCommand = new MoveToCommand(actor, worldCoords);
                    moveToCommand.Execute();
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

        private void DeinitializeWorld()
        {
            World.RemoveAllEntities();
            Core.Worlds.Remove(World);
            Core.Rendering.Viewports.Remove(viewport);
            Core.Inputs.KeyDown -= Inputs_KeyDown;
        }

        protected override void OnEnter()
        {
            InitializeWorld();

            Console.Clear();
            Console.WriteLine("---------- Pathfinding --------");
            Console.WriteLine("This demo shows three viewports with two cameras attached to them.");
            Console.WriteLine("Constrols:");
            Console.WriteLine("RMB + Move mouse cursor = Camera control over hovered viewport");
            Console.WriteLine("Keyboard arrows  = Control arrow actor");
        }

        protected override void OnLeave()
        {
            DeinitializeWorld();
        }

        #endregion Protected Methods

        #region Private Methods

        private void InitializeWorld()
        {
            World = new GameWorld(Core);

            var cameraBuilder = new CameraBuilder(Core);

            //Resources
            cameraBuilder.SetPosition(new Vector2(64, 64));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            Camera1 = (CameraEntity)cameraBuilder.Build();
            World.AddEntity(Camera1);


            viewport = (Viewport)Core.Rendering.Viewports.Create(50, 50, 540, 380);
            viewport.Camera = Camera1;

            Core.Worlds.Add(World);

            actor = ActorHelper.CreateActor(Core, new Vector2(64, 288));
            actor.Add(new AiControl());

            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas("Atlases/Tiles/16/Test");

            var stateMachine = ActorHelper.CreateMovementFSM(actor);
            stateMachine.SetInitialState("Standing_Right");

            World.AddEntity(actor);

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
                        World.AddEntity(blockBuilder.Build());
                    }
                }
            }

            Core.Inputs.KeyDown += Inputs_KeyDown;
            Core.Rendering.Viewports.Add(viewport);
        }

        #endregion Private Methods
    }
}