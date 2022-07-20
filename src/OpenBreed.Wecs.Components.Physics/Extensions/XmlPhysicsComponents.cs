using OpenBreed.Wecs.Components.Physics.Xml;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Physics.Extensions
{
    public static class XmlPhysicsComponents
    {
        public static void Setup()
        {
            XmlComponentsList.RegisterComponentType<XmlBodyComponent>();
            XmlComponentsList.RegisterComponentType<XmlMotionComponent>();
        }
    }
}
