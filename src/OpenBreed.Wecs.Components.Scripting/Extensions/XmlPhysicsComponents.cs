using OpenBreed.Wecs.Components.Scripting.Xml;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Scripting.Extensions
{
    public static class XmlScriptingComponents
    {
        public static void Setup()
        {
            XmlComponentsList.RegisterComponentType<XmlScriptComponent>();
        }
    }
}
