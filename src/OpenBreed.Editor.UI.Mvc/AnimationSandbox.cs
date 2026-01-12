using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Data;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Components.Animation;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Primitives;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;

namespace OpenBreed.Editor.UI.Mvc
{
    public class AnimationSandboxFactory : IAnimationSandboxFactory
    {
        #region Private Fields

        private readonly IServiceProvider serviceProvider;
        private readonly IServiceScopeFactory serviceScopeFactory;

        #endregion Private Fields

        #region Public Constructors

        public AnimationSandboxFactory(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceProvider = serviceProvider;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        #endregion Public Constructors

        #region Public Methods

        public IAnimationSandbox Create()
        {
            var sandboxScope = serviceScopeFactory.CreateScope();
            var sandbox = ActivatorUtilities.CreateInstance<AnimationSandbox>(sandboxScope.ServiceProvider);
            return sandbox;
        }

        #endregion Public Methods
    }

    public class AnimationSandbox : IAnimationSandbox
    {
        #region Private Fields

        private readonly IWorldMan worldMan;

        private readonly IEntityMan entityMan;
        private readonly IClipMan<IEntity> clipMan;
        private readonly IPaletteMan paletteMan;

        //private readonly IBuilderFactory builderFactory;

        private readonly PalettesDataProvider palettesDataProvider;
        private readonly IUpdater updater;
        private IWorld animationWorld;

        private IPalette palette;

        #endregion Private Fields

        #region Public Constructors

        public AnimationSandbox(
            IWorldMan worldMan,
            IEntityMan entityMan,
            IClipMan<IEntity> clipMan,
            IPaletteMan paletteMan,
            IUpdaterFactory updaterFactory,
            PalettesDataProvider palettesDataProvider)
        {
            this.worldMan = worldMan ?? throw new ArgumentNullException(nameof(worldMan));
            this.entityMan = entityMan ?? throw new ArgumentNullException(nameof(entityMan));
            this.clipMan = clipMan ?? throw new ArgumentNullException(nameof(clipMan));
            this.paletteMan = paletteMan ?? throw new ArgumentNullException(nameof(paletteMan));
            this.palettesDataProvider = palettesDataProvider ?? throw new ArgumentNullException(nameof(palettesDataProvider));
            updater = updaterFactory.CreateUpdater(60.0f, Update);
        }

        #endregion Public Constructors

        #region Public Properties

        public float CurrentTime
        {
            get
            {
                var ac = AnimationEntity.Get<AnimationComponent>();
                var animator = ac.States[0];
                return animator.Position;
            }
        }

        #endregion Public Properties

        #region Internal Properties

        internal IEntity AnimationEntity => entityMan.GetByTag("AnimationSample").First();

        #endregion Internal Properties

        #region Public Methods

        public MyExtentF GetGraphicalExtent()
        {
            var animationSampleEntity = entityMan.GetByTag("AnimationSample").FirstOrDefault();

            if (animationSampleEntity is null)
            {
                return MyExtentF.Empty;
            }

            //var sprite = animationSampleEntity.Get<SpriteComponent>();

            //sprite.

            return MyExtentF.Empty;
        }

        public void Load(string name, IRenderContext renderContext)
        {
            SetupPalettes(renderContext);
            SetupWorld(name, renderContext);
        }

        public void Render(IRenderView view, float dt)
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

        public void StopAnimation()
        {
            AnimationEntity.StopAnimation(animatorId: 0);
        }

        public void PlayAnimation()
        {
            AnimationEntity.PlayAnimationEx(animatorId: 0);
        }

        public void PauseAnimation()
        {
            AnimationEntity.PauseAnimation(animatorId: 0);
        }

        public void ToEndAnimation()
        {
            AnimationEntity.AnimationToEnd(animatorId: 0);
        }

        public void ToBeginAnimation()
        {
            AnimationEntity.AnimationToBegin(animatorId: 0);
        }

        public void FastRewindAnimation()
        {
        }

        public void FastForwardAnimation()
        {
        }

        #endregion Public Methods

        #region Private Methods

        private void Update(float dt)
        {
            worldMan.Update(dt);
        }

        private void SetupPalettes(IRenderContext renderContext)
        {
            var commonPaletteModel = palettesDataProvider.GetPalette("Palettes.COMMON");

            if (palette is not null)
            {
                return;
            }

            palette = paletteMan.GetByName("GamePalette");

            if (palette is not null)
            {
                return;
            }

            var builder = paletteMan.CreatePalette()
                .SetLength(256)
                .SetName("GamePalette")
                .SetColors(commonPaletteModel.Data.Select(color => color.ToColor4()).ToArray());

            var cb = commonPaletteModel[0];
            builder.SetColor(0, new Color4(cb.R / 255.0f, cb.G / 255.0f, cb.B / 255.0f, 0.0f));
            palette = builder.Build();
        }

        private void SetupWorld(string clipName, IRenderContext renderContext)
        {
            var builderFactory = renderContext.ServiceProvider.GetRequiredService<IBuilderFactory>();

            animationWorld = worldMan.GetByName("Preview");

            if (animationWorld is null)
            {
                var gameWorldBuilder = worldMan.Create();
                gameWorldBuilder.SetName("Preview");
                gameWorldBuilder.SetupGameWorldSystems(isEditor: true);

                animationWorld = gameWorldBuilder.Build();
            }

            var cameraEntity = entityMan.FindOrCreate("Camera", (e) =>
            {
                var cameraComponentBuilder = builderFactory.GetBuilder<CameraComponentBuilder>();
                cameraComponentBuilder.SetSize(100, 100);

                e.Add(cameraComponentBuilder.Build());
                animationWorld.AddEntity(e);
            });

            var animationSampleEntity = entityMan.FindOrCreate("AnimationSample", (e) =>
            {
                var spriteComponentBuilder = builderFactory.GetBuilder<SpriteComponentBuilder>();
                var animationComponentBuilder = builderFactory.GetBuilder<AnimationComponentBuilder>();
                var state = animationComponentBuilder.AddState();

                state.SetTime(0.0f);
                state.SetSpeed(0.0f);
                state.SetLoop(true);

                e.Add(PositionComponent.Create(0.0f, 0.0f));
                e.Add(spriteComponentBuilder.Build());
                e.Add(animationComponentBuilder.Build());

                animationWorld.AddEntity(e);
            });


            if (clipMan.TryGetId(clipName, out int clipId))
            {
                animationSampleEntity.SetAnimationClipById(animatorId: 0, clipId);
            }
        }

        #endregion Private Methods
    }
}