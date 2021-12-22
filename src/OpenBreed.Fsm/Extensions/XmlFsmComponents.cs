using OpenBreed.Fsm.Xml;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Rendering.Extensions
{
    public static class XmlFsmComponents
    {
        public static void Setup()
        {
            XmlComponentsList.RegisterComponentType<XmlFsmComponent>();
        }
    }
}
