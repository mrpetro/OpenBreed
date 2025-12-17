using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Sprites;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Hud;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Scripting;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenTK;
using OpenTK.Mathematics;
using System.Linq;
using OpenBreed.Common.Game.Managers;
using OpenBreed.Common.Game;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Extensions;

namespace OpenBreed.Sandbox.Worlds
{
    public class GameHudWorldHelper
    {
        #region Private Fields

        private readonly ISystemFactory systemFactory;
        private readonly IWorldMan worldMan;
        private readonly IFontMan fontMan;
        private readonly IWindow viewClient;
        private readonly IEntityMan entityMan;
        private readonly ITriggerMan triggerMan;
        private readonly IEntityFactory entityFactory;
        private readonly VanillaStatusBarHelper hudHelper;
        private readonly CameraHelper cameraHelper;
        private readonly IRepositoryProvider repositoryProvider;
        private readonly IPaletteMan paletteMan;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly SpriteAtlasDataProvider spriteAtlasDataProvider;
        private readonly ITextureMan textureMan;
        private readonly ISpriteMan spriteMan;
        private readonly ItemsMan itemsMan;

        #endregion Private Fields

        #region Public Constructors

        public GameHudWorldHelper(
            ISystemFactory systemFactory, 
            IWorldMan worldMan, 
            IFontMan fontMan, 
            IWindow viewClient, 
            IEntityMan entityMan,
            ITriggerMan triggerMan,
            IEntityFactory entityFactory,
            VanillaStatusBarHelper hudHelper, 
            CameraHelper cameraHelper, 
            IRepositoryProvider repositoryProvider, 
            IPaletteMan paletteMan,
            PalettesDataProvider palettesDataProvider,
            IDataLoaderFactory dataLoaderFactory,
            SpriteAtlasDataProvider spriteAtlasDataProvider,
            ITextureMan textureMan,
            ISpriteMan spriteMan,
            ItemsMan itemsMan)
        {
            this.systemFactory = systemFactory;
            this.worldMan = worldMan;
            this.fontMan = fontMan;
            this.viewClient = viewClient;

            this.entityMan = entityMan;
            this.triggerMan = triggerMan;
            this.entityFactory = entityFactory;
            this.hudHelper = hudHelper;
            this.cameraHelper = cameraHelper;
            this.repositoryProvider = repositoryProvider;
            this.paletteMan = paletteMan;
            this.palettesDataProvider = palettesDataProvider;
            this.dataLoaderFactory = dataLoaderFactory;
            this.spriteAtlasDataProvider = spriteAtlasDataProvider;
            this.textureMan = textureMan;
            this.spriteMan = spriteMan;
            this.itemsMan = itemsMan;
        }

        #endregion Public Constructors

        #region Public Methods


        private int AddWorldPalette(IWorld world)
        {
            var commonPaletteModel = palettesDataProvider.GetPalette("Palettes.COMMON");

            var paletteEntity = entityMan.Create(tag: $"Palettes/{WorldNames.GAME_HUD}");
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

            worldMan.RequestAddEntity(paletteEntity, world.Id);

            return palette.Id;
        }


        public void Create()
        {
            var statusBarTexture = textureMan.Create(
            "StatusBarPixel",
            1,
            1,
            new byte[] { 0x4D } );


            var loader = dataLoaderFactory.GetLoader<ISpriteAtlasDataLoader>();

            //Load common sprites
            var dbStatusBarSpriteAtlas = repositoryProvider.GetRepository<IDbSpriteAtlas>().GetById("Vanilla/Common/Status");

            //var colors = paletteModel.GetColors(0, 64);

            //paletteModel.SetColors(64, colors);

            var spriteAtlas = loader.Load(dbStatusBarSpriteAtlas);

            //Create FontAtlas
            var fontAtlas = fontMan.Create()
                                     .SetName("StatusBar")
                                     .SetAlias("Gfx/StatusBar")
                                     .SetSpriteAtlas("Vanilla/Common/Status")
                                     .MapCharacterToSpriteId('0', 2)
                                     .MapCharacterToSpriteId('1', 3)
                                     .MapCharacterToSpriteId('2', 4)
                                     .MapCharacterToSpriteId('3', 5)
                                     .MapCharacterToSpriteId('4', 6)
                                     .MapCharacterToSpriteId('5', 7)
                                     .MapCharacterToSpriteId('6', 8)
                                     .MapCharacterToSpriteId('7', 9)
                                     .MapCharacterToSpriteId('8', 10)
                                     .MapCharacterToSpriteId('9', 11)
                                     .MapCharacterToSpriteId('+', 12)
                                     .Build();


            var healthBarPixel = spriteMan.CreateAtlas()
                .SetName("StatusBarPixel")
                .SetTexture(statusBarTexture.Id)
                .AppendCoord(0, 0, 1, 1)
                .Build();

            var builder = worldMan.Create().SetName(WorldNames.GAME_HUD);

            AddSystems(builder);

            Setup(builder.Build());
        }

        #endregion Public Methods

        #region Private Methods

        private void AddSystems(IWorldBuilder builder)
        {
            builder.AddSystem<AnimatorSystem>();
            builder.AddSystem<SpriteSystem>();
            builder.AddSystem<TextSystem>();
            builder.AddSystem<ScriptRunningSystem>();
        }

        private void BindHealthBar(IEntity statusBarEntity, IEntity targetActor)
        {
            var spriteComponent = statusBarEntity.Get<SpriteComponent>();
            var healthComponent = targetActor.Get<HealthComponent>();

            var percent = healthComponent.GetPercent();
            spriteComponent.Scale = new Vector2((int)(64 * percent), 1);

            triggerMan.OnDamaged(targetActor, (s, a) =>
            {
                var percent = healthComponent.GetPercent();
                spriteComponent.Scale = new Vector2((int)(64 * percent), 1);
            });
        }

        private void BindAmmoBar(IEntity statusBarEntity, IEntity targetActor)
        {
            var spriteComponent = statusBarEntity.Get<SpriteComponent>();
            var ammoComponent = targetActor.Get<AmmoComponent>();

            var percent = ammoComponent.GetRoundsPercent();
            spriteComponent.Scale = new Vector2((int)(32 * percent), 1);

            triggerMan.OnInventoryChanged(targetActor, (s, a) =>
            {
                var percent = ammoComponent.GetRoundsPercent();
                spriteComponent.Scale = new Vector2((int)(32 * percent), 1);
            });
        }

        private void BindLivesCounter(IEntity statusCounterEntity, IEntity targetActor)
        {
            var livesComponent = targetActor.Get<LivesComponent>();

            var livesCount = livesComponent.Value;
            statusCounterEntity.SetText(0, livesCount.ToString().PadLeft(2, '0'));

            triggerMan.OnEntityLivesChanged(targetActor, (s, a) =>
            {
                var livesCount = livesComponent.Value;
                statusCounterEntity.SetText(0, livesCount.ToString().PadLeft(2, '0'));
            });
        }

        private void BindAmmoCounter(IEntity statusCounterEntity, IEntity targetActor)
        {
            var inventoryComponent = targetActor.Get<InventoryComponent>();

            triggerMan.OnInventoryChanged(targetActor, (s, a) =>
            {
                if (a.ItemId != ItemTypes.Ammo)
                    return;

                var slot = inventoryComponent.GetItemSlot(ItemTypes.Ammo);
                var quantity = slot.GetItemQuantity(ItemTypes.Ammo);

                if(quantity > 9)
                    statusCounterEntity.SetText(0, "+");
                else
                    statusCounterEntity.SetText(0, quantity.ToString());
            });
        }

        private void BindKeysCounter(IEntity statusCounterEntity, IEntity targetActor)
        {
            triggerMan.OnInventoryChanged(targetActor, (s, a) =>
            {
                if (a.ItemId != ItemTypes.KeycardStandard)
                    return;

                var inventoryComponent = targetActor.Get<InventoryComponent>();

                var slot = inventoryComponent.GetItemSlot(ItemTypes.KeycardStandard);
                var quantity = slot.GetItemQuantity(ItemTypes.KeycardStandard);

                statusCounterEntity.SetText(0, quantity.ToString().PadLeft(2, '0'));
            });
        }

        private IEntity GetPlayerControlledEntity(string playerName)
        {
            var player1Entity = entityMan.GetByTag($"Players/{playerName}").FirstOrDefault();

            if (player1Entity is null)
                return null;

            var controlledEntityId = player1Entity.GetControlledEntityId();
            return entityMan.GetById(controlledEntityId);
        }

        private void Setup(IWorld world)
        {
            var hudCamera = cameraHelper.CreateCamera($"Camera.{WorldNames.GAME_HUD}", 0, 0, 320, 240);

            triggerMan.OnWorldInitialized(world, () =>
            {
                hudCamera.Get<PaletteComponent>().PaletteId = AddWorldPalette(world);

                worldMan.RequestAddEntity(hudCamera, world.Id);

                var p1StatusBar = hudHelper.CreateHudElement("StatusBarP1", "Hud/StatusBar/P1", -160, 109);
                worldMan.RequestAddEntity(p1StatusBar, world.Id);

                var p1AmmoBar = hudHelper.CreateHudElement("AmmoBar", "Hud/AmmoBar/P1", 40, 115);
                worldMan.RequestAddEntity(p1AmmoBar, world.Id);

                var p1HealthBar = hudHelper.CreateHudElement("HealthBar", "Hud/HealthBar/P1", -128, 115);
                worldMan.RequestAddEntity(p1HealthBar, world.Id);

                var p1LivesCounter = hudHelper.CreateHudElement("LivesCounter", "Hud/LivesCounter/P1", -24, 120);
                worldMan.RequestAddEntity(p1LivesCounter, world.Id);

                var p1AmmoCounter = hudHelper.CreateHudElement("AmmoCounter", "Hud/AmmoCounter/P1", 80, 120);
                worldMan.RequestAddEntity(p1AmmoCounter, world.Id);

                var p1KeysCounter = hudHelper.CreateHudElement("KeysCounter", "Hud/KeysCounter/P1", 128, 120);
                worldMan.RequestAddEntity(p1KeysCounter, world.Id);

                var p2StatusBar = hudHelper.CreateHudElement("StatusBarP2", "Hud/StatusBar/P2", -160, -120);
                worldMan.RequestAddEntity(p2StatusBar, world.Id);

                var p2AmmoBar = hudHelper.CreateHudElement("AmmoBar", "Hud/AmmoBar/P2", 40, -114);
                worldMan.RequestAddEntity(p2AmmoBar, world.Id);

                var p2HealthBar = hudHelper.CreateHudElement("HealthBar", "Hud/HealthBar/P2", -128, -114);
                worldMan.RequestAddEntity(p2HealthBar, world.Id);

                var p2LivesCounter = hudHelper.CreateHudElement("LivesCounter", "Hud/LivesCounter/P2", -24, -109);
                worldMan.RequestAddEntity(p2LivesCounter, world.Id);

                var p2AmmoCounter = hudHelper.CreateHudElement("AmmoCounter", "Hud/AmmoCounter/P2", 80, -109);
                worldMan.RequestAddEntity(p2AmmoCounter, world.Id);

                var p2KeysCounter = hudHelper.CreateHudElement("KeysCounter", "Hud/KeysCounter/P2", 128, -109);
                worldMan.RequestAddEntity(p2KeysCounter, world.Id);

                var hudViewport = entityMan.GetByTag(ScreenWorldHelper.GAME_HUD_VIEWPORT).First();
                hudViewport.SetViewportCamera(hudCamera.Id);

                var p1Actor = GetPlayerControlledEntity("P1");

                if (p1Actor is not null)
                {
                    BindHealthBar(p1HealthBar, p1Actor);
                    BindAmmoBar(p1AmmoBar, p1Actor);
                    BindLivesCounter(p1LivesCounter, p1Actor);
                    BindAmmoCounter(p1AmmoCounter, p1Actor);
                    BindKeysCounter(p1KeysCounter, p1Actor);
                }

            }, singleTime: true);

        }

        #endregion Private Methods
    }
}