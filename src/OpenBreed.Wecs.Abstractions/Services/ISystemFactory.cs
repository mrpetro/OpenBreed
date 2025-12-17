namespace OpenBreed.Wecs.Abstractions.Services
{
    /// <summary>
    /// Interface of system factory
    /// </summary>
    public interface ISystemFactory
    {
        #region Public Methods

        ISystem CreateSystem<TSystem>() where TSystem : ISystem;

        #endregion Public Methods
    }
}