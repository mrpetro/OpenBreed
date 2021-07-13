namespace OpenBreed.Wecs.Components.Control
{
    public class AttackInputComponent : IEntityComponent
    {
        #region Public Constructors

        public AttackInputComponent(int playerId, int type)
        {
            PlayerId = playerId;
            Type = type;
        }

        #endregion Public Constructors

        #region Public Properties

        public int PlayerId { get; }
        public int Type { get; }

        #endregion Public Properties
    }
}