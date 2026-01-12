using System;
using System.ComponentModel;

namespace OpenBreed.Wecs.Abstractions.Attributes
{
    public abstract class SystemCategory
    {
    }

    /// <summary>
    /// System attribute that defines it's category
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SystemCategoryAttribute : Attribute
    {
        #region Public Constructors

        public SystemCategoryAttribute(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("Expected non empty category name");
            }

            Category = category;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// System category
        /// </summary>
        public string Category { get; }

        #endregion Public Properties
    }
}