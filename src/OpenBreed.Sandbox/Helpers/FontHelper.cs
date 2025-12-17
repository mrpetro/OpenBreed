using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Database.Interface;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Wecs.Abstractions.Services;

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

            var spriteAtlas = loader.Load("Vanilla/Common/Computer/Font");

            //Create FontAtlas
            var fontAtlasBuilder = fontMan.Create()
                                     .SetName("ComputerFont")
                                     .SetAlias("Gfx/ComputerFont")
                                     .SetSpriteAtlas("Vanilla/Common/Computer/Font");

            for (int i = 0; i < 59; i++)
            {
                var ch = 32 + (char)i;
                fontAtlasBuilder.MapCharacterToSpriteId(ch, i, 8);
            }

            fontAtlasBuilder.MapCharacterToSpriteId('\0', 0, 8);
            fontAtlasBuilder.MapCharacterToSpriteId('\r', 0, 8);
            fontAtlasBuilder.MapCharacterToSpriteId('\n', 0, 8);
            fontAtlasBuilder.SetHeight(12);

            var fontAtlas = fontAtlasBuilder.Build();
        }

        #endregion Public Methods
    }
}