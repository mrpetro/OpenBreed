namespace OpenBreed.Core.Modules
{
    /// <summary>
    /// Engine core module interface
    /// </summary>
    public interface ICoreModule
    {
        #region Public Properties

        /// <summary>
        /// Reference to core
        /// </summary>
        ICore Core { get; }

        #endregion Public Properties
    }
}