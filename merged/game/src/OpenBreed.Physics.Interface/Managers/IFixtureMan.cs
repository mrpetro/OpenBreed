namespace OpenBreed.Physics.Interface.Managers
{
    public interface IFixtureMan
    {
        #region Public Methods

        void Add(IFixture fixture);

        IFixture GetById(int fixtureId);

        int NewId();

        void Remove(int fixtureId);

        #endregion Public Methods
    }
}