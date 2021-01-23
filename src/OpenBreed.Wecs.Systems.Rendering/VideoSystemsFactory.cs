using OpenBreed.Core;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Systems.Rendering.Builders;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class VideoSystemsFactory
    {
        #region Private Fields

        private readonly ICore core;

        #endregion Private Fields

        #region Public Constructors

        public VideoSystemsFactory(ICore core)
        {
            this.core = core;
        }

        #endregion Public Constructors

        #region Public Methods

        public TextSystemBuilder CreateTextSystem()
        {
            return new TextSystemBuilder(core);
        }

        public SpriteSystemBuilder CreateSpriteSystem()
        {
            return new SpriteSystemBuilder(core);
        }

        public TileSystemBuilder CreateTileSystem()
        {
            return new TileSystemBuilder(core);
        }

        public ViewportSystemBuilder CreateViewportSystem()
        {
            return new ViewportSystemBuilder(core);
        }

        #endregion Public Methods
    }
}