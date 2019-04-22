using OpenBreed.Game.Entities;
using OpenBreed.Game.Entities.Builders;
using OpenBreed.Game.Physics;
using OpenBreed.Game.Rendering;
using OpenBreed.Game.Rendering.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Collections.Generic;
using System.Drawing;

namespace OpenBreed.Game.States
{
    public class GameState : BaseState
    {
        #region Public Fields

        public const string Id = "GAME";
        public Texture TestTexture;
        public World World;

        #endregion Public Fields

        #region Private Fields

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

            World = new World(this);

            var cameraBuilder = new WorldCameraBuilder(this);

            cameraBuilder.SetPosition(new Vector2(16, 32));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            Camera1 = (Camera)cameraBuilder.Build();

            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1);
            Camera2 = (Camera)cameraBuilder.Build();


            viewportLeft = new Viewport(50, 50, 540, 380);
            viewportLeft.Camera = Camera1;

            viewportRight = new Viewport(50, 50, 540, 380);
            viewportRight.Camera = Camera2;

            World.RenderSystem.AddViewport(viewportLeft);
            World.RenderSystem.AddViewport(viewportRight);
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

        public override void OnLoad()
        {
            base.OnLoad();

            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //GL.PatchParameter(PatchParameterInt.PatchVertices, 3);

            TestTexture = TextureMan.Load(@"Content\TexTest32bit.bmp");

            World.Initialize();


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

            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            //GL.Enable(EnableCap.Texture2D);
            //GL.BindTexture(TextureTarget.Texture2D, TestTexture.Id);

            World.OnRenderFrame(e);
            //GL.Disable(EnableCap.Texture2D);

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

            //View.Update();
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
                Camera1.Position = Vector2.Add(Camera1.Position, new Vector2(dx, -dy));
            }
            else if (mouseState.IsButtonDown(MouseButton.Right))
            {
                Camera2.Zoom = z;
                Camera2.Position = Vector2.Add(Camera2.Position, new Vector2(dx, -dy));
            }

            px = mouseState.X;
            py = mouseState.Y;
        }

        #endregion Public Methods
    }
}