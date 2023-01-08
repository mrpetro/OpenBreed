using OpenBreed.Wecs.Components.Gui.Xml;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Gui.Extensions
{
    public static class XmlGuiComponents
    {
        public static void Setup()
        {
            XmlComponentsList.RegisterComponentType<XmlCursorInputComponent>();
        }
    }
}
