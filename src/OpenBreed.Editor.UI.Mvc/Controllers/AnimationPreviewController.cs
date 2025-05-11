using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Editor.UI.Mvc.Models;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Rendering.Abstractions.Events;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using OpenBreed.Editor.UI.Mvc.Views;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Rendering.Abstractions.Extensions;
using System.Drawing;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Editor.UI.Mvc.Extensions;
using OpenBreed.Common;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Common.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Wecs.Components.Animation;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems;
using OpenBreed.Rendering.OpenGL.Managers;

namespace OpenBreed.Editor.UI.Mvc.Controllers
{
    public class AnimationPreviewController : IController
    {
        #region Private Fields

        private const int cellSize = 16;
        private readonly EditorView view;
        private readonly IClip<IEntity> model;
        private readonly IPaletteMan paletteMan;
        private readonly IWorldMan worldMan;
        private readonly IBuilderFactory builderFactory;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly IEntityMan entityMan;
        private readonly IUpdater updater;
        private IWorld animationWorld;
        private IPalette palette;
        private bool pendingReset;

        #endregion Private Fields

        #region Public Constructors

        public AnimationPreviewController(
            IEventsMan eventsMan,
            EditorView view,
            IClip<IEntity> model,
            IPaletteMan paletteMan,
            IWorldMan worldMan,
            IBuilderFactory builderFactory,
            PalettesDataProvider palettesDataProvider,
            IUpdaterFactory updaterFactory,
            IEntityMan entityMan)
        {
            this.view = view;
            this.model = model;
            this.paletteMan = paletteMan;
            this.worldMan = worldMan;
            this.builderFactory = builderFactory;
            this.palettesDataProvider = palettesDataProvider;
            this.entityMan = entityMan;

            view.Rendering += OnRender;
            view.CursorDown += OnCursorDown;

            LoadPalettes();
            SetupWorld();

            updater = updaterFactory.CreateUpdater(60.0f, OnUpdate);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Reset()
        {
            view.SetScaleLimits(1.0f / (float)Math.Pow(2, 8), (float)Math.Pow(2, 8));
            view.Reset();
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadPalettes()
        {
            var commonPaletteModel = palettesDataProvider.GetPalette("Palettes.COMMON");

            var builder = paletteMan.CreatePalette()
                .SetLength(256)
                .SetName("GamePalette")
                .SetColors(commonPaletteModel.Data.Select(color => color.ToColor4()).ToArray());

            var cb = commonPaletteModel[0];
            builder.SetColor(0, new Color4(cb.R / 255.0f, cb.G / 255.0f, cb.B / 255.0f, 0.0f));

            palette = builder.Build();
        }

        private void OnUpdate(float dt)
        {
            worldMan.Update(dt);
        }

        private void OnRenderFrame(Rendering.Abstractions.IRenderView view, Matrix4 transform, float dt)
        {
            if (animationWorld is null)
            {
                return;
            }

            view.SetPalette(palette);
            var renderable = animationWorld.Systems.OfType<IRenderableSystem>().ToArray();
            var renderContext = new WorldRenderContext(view, 0, dt, new Box2(view.Box.Min, view.Box.Max), animationWorld);
            for (int i = 0; i < renderable.Length; i++)
            {
                renderable[i].Render(renderContext);
            }
        }

        private void SetupWorld()
        {
            var gameWorldBuilder = worldMan.Create();
            gameWorldBuilder.SetName("Preview");

            var cameraEntity = entityMan.Create("Camera");
            var cameraComponentBuilder = builderFactory.GetBuilder<CameraComponentBuilder>();
            cameraComponentBuilder.SetSize(100, 100);

            cameraEntity.Add(cameraComponentBuilder.Build());

            //var viewportEntity = entityMan.Create("Viewport");
            //var viewportComponentBuilder = builderFactory.GetBuilder<ViewportComponentBuilder>();
            //viewportComponentBuilder.SetSize(100, 100);
            //viewportEntity.Add(viewportComponentBuilder.Build());

            var spriteComponentBuilder = builderFactory.GetBuilder<SpriteComponentBuilder>();
            var animationComponentBuilder = builderFactory.GetBuilder<AnimationComponentBuilder>();
            var state = animationComponentBuilder.AddState();

            state.SetClipByName(model.Name);
            state.SetSpeed(1.0f);
            state.SetLoop(true);

            var animationSampleEntity = entityMan.Create("AnimationSample");
            animationSampleEntity.Add(PositionComponent.Create(0.0f, 0.0f));
            animationSampleEntity.Add(spriteComponentBuilder.Build());
            animationSampleEntity.Add(animationComponentBuilder.Build());

            //mapEntity.Add(new StampPutterComponent());
            //mapEntity.Add(tileGridComponent);
            //mapEntity.Add(dataGridComponent);

            gameWorldBuilder.SetupGameWorldSystems(isEditor: true);

            animationWorld = gameWorldBuilder.Build();

            animationWorld.AddEntity(animationSampleEntity);
            //animationWorld.AddEntity(viewportEntity);
            animationWorld.AddEntity(cameraEntity);
        }

        private void OnReset(IRenderView view)
        {
            view.SetScale(2.0f);
            view.MoveTo(view.Box.HalfSize);
        }

        private void OnRender(IRenderView view, Matrix4 transform, float dt)
        {
            view.PushMatrix();

            if (pendingReset)
            {
                OnReset(view);
                pendingReset = false;
            }

            view.EnableAlpha();

            RenderAxes(view);

            view.DisableAlpha();

            OnRenderFrame(view, transform, dt);

            view.PopMatrix();
        }

        private void OnCursorDown(ViewCursorDownEvent e)
        {
            if (e.Key == CursorKey.Left)
            {
                //var cursorPos = GetCellIndexCoords(e.View, e.Position) + new Vector4i(model.CenterX, model.CenterY, 0, 1);

                //model.PutTiles(cursorPos, CurrentTileAtlasId, CurrentTileSelection);
            }
            else if (e.Key == CursorKey.Right)
            {
                //var cursorPos = GetCellIndexCoords(e.View, e.Position) + new Vector4i(model.CenterX, model.CenterY, 0, 1);

                //model.EraseTile(cursorPos);
            }
        }

        private void RenderAxes(IRenderView view)
        {
            var worldBox = view.ToWorldBox(view.Box);

            view.Context.Primitives.DrawLine(view, new Vector2(worldBox.Min.X, 0), new Vector2(worldBox.Max.X, 0), Color4.Red);
            view.Context.Primitives.DrawLine(view, new Vector2(0, worldBox.Min.Y), new Vector2(0, worldBox.Max.Y), Color4.Green);
        }

        #endregion Private Methods
    }
}