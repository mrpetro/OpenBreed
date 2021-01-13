using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core;

//using OpenBreed.Core.Extensions;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Core.Systems;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Systems.Rendering.Events;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL
{
    public class OpenGLModule : BaseCoreModule, IRenderModule
    {
        #region Public Constructors

        public OpenGLModule(ICore core) : base(core)
        {
            Textures = core.GetManager<ITextureMan>();
            Tiles = core.GetManager<ITileMan>();
            Sprites = core.GetManager<ISpriteMan>();
            Stamps = core.GetManager<IStampMan>();
            Fonts = core.GetManager<IFontMan>();
        }

        #endregion Public Constructors

        #region Public Properties

        public ITextureMan Textures { get; }

        public ISpriteMan Sprites { get; }

        public IStampMan Stamps { get; }

        public ITileMan Tiles { get; }

        public IFontMan Fonts { get; }

        public World ScreenWorld { get; set; }

        public float Fps { get; private set; }

        #endregion Public Properties

        #region Private Properties

        private Box2 ClipBox { get { return Box2.FromTLRB(Core.ClientRectangle.Width, 0.0f, Core.ClientRectangle.Height, 0.0f); } }

        #endregion Private Properties

        #region Public Methods

        public static void AddManagers(IManagerCollection manCollection)
        {
            manCollection.AddSingleton<ITextureMan>(() => new TextureMan());

            manCollection.AddSingleton<ITileMan>(() => new TileMan(manCollection.GetManager<ITextureMan>()));

            manCollection.AddSingleton<ISpriteMan>(() => new SpriteMan(manCollection.GetManager<ITextureMan>(),
                                                                       manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<IStampMan>(() => new StampMan());

            manCollection.AddSingleton<IFontMan>(() => new FontMan(manCollection.GetManager<ITextureMan>()));
        }

        public void Draw(float dt)
        {
            Fps = 1.0f / dt;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            try
            {
                GL.PushMatrix();
                ScreenWorld?.Systems.OfType<IRenderableSystem>().ForEach(item => item.Render(ClipBox, 0, dt));
            }
            finally
            {
                GL.PopMatrix();
            }
        }

        public void Cleanup()
        {
        }

        public void OnClientResized(float width, float height)
        {
            Core.Events.Raise(this, new ClientResizedEventArgs(width, height));
        }

        public void DrawUnitRectangle()
        {
            RenderTools.DrawUnitRectangle();
        }

        public void DrawRectangle(Box2 clipBox)
        {
            RenderTools.DrawRectangle(clipBox);
        }

        public void DrawBox(Box2 clipBox)
        {
            RenderTools.DrawBox(clipBox);
        }

        public void DrawUnitBox()
        {
            RenderTools.DrawUnitBox();
        }

        #endregion Public Methods
    }
}