namespace OpenBreed.Physics.Interface.Managers
{
    public interface IFixtureMan
    {
        #region Public Methods

        IFixture Create(string alias, IShape shape);

        IFixture GetById(int id);

        IFixture GetByAlias(string alias);

        void UnloadAll();

        #endregion Public Methods
    }
}