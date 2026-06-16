namespace OpenBreed.Pathfinding.Abstractions.Services
{
    public interface ITopology
    {
        #region Public Methods

        bool TryGetNeighborNodeId( int id, int exitId, out int neighbourId, out float distance, out float weight);

        #endregion Public Methods
    }
}