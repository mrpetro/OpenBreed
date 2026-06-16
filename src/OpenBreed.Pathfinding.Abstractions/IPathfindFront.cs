namespace OpenBreed.Pathfinding.Abstractions
{
    public interface IPathfindFront
    {
        #region Public Properties

        int Id { get; }

        #endregion Public Properties

        #region Public Methods

        bool Step(float stepValue);

        #endregion Public Methods
    }
}