using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Modules.Rendering.Systems;

namespace OpenBreed.Core.Modules.Rendering
{
    public class VideoSystemsFactory
    {
        #region Private Fields

        private readonly ICore core;
        private readonly TextSystemBuilder textSystemBuilder;
        private readonly SpriteSystemBuilder spriteSystemBuilder;
        private readonly TileSystemBuilder tileSystemBuilder;

        #endregion Private Fields

        #region Public Constructors

        public VideoSystemsFactory(ICore core)
        {
            this.core = core;
            textSystemBuilder = new TextSystemBuilder(core);
            spriteSystemBuilder = new SpriteSystemBuilder(core);
            tileSystemBuilder = new TileSystemBuilder(core);
        }

        #endregion Public Constructors

        #region Public Methods

        public TextSystem CreateTextSystem()
        {
            return textSystemBuilder.Build();
        }

        public SpriteSystem CreateSpriteSystem()
        {
            return spriteSystemBuilder.Build();
        }

        public TileSystem CreateTileSystem()
        {
            return tileSystemBuilder.Build();
        }

        public ViewportSystem CreateViewportSystem()
        {
            return new ViewportSystem(core);
        }

        #endregion Public Methods
    }
}