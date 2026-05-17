using OpenBreed.Wecs.Components.Xml;
using System;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Audio.Components.Xml
{
    [XmlRoot("SoundPlayer")]
    public class XmlSoundPlayerComponent : XmlComponentTemplate, ISoundPlayerComponentTemplate
    {
        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return new SoundPlayerComponent();
        }

        #endregion Public Methods
    }
}