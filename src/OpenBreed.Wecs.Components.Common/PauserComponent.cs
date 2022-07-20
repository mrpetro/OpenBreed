namespace OpenBreed.Wecs.Components.Common
{
    public class PauserComponent : IEntityComponent
    {
        #region Public Constructors

        public PauserComponent(bool pause)
        {
            Pause = pause;
        }

        #endregion Public Constructors

        #region Public Properties        
        
        public bool Pause { get; }

        #endregion Public Properties
    }
}