using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface
{
    /// <summary>
    /// Interface for getting Type attributes
    /// </summary>
    public  interface ITypeAttributesProvider
    {
        /// <summary>
        /// Get attributes from given type
        /// </summary>
        /// <param name="type">Type to get attributes from</param>
        /// <returns>Attribute objects</returns>
        object[] GetAttributes(Type type);
    }
}
