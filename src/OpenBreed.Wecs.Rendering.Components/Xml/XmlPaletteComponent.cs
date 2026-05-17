using OpenBreed.Wecs.Core.Components.Xml;
using OpenTK;
using OpenTK.Mathematics;
using System.Xml.Serialization;
using OpenBreed.Wecs.Components.Xml;
using System;

namespace OpenBreed.Wecs.Rendering.Components.Xml
{
    [XmlRoot("Palette")]
    public class XmlPaletteComponent : XmlComponentTemplate, IPaletteComponentTemplate
    {
        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return new PaletteComponent();
        }

        #endregion Public Methods
    }
}