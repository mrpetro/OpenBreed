using OpenBreed.Wecs.Components.Animation.Xml;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Animation.Extensions
{
    public static class XmlAnimationComponents
    {
        public static void Setup()
        {
            XmlComponentsList.RegisterComponentType<XmlAnimationComponent>();

        }
    }
}
