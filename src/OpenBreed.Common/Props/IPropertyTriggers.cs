using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Props
{
    public interface IPropertyTriggers
    {
        string OnLoad { get; set; }
        string OnDestroy { get; set; }
        string OnCollisionEnter { get; set; }
        string OnCollisionLeave { get; set; }
    }
}
