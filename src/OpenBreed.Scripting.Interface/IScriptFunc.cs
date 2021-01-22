using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Scripting.Interface
{
    /// <summary>
    /// Script function interface
    /// </summary>
    public interface IScriptFunc
    {
        /// <summary>
        /// Function invoke method
        /// </summary>
        /// <param name="args">collection of arguments to be passed when invoking</param>
        /// <returns>Return object if any</returns>
        object Invoke(params object[] args);
    }
}
