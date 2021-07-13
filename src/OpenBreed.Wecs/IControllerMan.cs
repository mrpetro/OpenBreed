using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs
{
    public interface IControllerMan
    {
        IComponentController GetController(string component, string property);
    }
}
