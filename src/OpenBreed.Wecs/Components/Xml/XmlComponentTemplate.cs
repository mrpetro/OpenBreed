using OpenBreed.Wecs.Abstractions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Xml
{
    public class XmlComponentTemplate : IComponentTemplate
    {
        public virtual IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return null;
        }
    }
}
