using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Core.Components.Xml
{
    [XmlRoot("Followed")]
    public class XmlFollowedComponent : XmlComponentTemplate, IFollowedComponentTemplate
    {
        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return new FollowedComponent();
        }
    }
}
