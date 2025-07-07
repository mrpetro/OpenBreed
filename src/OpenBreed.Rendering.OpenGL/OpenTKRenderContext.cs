using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenBreed.Common.Tools.Collections;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions.Renderers;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace OpenBreed.Rendering.OpenGL
{
    public class OpenTKRenderContext : IRenderContext
    {
        #region Private Fields

        private static int nextId = 0;
        private readonly IGraphicsContext graphicsContext;
        private readonly Action<IGraphicsContext> deinitializeCallback;
        private readonly HostCoordinateSystemConverter hostCoordinateSystemConverter;
        private readonly ILogger logger;
        private readonly IEventsMan eventsMan;
        private readonly IServiceScope serviceScope;
        private readonly IdMap<RenderView> views = new IdMap<RenderView>();
        private int id = nextId++;

        #endregion Private Fields

        #region Public Constructors

        public OpenTKRenderContext(
            ILogger logger,
            IEventsMan eventsMan,
            IPaletteMan paletteMan,
            IStampMan stampMan,
            IServiceScopeFactory serviceScopeFactory,
            IGraphicsContext graphicsContext,
            Action<IGraphicsContext> destroyCallback,
            HostCoordinateSystemConverter hostCoordinateSystemConverter)
        {
            this.logger = logger;
            this.eventsMan = eventsMan;
            this.serviceScope = serviceScopeFactory.CreateScope();
            this.graphicsContext = graphicsContext;
            this.deinitializeCallback = destroyCallback ?? throw new ArgumentNullException(nameof(destroyCallback));
            this.hostCoordinateSystemConverter = hostCoordinateSystemConverter;
            Palettes = paletteMan;
            TileStamps = stampMan;

            Primitives = ServiceProvider.GetRequiredService<IPrimitiveRenderer>();
            Textures = ServiceProvider.GetRequiredService<ITextureMan>();
            Sprites = ServiceProvider.GetRequiredService<ISpriteMan>();
            SpriteRenderer = ServiceProvider.GetRequiredService<ISpriteRenderer>();
            TileRenderer = ServiceProvider.GetRequiredService<ITileRenderer>();
            Fonts = ServiceProvider.GetRequiredService<IFontMan>();
            Tiles = ServiceProvider.GetRequiredService<ITileMan>();
            Pictures = ServiceProvider.GetRequiredService<IPictureMan>();
            PictureRenderer = ServiceProvider.GetRequiredService<IPictureRenderer>();

            Primitives.Load();
        }

        #endregion Public Constructors

        #region Public Properties

        public IServiceProvider ServiceProvider => serviceScope.ServiceProvider;

        public ITextureMan Textures { get; }
        public ISpriteMan Sprites { get; }
        public ISpriteRenderer SpriteRenderer { get; }
        public ITileRenderer TileRenderer { get; }
        public IPrimitiveRenderer Primitives { get; }
        public IPictureMan Pictures { get; }
        public IPictureRenderer PictureRenderer { get; }
        public ITileMan Tiles { get; }
        public IFontMan Fonts { get; }
        public IStampMan TileStamps { get; }
        public IPaletteMan Palettes { get; }

        public IEnumerable<IRenderView> Views => views.Items;

        #endregion Public Properties

        #region Public Methods

        public IRenderView CreateView(float minX = 0, float minY = 0, float maxX = 1, float maxY = 1)
        {
            var newId = views.NewId();

            var renderView = new RenderView(this, hostCoordinateSystemConverter, new Box2(new Vector2(minX, minY), new Vector2(maxX, maxY)), newId);

            views.Add(renderView);
            renderView.Reset();
            return renderView;
        }

        public void RemoveView(IRenderView renderView)
        {
            if (renderView.Context != this)
            {
                throw new InvalidOperationException("Trying to remove view from incorrect render context.");
            }

            views.RemoveById(renderView.Id);
        }

        public void CursorDown(int cursorId, Vector2i point, CursorKey cursorKey)
        {
            if (!TryGetView(point, out RenderView view))
            {
                return;
            }

            point = view.FromHostPoint(point);

            view.OnCursorDown(cursorId, point, cursorKey);
            eventsMan.Raise(new ViewCursorDownEvent(view, cursorId, point, cursorKey));
        }

        public void CursorUp(int cursorId, Vector2i point, CursorKey cursorKey)
        {
            if (!TryGetView(point, out RenderView view))
            {
                return;
            }

            point = view.FromHostPoint(point);

            view.OnCursorUp(cursorId, point, cursorKey);
            eventsMan.Raise(new ViewCursorUpEvent(view, cursorId, point, cursorKey));
        }

        public void KeyDown(Abstractions.Events.Keys key, Abstractions.Events.KeyModifiers modifiers)
        {
            foreach (var view in views.Items)
            {
                view.OnKeyDown(key, modifiers);
            }
        }

        public void KeyUp(Abstractions.Events.Keys key, Abstractions.Events.KeyModifiers modifiers)
        {
            foreach (var view in views.Items)
            {
                view.OnKeyUp(key, modifiers);
            }
        }

        public void CursorEnter(int cursorId, Vector2i point)
        {
            if (!TryGetView(point, out RenderView view))
            {
                return;
            }

            point = view.FromHostPoint(point);

            view.OnCursorEnter(cursorId, point);
            eventsMan.Raise(new ViewCursorEnterEvent(view, cursorId, point));
        }

        public void CursorLeave(int cursorId, Vector2i point)
        {
            if (!TryGetView(point, out RenderView view))
            {
                return;
            }

            point = view.FromHostPoint(point);

            view.OnCursorLeave(cursorId, point);
            eventsMan.Raise(new ViewCursorLeaveEvent(view, cursorId, point));
        }

        public void CursorMove(int cursorId, Vector2i point)
        {
            if (!TryGetView(point, out RenderView view))
            {
                return;
            }

            point = view.FromHostPoint(point);

            view.OnCursorMove(cursorId, point);
            eventsMan.Raise(new ViewCursorMoveEvent(view, cursorId, point));
        }

        public void CursorWheel(int cursorId, Vector2i point, int wheelDelta)
        {
            if (!TryGetView(point, out RenderView view))
            {
                return;
            }

            point = view.FromHostPoint(point);

            view.OnCursorWheel(cursorId, point, wheelDelta);
            eventsMan.Raise(new ViewCursorWheelEvent(view, cursorId, point, wheelDelta));
        }

        public void Render(float dt)
        {
            LoadRefresh();

            GL.ClearDepth(1.0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            foreach (var view in views.Items)
            {
                view.OnRender(dt);
            }
        }

        public void TextInput(string text)
        {
            foreach (var view in views.Items)
            {
                view.OnTextInput(text);
            }
        }

        public void Initialize()
        {
            eventsMan.Raise(new RenderContextInitializedEvent(this));
        }

        public void Deinitialize()
        {
            deinitializeCallback.Invoke(graphicsContext);
        }

        public void Resize(int width, int height)
        {
            foreach (var view in views.Items)
            {
                view.OnResize(width, height);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadRefresh()
        {
            Textures.LoadRefresh(this);
            Tiles.LoadRefresh(this);
            Sprites.LoadRefresh(this);
            Fonts.LoadRefresh(this);
            Pictures.LoadRefresh(this);
        }

        private bool TryGetView(Vector2i point, out RenderView view)
        {
            view = views.Items.FirstOrDefault(v => v.Box.ContainsInclusive(point));
            return view is not null;
        }

        #endregion Private Methods
    }
}