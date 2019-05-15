﻿using OpenBreed.Core;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Rendering;
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
    public class StateTechDemo1 : BaseState
    {
        #region Public Fields

        public const string Id = "TECH_DEMO_1";

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

        private static byte[] mapB = new byte[] {
            3,3,0,0,0,3,3,3,3,3,
            0,0,0,0,0,0,0,0,1,3,
            7,0,0,0,0,0,0,0,1,3,
            0,0,0,1,0,1,0,0,4,3,
            0,0,0,2,2,2,0,0,2,3,
            0,3,3,0,0,0,3,3,3,3
        };

        private ITexture tileTex;
        private ITexture spriteTex;
        private TileAtlas tileAtlas;
        private SpriteAtlas spriteAtlas;
        private Viewport viewportA;
        private Viewport viewportB;
        private Viewport viewportC;

        #endregion Private Fields

        #region Public Constructors

        public StateTechDemo1(ICore core)
        {
            Core = core;

            World = new World(Core);

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
            World.AddEntity(Camera1);

            cameraBuilder.SetPosition(new Vector2(64, 288));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            Camera2 = (Camera)cameraBuilder.Build();
            World.AddEntity(Camera2);

            viewportA = new Viewport(50, 50, 540, 380);
            viewportA.Camera = Camera1;

            viewportB = new Viewport(50, 50, 540, 380);
            viewportB.Camera = Camera2;

            viewportC = new Viewport(50, 50, 540, 380);
            viewportC.Camera = Camera2;

            Core.Worlds.Add(World);

            InitializeWorld();
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public Camera Camera1 { get; }

        public Camera Camera2 { get; }

        public override string Name { get { return Id; } }

        #endregion Public Properties

        #region Public Methods

        public override void OnResize(Rectangle clientRectangle)
        {
            base.OnResize(clientRectangle);

            viewportA.X = clientRectangle.X + 25;
            viewportA.Y = clientRectangle.Y + 25;
            viewportA.Width = clientRectangle.Width / 2 - 25;
            viewportA.Height = clientRectangle.Height / 2 - 25;

            viewportB.X = clientRectangle.X + 25 + clientRectangle.Width / 2;
            viewportB.Y = clientRectangle.Y + 25;
            viewportB.Width = clientRectangle.Width / 2 - 50;
            viewportB.Height = clientRectangle.Height / 2 - 25;

            viewportC.X = clientRectangle.X + 25;
            viewportC.Y = clientRectangle.Y + 25 + clientRectangle.Height / 2;
            viewportC.Width = clientRectangle.Width - 50;
            viewportC.Height = clientRectangle.Height - clientRectangle.Height / 2 - 50 ;
        }

        public override void ProcessInputs(FrameEventArgs e)
        {
            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Key.Escape))
                ChangeState(MenuState.Id);

            var mouseState = Mouse.GetState();

            Viewport hoverViewport = null;

            if (viewportA.TestScreenCoords(Core.CursorPos))
                hoverViewport = viewportA;
            else if (viewportB.TestScreenCoords(Core.CursorPos))
                hoverViewport = viewportB;
            else if (viewportC.TestScreenCoords(Core.CursorPos))
                hoverViewport = viewportC;
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
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnEnter()
        {
            Core.Viewports.Add(viewportA);
            Core.Viewports.Add(viewportB);
            Core.Viewports.Add(viewportC);

            Console.Clear();
            Console.WriteLine("---------- Viewports & Cameras --------");
            Console.WriteLine("This demo shows three viewports with two cameras attached to them.");
            Console.WriteLine("Constrols:");
            Console.WriteLine("RMB + Move mouse cursor = Camera control over hovered viewport");
            Console.WriteLine("Keyboard arrows  = Control arrow actor");
        }

        protected override void OnLeave()
        {
            Core.Viewports.Remove(viewportA);
            Core.Viewports.Remove(viewportB);
            Core.Viewports.Remove(viewportC);
        }

        #endregion Protected Methods

        #region Private Methods

        private void InitializeWorld()
        {
            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas(tileAtlas);

            var actorBuilder = new WorldActorBuilder(Core);
            actorBuilder.SetSpriteAtlas(spriteAtlas);
            actorBuilder.SetPosition(new OpenTK.Vector2(64, 288));
            actorBuilder.SetDirection(new OpenTK.Vector2(1, 0));

            actorBuilder.SetController(new CreatureController(Key.Up, Key.Down, Key.Left, Key.Right));

            World.AddEntity((WorldActor)actorBuilder.Build());

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

        private void InitializeWorldB()
        {
            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas(tileAtlas);

            var actorBuilder = new WorldActorBuilder(Core);
            actorBuilder.SetSpriteAtlas(spriteAtlas);
            actorBuilder.SetPosition(new OpenTK.Vector2(50, 20));
            actorBuilder.SetDirection(new OpenTK.Vector2(1, 0));

            actorBuilder.SetController(new CreatureController(Key.Up, Key.Down, Key.Left, Key.Right));
            World.AddEntity((WorldActor)actorBuilder.Build());

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
                        World.AddEntity((WorldBlock)blockBuilder.Build());
                    }
                }
            }
        }

        #endregion Private Methods
    }
}