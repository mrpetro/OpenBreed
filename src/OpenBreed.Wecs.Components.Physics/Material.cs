using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Physics
{
    /// <summary>
    /// Body material
    /// </summary>
    public class Material
    {
        /// <summary>
        /// Material density in 2D world. Units: Kg/m^2
        /// </summary>
        public float Density { get; }

        /// <summary>
        /// Material ID
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Material name
        /// </summary>
        public string Name { get; }
    }
}
