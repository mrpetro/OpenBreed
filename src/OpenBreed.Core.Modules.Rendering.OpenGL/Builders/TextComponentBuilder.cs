using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Rendering.Builders
{
    public class TextComponentBuilder : BaseComponentBuilder
    {
        #region Private Fields

        /// <summary>
        /// Id of text font
        /// </summary>
        public int FontId { get; set; }

        /// <summary>
        /// Offset position of this text part
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Color of this text part
        /// </summary>
        public Color4 Color { get; set; }

        /// <summary>
        /// Actual text of this part
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Order of drawing, higher value object is rendered on top of lower value objects
        /// </summary>
        public float Order { get; set; }

        #endregion Private Fields

        #region Private Constructors

        private TextComponentBuilder(ICore core) : base(core)
        {
        }

        #endregion Private Constructors

        #region Public Methods

        public static IComponentBuilder New(ICore core)
        {
            return new TextComponentBuilder(core);
        }

        public override IEntityComponent Build()
        {
            return TextComponent.Create(FontId, Offset, Color, Text, Order);
        }

        public override void SetProperty(object key, object value)
        {
            var propertyName = Convert.ToString(key);
            switch (propertyName)
            {
                case nameof(FontId):
                    FontId = ToFontId(value);
                    break;

                case nameof(Text):
                    Text = Convert.ToString(value);
                    break;

                case nameof(Order):
                    Order = Convert.ToSingle(value);
                    break;

                case nameof(Offset):
                    Offset = ToVector2(value);
                    break;

                case nameof(Color):
                    Color = ToColor4(value);
                    break;

                default:
                    break;
            }
        }

        #endregion Public Methods
    }
}
