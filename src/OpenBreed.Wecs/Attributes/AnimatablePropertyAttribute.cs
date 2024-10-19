using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Attributes
{
    /// <summary>
    /// Attibute used on entity component properties to indicate if property can be animated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AnimatablePropertyAttribute : Attribute
    {
    }
}
