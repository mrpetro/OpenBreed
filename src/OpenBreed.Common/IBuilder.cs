namespace OpenBreed.Common
{
    public interface IBuilder
    {
    }

    public interface IBuilder<TClass> : IBuilder
    {
        #region Public Methods

        TClass Build();

        #endregion Public Methods
    }
}