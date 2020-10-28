namespace OpenBreed.Common
{
    public interface IApplication
    {
        #region Public Methods

        T GetInterface<T>() where T : IApplicationInterface;

        #endregion Public Methods
    }
}