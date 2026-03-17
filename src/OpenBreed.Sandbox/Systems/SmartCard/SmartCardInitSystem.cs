using OpenBreed.Common.Data;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Rendering.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenBreed.Sandbox.Extensions;

namespace OpenBreed.Sandbox.Systems.SmartCard
{
    public class SmartCardInitSystem :
        IEventSystem<WorldInitializedEventArgs>
    {
        #region Private Fields

        private readonly IGameServices services;
        private readonly PalettesDataProvider palettesDataProvider;

        #endregion Private Fields

        #region Public Constructors

        public SmartCardInitSystem(IGameServices services,
            PalettesDataProvider palettesDataProvider)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.palettesDataProvider = palettesDataProvider ?? throw new ArgumentNullException(nameof(palettesDataProvider));
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(WorldInitializedEventArgs e)
        {
            var world = services.Worlds.GetById(e.WorldId);

            if (world.Name != WorldNames.SmartCardReader)
            {
                return;
            }

            var smartCardCamera = services.Factory.CreateCamera($"Camera.{WorldNames.SmartCardReader}", 0, 0, 320, 240);

            smartCardCamera.Get<PaletteComponent>().PaletteId = AddWorldPalette(world);

            services.Worlds.RequestAddEntity(smartCardCamera, world.Id);

            AddBackground(world, -320, -272);
            AddText(world, -320 / 2 + 20, 240 / 2 - 49);
        }

        #endregion Public Methods

        #region Private Methods

        private void AddBackground(IWorld world, int x, int y)
        {
            var timer = services.Factory.Create(@"ABTA\Templates\Common\SmartCardScreen\Background")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetTag("SmartCardScreen/Background")
                .Build();

            services.Worlds.RequestAddEntity(timer, world.Id);
        }

        private void AddText(IWorld world, int x, int y)
        {
            var textEntity = services.Factory.Create(@"ABTA\Templates\Common\SmartCardScreen\Text")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetTag("SmartCardScreen/Text")
                .Build();
            textEntity.SetText(0, string.Empty);
            services.Worlds.RequestAddEntity(textEntity, world.Id);
        }

        private int AddWorldPalette(IWorld world)
        {
            var commonPaletteModel = palettesDataProvider.GetPalette("Vanilla/Common/SmartCardScreen/Palette");

            var paletteEntity = services.Entities.Create(tag: $"Palettes/{WorldNames.SmartCardReader}");
            var paletteComponent = new PaletteComponent();
            paletteEntity.Add(paletteComponent);

            var builder = services.Palettes.CreatePalette()
                .SetName(paletteEntity.Tag)
                .SetLength(256)
                .SetColors(commonPaletteModel.Data.Select(color => PaletteHelper.ToColor4(color)).ToArray())
                .SetColors(Enumerable.Range(0, 64).Select(idx => PaletteHelper.ToColor4(MyColor.FromArgb(255, 0, 168, 168))).ToArray(), 32);

            var palette = builder.Build();

            paletteComponent.PaletteId = palette.Id;

            services.Worlds.RequestAddEntity(paletteEntity, world.Id);

            return palette.Id;
        }

        #endregion Private Methods
    }
}