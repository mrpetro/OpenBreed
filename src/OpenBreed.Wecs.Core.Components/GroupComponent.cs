namespace OpenBreed.Wecs.Core.Components
{
    [ComponentName("Group")]
    public class GroupComponent : IEntityComponent
    {
        #region Public Constructors

        public GroupComponent(int id)
        {
            Id = id;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Ids of entities which are members of this group 
        /// </summary>
        public int Id { get; }

        #endregion Public Properties
    }
}