namespace OpenBreed.Core.Common.Systems.Components
{
    public class GroupPart : IEntityComponent
    {
        #region Public Constructors

        public GroupPart()
        {
        }

        #endregion Public Constructors

        #region Private Constructors

        private GroupPart(int entityId)
        {
            EntityId = entityId;
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// Grouping Entity ID
        /// </summary>
        public int EntityId { get; }

        #endregion Public Properties

        #region Public Methods

        public static GroupPart Create(int entityId)
        {
            return new GroupPart(entityId);
        }

        #endregion Public Methods
    }
}