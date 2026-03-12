using System;

namespace OpenBreed.Wecs.Abstractions.Attributes
{   
    /// <summary>
    /// Entity System Attribute that can set requirement for specific worlds
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RequireWorldAttribute : Attribute
    {
        #region Public Constructors

        public RequireWorldAttribute(params string[] worldTypes)
        {
            WorldTypes = worldTypes;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// World types which are not accepted by the system  
        /// </summary>
        public string[] WorldTypes { get; }

        #endregion Public Properties
    }
}