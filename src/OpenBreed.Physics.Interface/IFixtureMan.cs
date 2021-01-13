namespace OpenBreed.Physics.Interface
{
    public interface IFixtureMan
    {
        #region Public Methods

        IFixture Create(string alias, string type, IShape shape);

        IFixture GetById(int id);

        IFixture GetByAlias(string alias);

        void UnloadAll();

        #endregion Public Methods
    }
}