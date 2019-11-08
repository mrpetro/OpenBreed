using OpenTK;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static string ToConsole(Vector2 point)
        {
            return string.Format(CultureInfo.InvariantCulture ,"{0:0.000}, {1:0.000}", point.X, point.Y);
        }
    }
}
