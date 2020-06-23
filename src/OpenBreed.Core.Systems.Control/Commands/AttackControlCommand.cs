using OpenBreed.Core.Commands;

using OpenBreed.Core.Entities;
using OpenTK;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Commands
{
    public struct AttackControlCommand : ICommand
    {
        #region Public Fields

        public const string TYPE = "ATTACK_CONTROL";

        #endregion Public Fields

        #region Public Constructors

        public AttackControlCommand(int entityId, Entity entity, bool primary, bool secondary)
        {
            EntityId = entityId;
            Primary = primary;
            Secondary = secondary;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Name { get { return TYPE; } }

        public bool Primary { get; }
        public bool Secondary { get; }
        
        #endregion Public Properties
    }
}