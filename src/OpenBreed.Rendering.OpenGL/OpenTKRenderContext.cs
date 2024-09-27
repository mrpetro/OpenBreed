using Microsoft.Extensions.Logging;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace OpenBreed.Rendering.OpenGL
{
    public class OpenTKRenderContext : IRenderContext
    {
        #region Private Fields

        private readonly IGraphicsContext graphicsContext;
        private readonly HostCoordinateSystemConverter hostCoordinateSystemConverter;
        private readonly ILogger logger;
        private readonly IEventsMan eventsMan;
        private readonly List<RenderView> views = new List<RenderView>();

        #endregion Private Fields

        #region Public Constructors

        public OpenTKRenderContext(
            ILogger logger,
            IEventsMan eventsMan,
            IGraphicsContext graphicsContext,
            HostCoordinateSystemConverter hostCoordinateSystemConverter)
        {
            this.logger = logger;
            this.eventsMan = eventsMan;
            this.graphicsContext = graphicsContext;
            this.hostCoordinateSystemConverter = hostCoordinateSystemConverter;

            Primitives = new PrimitiveRenderer();
            Textures = new TextureMan(logger);
            Sprites = new SpriteMan(Textures, Primitives, logger);
            SpriteRenderer = new SpriteRenderer((SpriteMan)Sprites, Primitives);
            Fonts = new FontMan(Textures, Sprites, SpriteRenderer, Primitives);
            Tiles = new TileMan(Textures, logger, Primitives);
            Pictures = new PictureMan(Textures, Primitives, logger);
            PictureRenderer = new PictureRenderer((PictureMan)Pictures, Primitives);
            TileStamps = new StampMan(logger);
            Palettes = new PaletteMan(logger);

            Primitives.Load();
        }

        #endregion Public Constructors

        #region Public Properties

        public ITextureMan Textures { get; }
        public ISpriteMan Sprites { get; }
        public ISpriteRenderer SpriteRenderer { get; }
        public IPrimitiveRenderer Primitives { get; }
        public IPictureMan Pictures { get; }
        public IPictureRenderer PictureRenderer { get; }
        public ITileMan Tiles { get; }
        public IFontMan Fonts { get; }
        public IStampMan TileStamps { get; }
        public IPaletteMan Palettes { get; }

        #endregion Public Properties

        #region Public Methods

        public IRenderView CreateView(RenderDelegate viewRenderer, float minX = 0, float minY = 0, float maxX = 1, float maxY = 1)
        {
            var renderView = new RenderView(this, hostCoordinateSystemConverter, viewRenderer, new Box2(new Vector2(minX, minY), new Vector2(maxX, maxY)));
            views.Add(renderView);
            renderView.Reset();
            return renderView;
        }

        public void CursorDown(int cursorId, Vector2i point, CursorKeys cursorKey)
        {
            if (!TryGetView(point, out RenderView view))
            {
                return;
            }

            point = view.GetHostToViewCoords(point);
            eventsMan.Raise(new ViewCursorDownEvent(view, cursorId, point, cursorKey));
        }

        public void CursorUp(int cursorId, Vector2i point, CursorKeys cursorKey)
        {
            if (!TryGetView(point, out RenderView view))
            {
                return;
            }

            point = view.GetHostToViewCoords(point);
            eventsMan.Raise(new ViewCursorUpEvent(view, cursorId, point, cursorKey));
        }

        public void CursorEnter(int cursorId, Vector2i point)
        {
            if (!TryGetView(point, out RenderView view))
            {
                return;
            }

            point = view.GetHostToViewCoords(point);
            eventsMan.Raise(new ViewCursorEnterEvent(view, cursorId, point));
        }

        public void CursorLeave(int cursorId, Vector2i point)
        {
            if (!TryGetView(point, out RenderView view))
            {
                return;
            }
        }

        public void CursorMove(int cursorId, Vector2i point)
        {
            if (!TryGetView(point, out RenderView view))
            {
                return;
            }

            point = view.GetHostToViewCoords(point);
            eventsMan.Raise(new ViewCursorMoveEvent(view, cursorId, point));
        }

        public void CursorWheel(int cursorId, Vector2i point, int wheelDelta)
        {
            if (!TryGetView(point, out RenderView view))
            {
                return;
            }

            point = view.GetHostToViewCoords(point);
            eventsMan.Raise(new ViewCursorWheelEvent(view, cursorId, point, wheelDelta));
        }

        public void Render(float dt)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            foreach (var view in views)
            {
                view.OnRender(dt);
            }
        }

        public void Initialize()
        {



            eventsMan.Raise(new RenderContextInitializedEvent(this));
        }

        public void Resize(int width, int height)
        {
            foreach (var view in views)
            {
                view.OnResize(width, height);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private bool TryGetView(Vector2i point, out RenderView view)
        {
            view = views.FirstOrDefault(v => v.Box.ContainsInclusive(point));
            return view is not null;
        }

        #endregion Private Methods
    }
}