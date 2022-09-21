using OpenBreed.Wecs.Components.Rendering.Xml;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Rendering.Extensions
{
    public static class XmlRenderingComponents
    {
        public static void Setup()
        {
            XmlComponentsList.RegisterComponentType<XmlPictureComponent>();
            XmlComponentsList.RegisterComponentType<XmlSpriteComponent>();
            XmlComponentsList.RegisterComponentType<XmlTextComponent>();
            XmlComponentsList.RegisterComponentType<XmlViewportComponent>();
            XmlComponentsList.RegisterComponentType<XmlCameraComponent>();
            XmlComponentsList.RegisterComponentType<XmlTilePutterComponent>();
        }
    }
}
