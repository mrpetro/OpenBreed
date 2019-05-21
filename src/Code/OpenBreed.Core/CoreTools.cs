using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core
{
    /// <summary>
    /// Various methods used in Core and related assemblies
    /// </summary>
    public static class CoreTools
    {
        /// <summary>
        /// Throw invalid operation exception with component type name message.
        /// </summary>
        /// <typeparam name="T">Type of component</typeparam>
        public static void ThrowComponentRequiredException<T>()
        {
            throw new InvalidOperationException($"Component {typeof(T)} required");
        }
    }
}
