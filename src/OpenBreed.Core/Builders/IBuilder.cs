namespace OpenBreed.Core.Builders
{
    /// <summary>
    /// Property setter interface
    /// </summary>
    public interface IPropertySetter
    {
        #region Public Methods

        /// <summary>
        /// Set property based on given key and value
        /// </summary>
        /// <param name="key">Property key</param>
        /// <param name="value">Property value</param>
        void SetProperty(object key, object value);

        #endregion Public Methods
    }
}