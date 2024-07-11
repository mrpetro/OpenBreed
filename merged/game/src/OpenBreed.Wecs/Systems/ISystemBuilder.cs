namespace OpenBreed.Wecs.Systems
{
    public interface ISystemBuilder<T> where T : IMatchingSystem
    {
        #region Public Methods

        T Build();

        #endregion Public Methods
    }
}