namespace OpenBreed.Wecs.Abstractions.Services
{
    public interface ISystemBuilder<T> where T : IMatchingSystem
    {
        #region Public Methods

        T Build();

        #endregion Public Methods
    }
}