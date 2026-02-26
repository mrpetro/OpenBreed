namespace OpenBreed.Wecs.Abstractions.Attributes
{
    /// <summary>
    /// Entity System Attribute that can set requirement for entities with specific tag.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RequireEntityWithTagAttribute : Attribute
    {
        #region Public Constructors

        public RequireEntityWithTagAttribute(string tag)
        {
            Tag = tag;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Required entity tag.
        /// </summary>
        public string Tag { get; }

        #endregion Public Properties
    }
}