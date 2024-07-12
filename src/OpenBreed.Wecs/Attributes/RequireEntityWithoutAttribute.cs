using System;

namespace OpenBreed.Wecs.Attributes
{   
    /// <summary>
    /// Entity System Attribute that can set requirement for entities without specific components 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RequireEntityWithoutAttribute : Attribute
    {
        #region Public Constructors

        public RequireEntityWithoutAttribute(params Type[] componentTypes)
        {
            ComponentTypes = componentTypes;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Component types which are not accepted by the system  
        /// </summary>
        public Type[] ComponentTypes { get; }

        #endregion Public Properties
    }
}