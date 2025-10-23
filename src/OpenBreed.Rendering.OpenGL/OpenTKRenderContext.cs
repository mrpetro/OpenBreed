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
using OpenBreed.Rendering.OpenGL.Renderers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections;
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

        private readonly IGraphicsContext graphicsContext;
        private readonly Action<IGraphicsContext> deinitializeCallback;
        private readonly HostCoordinateSystemConverter hostCoordinateSystemConverter;
        private readonly ILogger logger;
        private readonly IEventsMan eventsMan;
        private readonly IServiceScope serviceScope;
        private readonly IdMap<RenderView> views = new IdMap<RenderView>();
        private readonly HashSet<RenderView> activeViews = new HashSet<RenderView>();

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

            FontRenderer = ServiceProvider.GetRequiredService<IFontRenderer>();
            SpriteRenderer = ServiceProvider.GetRequiredService<ISpriteRenderer>();
            TileRenderer = ServiceProvider.GetRequiredService<ITileRenderer>();
            PictureRenderer = ServiceProvider.GetRequiredService<IPictureRenderer>();
            Primitives = ServiceProvider.GetRequiredService<IPrimitiveRenderer>();

            Primitives.Load();
        }

        #endregion Public Constructors

        #region Public Properties

        public IServiceProvider ServiceProvider => serviceScope.ServiceProvider;

        public IFontRenderer FontRenderer { get; }
        public ISpriteRenderer SpriteRenderer { get; }
        public ITileRenderer TileRenderer { get; }
        public IPrimitiveRenderer Primitives { get; }
        public IPictureRenderer PictureRenderer { get; }
        public IStampMan TileStamps { get; }
        public IPaletteMan Palettes { get; }

        public Vector2i Size { get; private set; }

        public IEnumerable<IRenderView> Views => views.Items;
        public IEnumerable<IRenderView> ActiveViews => activeViews;

        #endregion Public Properties

        #region Public Methods

        public IRenderView CreateView(float minX = 0, float minY = 0, float maxX = 1, float maxY = 1, bool activate = true)
        {
            var newId = views.NewId();

            var renderView = new RenderView(this, hostCoordinateSystemConverter, new Box2(new Vector2(minX, minY), new Vector2(maxX, maxY)), newId);

            views.Add(renderView);
            renderView.Reset();

            renderView.OnResize();

            if (activate)
            {
                if (!renderView.Activate())
                {
                    throw new InvalidOperationException("Expected new render view to be activated.");
                }
            }

            return renderView;
        }

        public void RemoveView(IRenderView renderView)
        {
            if (renderView.Context != this)
            {
                throw new InvalidOperationException("Trying to remove view from incorrect render context.");
            }

            activeViews.Remove((RenderView)renderView);
            views.RemoveById(renderView.Id);
        }

        public void CursorDown(int cursorId, Vector2i point, CursorKey cursorKey, Abstractions.Events.KeyModifiers modifiers)
        {
            if (!TryGetView(point, out RenderView view))
            {
                return;
            }

            point = view.FromHostPoint(point);

            view.OnCursorDown(cursorId, point, cursorKey);
            eventsMan.Raise(new ViewCursorDownEvent(view, cursorId, point, cursorKey, modifiers));
        }

        public void CursorUp(int cursorId, Vector2i point, CursorKey cursorKey, Abstractions.Events.KeyModifiers modifiers)
        {
            if (!TryGetView(point, out RenderView view))
            {
                return;
            }

            point = view.FromHostPoint(point);

            view.OnCursorUp(cursorId, point, cursorKey);
            eventsMan.Raise(new ViewCursorUpEvent(view, cursorId, point, cursorKey, modifiers));
        }

        public void KeyDown(Abstractions.Events.Keys key, Abstractions.Events.KeyModifiers modifiers)
        {
            foreach (var view in activeViews)
            {
                view.OnKeyDown(key, modifiers);
                eventsMan.Raise(new ViewKeyDownEvent(view, key, modifiers));
            }
        }

        public void KeyUp(Abstractions.Events.Keys key, Abstractions.Events.KeyModifiers modifiers)
        {
            foreach (var view in activeViews)
            {
                view.OnKeyUp(key, modifiers);
                eventsMan.Raise(new ViewKeyUpEvent(view, key, modifiers));
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

        public void CursorMove(int cursorId, Vector2i point, BitArray cursorKeyStates, Abstractions.Events.KeyModifiers modifiers)
        {
            if (!TryGetView(point, out RenderView view))
            {
                return;
            }

            point = view.FromHostPoint(point);

            view.OnCursorMove(cursorId, point, cursorKeyStates, modifiers);
            eventsMan.Raise(new ViewCursorMoveEvent(view, cursorId, cursorKeyStates, point));
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
            GL.ClearDepth(1.0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            foreach (var view in activeViews)
            {
                view.OnRender(dt);
            }
        }

        public void TextInput(string text)
        {
            foreach (var view in activeViews)
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
            Size = new Vector2i(width, height);

            foreach (var view in activeViews)
            {
                view.OnResize();
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal bool ActivateView(RenderView renderView)
        {
            return activeViews.Add(renderView);
        }

        internal bool DeactivateView(RenderView renderView)
        {
            return activeViews.Remove(renderView);
        }

        #endregion Internal Methods

        #region Private Methods

        private bool TryGetView(Vector2i point, out RenderView view)
        {
            view = activeViews.FirstOrDefault(v => v.Box.ContainsInclusive(point));
            return view is not null;
        }

        #endregion Private Methods
    }
}