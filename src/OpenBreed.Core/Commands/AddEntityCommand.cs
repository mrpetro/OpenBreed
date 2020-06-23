namespace OpenBreed.Core.Commands
{
    public class AddEntityCommand : ICommand
    {
        #region Public Fields

        public const string TYPE = "ADD_ENTITY";

        #endregion Public Fields

        #region Public Constructors

        public AddEntityCommand(int worldId, int entityId)
        {
            WorldId = worldId;
            EntityId = entityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int WorldId { get; }
        public int EntityId { get; }

        public string Name { get { return TYPE; } }

        #endregion Public Properties
    }
}