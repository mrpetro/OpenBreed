namespace OpenBreed.Core.Systems.Physics.Components
{
    /// <summary>
    /// Physical body component interface which is used in entites that will remain world initial position all the time
    /// </summary>
    public interface IStaticBody : IPhysicsComponent
    {
        #region Public Methods

        void GetGridIndices(out int x, out int y);

        #endregion Public Methods
    }
}