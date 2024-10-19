using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.Game.Wecs.Components.Xml
{
    [XmlRoot("Damager")]
    public class XmlDamagerComponent : XmlComponentTemplate, IDamagerComponentTemplate
    {
    }
}
