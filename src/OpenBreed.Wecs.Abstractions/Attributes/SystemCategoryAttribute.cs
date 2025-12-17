using System;
using System.ComponentModel;

namespace OpenBreed.Wecs.Abstractions.Attributes
{
    public abstract class SystemCategory { }

    /// <summary>
    /// System attribute that defines it's category
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SystemCategoryAttribute<TSystemCategory> : Attribute where TSystemCategory : SystemCategory
    {
        #region Public Constructors

        public SystemCategoryAttribute()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// System category
        /// </summary>
        public Type Category => typeof(TSystemCategory);

        #endregion Public Properties
    }
}