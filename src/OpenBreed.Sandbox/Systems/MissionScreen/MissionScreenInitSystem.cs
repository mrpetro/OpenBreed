using OpenBreed.Common.Data;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Rendering.Components;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Systems.MissionScreen
{
    public class MissionScreenInitSystem :
        IEventSystem<WorldInitializedEventArgs>
    {
        #region Private Fields

        private readonly IGameServices services;
        private readonly PalettesDataProvider palettesDataProvider;

        #endregion Private Fields

        #region Public Constructors

        public MissionScreenInitSystem(IGameServices services,
            PalettesDataProvider palettesDataProvider)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.palettesDataProvider = palettesDataProvider ?? throw new ArgumentNullException(nameof(palettesDataProvider));
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(WorldInitializedEventArgs e)
        {
            var world = services.Worlds.GetById(e.WorldId);

            if (world.Name != WorldNames.MissionScreen)
            {
                return;
            }

            var missionScreenCamera = services.Factory.CreateCamera("Camera.MissionScreen", 0, 0, 320, 240);

            missionScreenCamera.Get<PaletteComponent>().PaletteId = AddWorldPalette(world);

            services.Worlds.RequestAddEntity(missionScreenCamera, world.Id);

            AddBackground(world, -320, -272);
            AddText(world, -320 / 2 + 48, 240 / 2 - 35);
        }

        #endregion Public Methods

        #region Private Methods

        private void AddBackground(IWorld world, int x, int y)
        {
            var timer = services.Factory.Create(@"ABTA\Templates\Common\MissionScreen\Background")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetTag("MissionScreen/Background")
                .Build();

            services.Worlds.RequestAddEntity(timer, world.Id);
        }

        private void AddText(IWorld world, int x, int y, string text = "")
        {
            var textEntity = services.Factory.Create(@"ABTA\Templates\Common\MissionScreen\Text")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetTag("MissionScreen/Text")
                .Build();
            textEntity.SetText(0, text);
            services.Worlds.RequestAddEntity(textEntity, world.Id);
        }

        private int AddWorldPalette(IWorld world)
        {
            var commonPaletteModel = palettesDataProvider.GetPalette("Vanilla/Common/MissionScreen/Palette");

            var paletteEntity = services.Entities.Create(tag: $"Palettes/{WorldNames.MissionScreen}");
            var paletteComponent = new PaletteComponent();
            paletteEntity.Add(paletteComponent);

            var builder = services.Palettes.CreatePalette()
                .SetName(paletteEntity.Tag)
                .SetLength(256)
                .SetColors(commonPaletteModel.Data.Select(color => color.ToColor4()).ToArray())
                .SetColors(Enumerable.Range(0, 64).Select(idx => Color4.White).ToArray(), 32);

            var palette = builder.Build();

            paletteComponent.PaletteId = palette.Id;

            services.Worlds.RequestAddEntity(paletteEntity, world.Id);

            return palette.Id;
        }

        #endregion Private Methods
    }
}