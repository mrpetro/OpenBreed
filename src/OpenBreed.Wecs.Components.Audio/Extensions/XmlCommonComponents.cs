using OpenBreed.Wecs.Components.Audio.Xml;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Audio.Extensions
{
    public static class XmlAudioComponents
    {
        public static void Setup()
        {
            XmlComponentsList.RegisterComponentType<XmlSoundPlayerComponent>();
        }
    }
}
