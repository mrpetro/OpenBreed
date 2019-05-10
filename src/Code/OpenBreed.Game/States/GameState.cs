using OpenBreed.Game.Entities;
using OpenBreed.Game.Entities.Builders;
using OpenBreed.Game.Rendering;
using OpenBreed.Game.Rendering.Helpers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OpenBreed.Game.States
{
    public class GameState : BaseState
    {
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


        #region Public Fields

        public const string Id = "GAME";
        public Texture TestTexture;
        public World WorldA;
        public World WorldB;

        #endregion Public Fields

        #region Private Fields

        private Texture tileTex;
        private Texture spriteTex;
        private TileAtlas tileAtlas;
        private SpriteAtlas spriteAtlas;

        private readonly List<Viewport> viewports = new List<Viewport>();
        private int px;
        private int py;
        private Viewport viewportLeft;
        private Viewport viewportRight;

        #endregion Private Fields

        #region Public Constructors

        public GameState()
        {
            TextureMan = new TextureMan();
            EntityMan = new EntityMan();

            WorldA = new World(this);
            WorldB = new World(this);
            var cameraBuilder = new CameraBuilder(this);

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

            AddViewport(viewportLeft);
            AddViewport(viewportRight);

            Console.WriteLine("LMB + Move = Left camera control");
            Console.WriteLine("RMB + Move = Right camera control");
            Console.WriteLine("Keyboard arrows  = Control arrow actor");
        }

        #endregion Public Constructors

        #region Public Properties

        public Camera Camera1 { get; }
        public Camera Camera2 { get; }
        public EntityMan EntityMan { get; }
        public override string Name { get { return Id; } }
        public TextureMan TextureMan { get; }

        #endregion Public Properties

        #region Public Methods

        public void AddViewport(Viewport viewport)
        {
            if (viewports.Contains(viewport))
                throw new InvalidOperationException("Viewport already added.");

            viewports.Add(viewport);
        }

        private void InitializeWorldA()
        {
            var blockBuilder = new WorldBlockBuilder(this);
            blockBuilder.SetTileAtlas(tileAtlas);

            var actorBuilder = new WorldActorBuilder(this);
            actorBuilder.SetSpriteAtlas(spriteAtlas);
            actorBuilder.SetPosition(new OpenTK.Vector2(20, 20));
            actorBuilder.SetDirection(new OpenTK.Vector2(1, 0));

            actorBuilder.SetController(new Control.Components.MovementController());
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
            var blockBuilder = new WorldBlockBuilder(this);
            blockBuilder.SetTileAtlas(tileAtlas);

            var actorBuilder = new WorldActorBuilder(this);
            actorBuilder.SetSpriteAtlas(spriteAtlas);
            actorBuilder.SetPosition(new OpenTK.Vector2(50, 20));
            actorBuilder.SetDirection(new OpenTK.Vector2(1, 0));

            actorBuilder.SetController(new Control.Components.MovementController());
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


        public override void OnLoad()
        {
            base.OnLoad();

            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //GL.PatchParameter(PatchParameterInt.PatchVertices, 3);

            TestTexture = TextureMan.Load(@"Content\TexTest32bit.bmp");

            WorldA.Initialize();
            WorldB.Initialize();

            InitializeWorldA();
            InitializeWorldB();


            //TestTexture = TextureMan.Load(@"Content\TexTest24bit.bmp");
            //TestTexture = TextureMan.Load(@"Content\TexTest8bitIndexed.bmp");
            //TestTexture = TextureMan.Load(@"Content\TexTest4bitIndexed.bmp");

            //GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);                  // Select The Type Of Blending

            //GL.Enable(EnableCap.StencilTest);
            GL.ClearStencil(0x0);
            GL.StencilMask(0xFFFFFFFF);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactor.SrcAlpha,BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
        }

        public override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            GL.PushMatrix();

            for (int i = 0; i < viewports.Count; i++)
                viewports[i].Draw();

            GL.PopMatrix();
        }

        public override void OnResize(Rectangle clientRectangle)
        {
            base.OnResize(clientRectangle);

            GL.LoadIdentity();
            GL.Viewport(0, 0, clientRectangle.Width, clientRectangle.Height);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Ortho(0, clientRectangle.Width, 0, clientRectangle.Height, 0, 1); // Origin in lower-left corner

            viewportLeft.X = clientRectangle.X + 25;
            viewportLeft.Y = clientRectangle.Y + 25;
            viewportLeft.Width = clientRectangle.Width / 2 - 50;
            viewportLeft.Height = clientRectangle.Height - 50;

            viewportRight.X = clientRectangle.X + 25 + clientRectangle.Width / 2;
            viewportRight.Y = clientRectangle.Y + 25;
            viewportRight.Width = clientRectangle.Width / 2 - 50;
            viewportRight.Height = clientRectangle.Height - 50;
        }

        public override void OnUpdate(FrameEventArgs e)
        {
            base.OnUpdate(e);

            WorldA.Update((float)e.Time);
            WorldB.Update((float)e.Time);
        }

        public override void ProcessInputs(FrameEventArgs e)
        {
            WorldA.ProcessInputs(e.Time);
            WorldB.ProcessInputs(e.Time);

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

        public void RemoveViewport(Viewport viewport)
        {
            viewports.Remove(viewport);
        }

        #endregion Public Methods
    }
}