using System;

namespace OpenBreed.Wecs.Attributes
{
    /// <summary>
    /// Entity System Attribute that can set requirement for entities with specific components 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RequireEntityWithAttribute : Attribute
    {
        #region Public Constructors

        public RequireEntityWithAttribute(params Type[] componentTypes)
        {
            ComponentTypes = componentTypes;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Component types which are accepted by the system  
        /// </summary>
        public Type[] ComponentTypes { get; }

        #endregion Public Properties
    }
}