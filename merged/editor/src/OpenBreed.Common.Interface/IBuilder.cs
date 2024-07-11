namespace OpenBreed.Common.Interface
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