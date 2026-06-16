namespace OpenBreed.Pathfinding.Abstractions.Services
{
    public interface ITopology
    {
        #region Public Methods

        float GetWeight(int id);

        bool TryGetNeighborNodeId( int id, int exitId, out int neighbourId, out float distance);

        #endregion Public Methods
    }
}