namespace OpenBreed.Wecs.Components.Common
{
	/// <summary>
	/// Component used to store information about an source entity that emitted this component entity
	/// </summary>
	public sealed class SourceEntityComponent : IEntityComponent
    {
        #region Private Constructors

        private SourceEntityComponent(int entityId)
        {
            EntityId = entityId;
        }

		#endregion Private Constructors

		#region Public Properties

		/// <summary>
		/// ID of entity that emitted this entity
		/// </summary>
		public int EntityId { get; }

        #endregion Public Properties

        #region Public Methods

        public static SourceEntityComponent Create(int entityId)
        {
            return new SourceEntityComponent(entityId);
        }

        #endregion Public Methods
    }
}