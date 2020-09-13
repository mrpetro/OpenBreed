using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common
{
    public class ApplicationBase : IApplication
    {
        public ServiceLocator ServiceLocator { get; }

        protected ApplicationBase()
        {
            ServiceLocator = ServiceLocator.Instance;
        }
    }
}
