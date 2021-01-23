using OpenBreed.Ecsw.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Components.Common.Xml
{
    [XmlRoot("Timer")]
    public class XmlTimerComponent : XmlComponentTemplate, ITimerComponentTemplate
    {
    }
}
