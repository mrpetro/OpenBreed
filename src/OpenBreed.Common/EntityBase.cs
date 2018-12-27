using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common
{
    public abstract class EntityBase
    {
       public long Id { get; }

       public virtual string Name { get; protected set; }
    }
}
