using System;

namespace OpenBreed.Wecs.Abstractions.Attributes
{

    /// <summary>
    /// Entity System Attribute that can set requirement for entities of specific class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RequireEntityClassAttribute : Attribute
    {
        #region Public Constructors

        public RequireEntityClassAttribute(string entityClass)
        {
            EntityClass = entityClass;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Required entity class name.
        /// </summary>
        public string EntityClass { get; }

        #endregion Public Properties
    }
}