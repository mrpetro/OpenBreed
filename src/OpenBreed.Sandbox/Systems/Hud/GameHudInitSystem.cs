using OpenBreed.Common.Data;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Rendering.Components;
using OpenBreed.Wecs.Rendering.Systems.Events;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace OpenBreed.Common.Game.Wecs.Systems.Hud
{
    public class GameHudInitSystem :
        IEventSystem<WorldInitializedEventArgs>
    {
        #region Private Fields

        private readonly IGameServices services;
        private readonly IPaletteMan paletteMan;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly IEntityClass actorClass;

        #endregion Private Fields

        #region Public Constructors

        public GameHudInitSystem(IGameServices services,
            IPaletteMan paletteMan,
            PalettesDataProvider palettesDataProvider)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.paletteMan = paletteMan ?? throw new ArgumentNullException(nameof(paletteMan));
            this.palettesDataProvider = palettesDataProvider ?? throw new ArgumentNullException(nameof(palettesDataProvider));
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(WorldInitializedEventArgs e)
        {
            var world = services.Worlds.GetById(e.WorldId);

            if (world.Name != WorldNames.GameHud)
            {
                return;
            }

            var hudCamera = services.Factory.CreateCamera($"Camera.{WorldNames.GameHud}", 0, 0, 320, 240);

            hudCamera.Get<PaletteComponent>().PaletteId = AddWorldPalette(world);

            services.Worlds.RequestAddEntity(hudCamera, world.Id);

            var p1StatusBar = CreateHudElement("StatusBarP1", "Hud/StatusBar/P1", -160, 109);
            services.Worlds.RequestAddEntity(p1StatusBar, world.Id);

            var p1AmmoBar = CreateHudElement("AmmoBar", "Hud/AmmoBar/P1", 40, 115);
            services.Worlds.RequestAddEntity(p1AmmoBar, world.Id);

            var p1HealthBar = CreateHudElement("HealthBar", "Hud/HealthBar/P1", -128, 115);
            services.Worlds.RequestAddEntity(p1HealthBar, world.Id);

            var p1LivesCounter = CreateHudElement("LivesCounter", "Hud/LivesCounter/P1", -24, 112);
            services.Worlds.RequestAddEntity(p1LivesCounter, world.Id);

            var p1AmmoCounter = CreateHudElement("AmmoCounter", "Hud/AmmoCounter/P1", 80, 112);
            services.Worlds.RequestAddEntity(p1AmmoCounter, world.Id);

            var p1KeysCounter = CreateHudElement("KeysCounter", "Hud/KeysCounter/P1", 128, 112);
            services.Worlds.RequestAddEntity(p1KeysCounter, world.Id);

            var p2StatusBar = CreateHudElement("StatusBarP2", "Hud/StatusBar/P2", -160, -120);
            services.Worlds.RequestAddEntity(p2StatusBar, world.Id);

            var p2AmmoBar = CreateHudElement("AmmoBar", "Hud/AmmoBar/P2", 40, -114);
            services.Worlds.RequestAddEntity(p2AmmoBar, world.Id);

            var p2HealthBar = CreateHudElement("HealthBar", "Hud/HealthBar/P2", -128, -114);
            services.Worlds.RequestAddEntity(p2HealthBar, world.Id);

            var p2LivesCounter = CreateHudElement("LivesCounter", "Hud/LivesCounter/P2", -24, -117);
            services.Worlds.RequestAddEntity(p2LivesCounter, world.Id);

            var p2AmmoCounter = CreateHudElement("AmmoCounter", "Hud/AmmoCounter/P2", 80, -117);
            services.Worlds.RequestAddEntity(p2AmmoCounter, world.Id);

            var p2KeysCounter = CreateHudElement("KeysCounter", "Hud/KeysCounter/P2", 128, -117);
            services.Worlds.RequestAddEntity(p2KeysCounter, world.Id);

            var hudViewport = services.Entities.GetByTag(ScreenWorldHelper.GAME_HUD_VIEWPORT).First();
            hudViewport.SetViewportCamera(hudCamera.Id);
        }

        #endregion Public Methods

        #region Private Methods

        private int AddWorldPalette(IWorld world)
        {
            var commonPaletteModel = palettesDataProvider.GetPalette("Palettes.COMMON");

            var paletteEntity = services.Entities.Create(tag: $"Palettes/{WorldNames.GameHud}");
            var paletteComponent = new PaletteComponent();
            paletteEntity.Add(paletteComponent);

            var builder = paletteMan.CreatePalette()
                .SetName(paletteEntity.Tag)
                .SetLength(256)
                .SetColors(commonPaletteModel.Data.Select(color => PaletteHelper.ToColor4(color)).ToArray());

            var cb = commonPaletteModel[0];
            builder.SetColor(0, new Color4(cb.R / 255.0f, cb.G / 255.0f, cb.B / 255.0f, 0.0f));

            var palette = builder.Build();

            paletteComponent.PaletteId = palette.Id;

            services.Worlds.RequestAddEntity(paletteEntity, world.Id);

            return palette.Id;
        }

        private IEntity CreateHudElement(
            string elementName,
            string entityTag,
            int x,
            int y)
        {
            return services.Factory.Create(@$"ABTA\Templates\Common\Hud\{elementName}")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetTag(entityTag)
                .Build();
        }

        #endregion Private Methods
    }
}