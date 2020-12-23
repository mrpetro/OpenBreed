using OpenBreed.Core.Components;
using OpenTK.Graphics;

namespace OpenBreed.Rendering.Components
{
    public class TextPresentationComponent : IEntityComponent
    {
        #region Internal Constructors

        public TextPresentationComponent(int fontId, Color4 color, float order)
        {
            FontId = fontId;
            Color = color;
            Order = order;
        }

        #endregion Internal Constructors

        #region Public Properties

        public int FontId { get; set; }

        public Color4 Color { get; set; }

        public float Order { get; set; }

        #endregion Public Properties
    }
}