namespace OpenBreed.Game.Physics.Components
{
    public interface IStaticBody : IPhysicsComponent
    {
        #region Public Methods

        void GetGridIndices(out int x, out int y);

        #endregion Public Methods
    }
}