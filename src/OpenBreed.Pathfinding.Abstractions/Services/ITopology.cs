namespace OpenBreed.Pathfinding.Abstractions.Services
{
    public interface ITopology
    {
        #region Public Methods

        int GetWeight(int id);

        bool TryGetNeighborNodeId( int id, int exitId, out int neighbourId);

        #endregion Public Methods
    }
}