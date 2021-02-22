namespace OpenBreed.Wecs.Systems
{
    public interface ISystemFactory
    {
        #region Public Methods

        TSystem Create<TSystem>() where TSystem : ISystem;

        #endregion Public Methods
    }
}