using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Attributes
{
    /// <summary>
    /// Attibute used on entity component class to provide name of the component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentNameAttribute : Attribute
    {
        #region Public Constructors

        public ComponentNameAttribute(string name)
        {
            Name = name;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Name of entity component.
        /// </summary>
        public string Name { get; }

        #endregion Public Properties
    }
}