namespace OpenBreed.Wecs.Components.Common
{
    public class PauserComponent : IEntityComponent
    {
        #region Public Constructors

        public PauserComponent(int worldId, bool pause)
        {
            WorldId = worldId;
            Pause = pause;
        }

        #endregion Public Constructors

        #region Public Properties        
        
        public int WorldId { get; }
        public bool Pause { get; }

        #endregion Public Properties
    }
}