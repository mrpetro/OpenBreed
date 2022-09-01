using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Model.Palettes;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using System.Linq;

namespace OpenBreed.Sandbox.Helpers
{
    public class FontHelper
    {
        #region Private Fields

        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityMan entityMan;
        private readonly IFontMan fontMan;
        private readonly IRepositoryProvider repositoryProvider;
        private readonly SpriteAtlasDataProvider spriteAtlasDataProvider;

        #endregion Private Fields

        #region Public Constructors

        public FontHelper(
            IFontMan fontMan,
            IEntityMan entityMan,
            IRepositoryProvider repositoryProvider,
            IDataLoaderFactory dataLoaderFactory,
            SpriteAtlasDataProvider spriteAtlasDataProvider)
        {
            this.fontMan = fontMan;
            this.entityMan = entityMan;
            this.repositoryProvider = repositoryProvider;
            this.dataLoaderFactory = dataLoaderFactory;
            this.spriteAtlasDataProvider = spriteAtlasDataProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public void SetupGameFont()
        {
            var loader = dataLoaderFactory.GetLoader<ISpriteAtlasDataLoader>();

            //Load common sprites
            var dbStatusBarSpriteAtlas = repositoryProvider.GetRepository<IDbSpriteAtlas>().GetById("Vanilla/Common/Computer/Font");

            var spriteSet = spriteAtlasDataProvider.GetSpriteSet(dbStatusBarSpriteAtlas.Id);

            var paletteModel = GetPaletteModel("GameWorld/Palette/CMAP");
            var spriteAtlas = loader.Load(dbStatusBarSpriteAtlas.Id, paletteModel);

            //Create FontAtlas
            var fontAtlasBuilder = fontMan.Create()
                                     .SetName("ComputerFont");

            for (int i = 0; i < 59; i++)
            {
                var ch = 32 + (char)i;
                fontAtlasBuilder.AddCharacterFromSprite(ch, $"Vanilla/Common/Computer/Font#{i}", 0);
            }

            var fontAtlas = fontAtlasBuilder.Build();
        }

        #endregion Public Methods

        #region Private Methods

        private PaletteModel GetPaletteModel(string paletteName)
        {
            var paletteEntity = entityMan.GetByTag(paletteName).FirstOrDefault();

            if (paletteEntity is null)
                return PaletteModel.NullPalette;

            var paletteComponent = paletteEntity.TryGet<PaletteComponent>();

            if (paletteComponent is null)
                return PaletteModel.NullPalette;

            var paletteBuilder = PaletteBuilder.NewPaletteModel();
            paletteBuilder.SetName(paletteName);
            for (int i = 0; i < paletteComponent.Colors.Length; i++)
            {
                var c = paletteComponent.Colors[i];

                paletteBuilder.SetColor(i, System.Drawing.Color.FromArgb(255, c.R, c.G, c.B));
            }

            return paletteBuilder.Build();
        }

        #endregion Private Methods
    }
}