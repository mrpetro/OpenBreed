namespace OpenBreed.Wecs.Systems
{
    public interface ISystemBuilder<T> where T : ISystem
    {
        #region Public Methods

        T Build();

        #endregion Public Methods
    }
}