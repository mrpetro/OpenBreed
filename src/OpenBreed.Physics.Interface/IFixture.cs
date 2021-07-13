
namespace OpenBreed.Physics.Interface
{
    public interface IFixture
    {
        #region Public Properties

        int Id { get; }

        IShape Shape { get; }

        #endregion Public Properties
    }
}