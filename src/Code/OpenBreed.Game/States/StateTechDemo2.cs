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
    public class StateTechDemo2 : BaseState
    {
        #region Public Fields

        public const string Id = "TECH_DEMO_2";

        public World WorldA;

        public World WorldB;

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

        private Texture tileTex;
        private Texture spriteTex;
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

            TextureMan = new TextureMan();
            EntityMan = new EntityMan();

            WorldA = new World(Core);
            WorldB = new World(Core);
            var cameraBuilder = new CameraBuilder(Core);

            //Resources
            tileTex = TextureMan.Load(@"Content\TileAtlasTest32bit.bmp");
            spriteTex = TextureMan.Load(@"Content\ArrowSpriteSet.png");
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

            InitializeWorldA();
            InitializeWorldB();

            Console.WriteLine("LMB + Move = Left camera control");
            Console.WriteLine("RMB + Move = Right camera control");
            Console.WriteLine("Keyboard arrows  = Control arrow actor");
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public Camera Camera1 { get; }

        public Camera Camera2 { get; }

        public EntityMan EntityMan { get; }

        public override string Name { get { return Id; } }

        public TextureMan TextureMan { get; }

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
                ChangeState(MenuState.Id);

            var mouseState = Mouse.GetState();

            int dx = mouseState.X - px;
            int dy = mouseState.Y - py;

            var z = 1 + ((float)mouseState.Scroll.Y) / 20.0f;

            if (z == 0)
                z = 1.0f;

            if (mouseState.IsButtonDown(MouseButton.Left))
            {
                Camera1.Zoom = z;
                Camera1.Position = Vector2.Subtract(Camera1.Position, new Vector2(dx, -dy));
            }
            else if (mouseState.IsButtonDown(MouseButton.Right))
            {
                Camera2.Zoom = z;
                Camera2.Position = Vector2.Subtract(Camera2.Position, new Vector2(dx, -dy));
            }

            px = mouseState.X;
            py = mouseState.Y;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnEnter()
        {
            Core.Viewports.Add(viewportLeft);
            Core.Viewports.Add(viewportRight);
        }

        protected override void OnLeave()
        {
            Core.Viewports.Remove(viewportLeft);
            Core.Viewports.Remove(viewportRight);
        }

        #endregion Protected Methods

        #region Private Methods

        private void InitializeWorldA()
        {
            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas(tileAtlas);

            var actorBuilder = new WorldActorBuilder(Core);
            actorBuilder.SetSpriteAtlas(spriteAtlas);
            actorBuilder.SetPosition(new OpenTK.Vector2(20, 20));
            actorBuilder.SetDirection(new OpenTK.Vector2(1, 0));

            actorBuilder.SetController(new CreatureController());
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
                        blockBuilder.SetIndices(x + 5, y + 5);
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
            actorBuilder.SetPosition(new OpenTK.Vector2(50, 20));
            actorBuilder.SetDirection(new OpenTK.Vector2(1, 0));

            actorBuilder.SetController(new CreatureController());
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