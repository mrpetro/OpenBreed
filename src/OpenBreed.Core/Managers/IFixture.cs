namespace OpenBreed.Core.Managers
{
    public interface IFixture
    {
        #region Public Properties

        int Id { get; }

        IShape Shape { get; }

        #endregion Public Properties
    }
}