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
using OpenBreed.Game.Commands;
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
    /// Tech Demo Class: Pathfinding
    /// </summary>
    public class StateTechDemo3 : BaseState
    {
        #region Public Fields

        public const string Id = "TECH_DEMO_3";

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

        private WorldActor actor;
        private ITexture tileTex;
        private ITexture spriteTex;
        private TileAtlas tileAtlas;
        private SpriteAtlas spriteAtlas;
        private Viewport viewport;

        #endregion Private Fields

        #region Public Constructors

        public StateTechDemo3(ICore core)
        {
            Core = core;

            InitializeWorld();
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public Camera Camera1 { get; private set; }

        public override string Name { get { return Id; } }

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

        public override void ProcessInputs(FrameEventArgs e)
        {
            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Key.Escape))
                ChangeState(MenuState.Id);

            var mouseState = Mouse.GetState();

            Viewport hoverViewport = null;

            if (viewport.TestScreenCoords(Core.CursorPos))
                hoverViewport = viewport;
            else
                hoverViewport = null;

            if (hoverViewport != null)
            {

                var z = 1 + ((float)mouseState.Scroll.Y) / 20.0f;

                if (z == 0)
                    z = 1.0f;



                if (mouseState.IsButtonDown(MouseButton.Right))
                {
                    var transf = hoverViewport.Camera.GetTransform();
                    transf.Invert();
                    var delta4 = Vector4.Transform(transf, new Vector4(Core.CursorDelta));
                    var delta2 = new Vector2(-delta4.X, -delta4.Y);
                    hoverViewport.Camera.Zoom = z;
                    hoverViewport.Camera.Position += delta2;
                }

                if (mouseState.IsButtonDown(MouseButton.Left))
                {
                    var worldCoords = hoverViewport.GetWorldCoords(Core.CursorPos);

                    //var transf = hoverViewport.Camera.GetTransform();
                    //var worldPos4 = Vector4.Transform(transf, new Vector4(Core.CursorPos));
                    //var worldPos2 = new Vector2(worldPos4.X, worldPos4.Y);

                    var moveToCommand = new MoveToCommand(actor, worldCoords);
                    moveToCommand.Execute();
                }
             }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnEnter()
        {
            Core.Viewports.Add(viewport);

            Console.Clear();
            Console.WriteLine("---------- Pathfinding --------");
            Console.WriteLine("This demo shows three viewports with two cameras attached to them.");
            Console.WriteLine("Constrols:");
            Console.WriteLine("RMB + Move mouse cursor = Camera control over hovered viewport");
            Console.WriteLine("Keyboard arrows  = Control arrow actor");
        }

        protected override void OnLeave()
        {
            Core.Viewports.Remove(viewport);
        }

        #endregion Protected Methods

        #region Private Methods

        private void InitializeWorld()
        {
            World = new World(Core);

            var cameraBuilder = new CameraBuilder(Core);

            //Resources
            tileTex = Core.Rendering.GetTexture(@"Content\TileAtlasTest32bit.bmp");
            spriteTex = Core.Rendering.GetTexture(@"Content\ArrowSpriteSet.png");
            tileAtlas = new TileAtlas(tileTex, 16, 4, 4);
            spriteAtlas = new SpriteAtlas(spriteTex, 32, 8, 1);

            cameraBuilder.SetPosition(new Vector2(64, 64));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            Camera1 = (Camera)cameraBuilder.Build();
            World.AddEntity(Camera1);


            viewport = new Viewport(50, 50, 540, 380);
            viewport.Camera = Camera1;

            Core.Worlds.Add(World);

            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas(tileAtlas);

            var actorBuilder = new WorldActorBuilder(Core);
            actorBuilder.SetSpriteAtlas(spriteAtlas);
            actorBuilder.SetSprite(new AIControllerDebug(new Sprite(spriteAtlas)));
            actorBuilder.SetPosition(new DynamicPosition(64, 288));
            actorBuilder.SetDirection(new Direction(1, 0));
            actorBuilder.SetShape(new AxisAlignedBoxShape(32, 32));
            actorBuilder.SetAnimator(new SpriteAnimator());
            actorBuilder.SetMovement(new CreatureMovement());
            actorBuilder.SetBody(new DynamicBody());

            actorBuilder.SetController(new AIController());

            actor = (WorldActor)actorBuilder.Build();
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
                        World.AddEntity((WorldBlock)blockBuilder.Build());
                    }
                }
            }
        }

        #endregion Private Methods
    }
}