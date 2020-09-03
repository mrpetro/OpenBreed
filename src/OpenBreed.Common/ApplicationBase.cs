using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common
{
    public class ApplicationBase : IApplication
    {
        public T GetInterface<T>() where T : IApplicationInterface
        {
            throw new NotImplementedException();
        }
    }
}
