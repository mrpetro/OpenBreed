namespace OpenBreed.Core.Commands
{
    public class RemoveEntityCommand : ICommand
    {
        #region Public Fields

        public const string TYPE = "REMOVE_ENTITY";

        #endregion Public Fields

        #region Public Constructors

        public RemoveEntityCommand(int worldId, int entityId)
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