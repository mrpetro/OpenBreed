using OpenBreed.Pathfinding.Abstractions;

namespace OpenBreed.Pathfinding
{
    internal class PathfindFront : IPathfindFront
    {
        #region Public Constructors

        public PathfindFront(int id, float stepLeft)
        {
            Id = id;
            StepLeft = stepLeft;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id { get; }

        public float StepLeft { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public bool Step(float stepValue)
        {
            StepLeft -= stepValue;
            return StepLeft <= 0.0f;
        }

        #endregion Public Methods
    }
}