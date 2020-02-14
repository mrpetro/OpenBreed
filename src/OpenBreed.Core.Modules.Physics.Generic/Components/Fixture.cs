using OpenBreed.Core.Modules.Physics.Components.Shapes;

namespace OpenBreed.Core.Modules.Physics.Components
{
    public class Fixture
    {
        #region Public Constructors

        public Fixture(int id, IShape shape)
        {
            Id = id;
            Shape = shape;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id { get; }

        public IShape Shape { get; }

        #endregion Public Properties
    }
}