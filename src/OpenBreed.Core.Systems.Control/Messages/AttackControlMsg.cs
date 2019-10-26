using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;
using OpenTK;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Messages
{
    public struct AttackControlMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "ATTACK_CONTROL";

        #endregion Public Fields

        #region Public Constructors

        public AttackControlMsg(IEntity entity, bool primary, bool secondary)
        {
            Entity = entity;
            Primary = primary;
            Secondary = secondary;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public string Type { get { return TYPE; } }

        public bool Primary { get; }
        public bool Secondary { get; }
        
        #endregion Public Properties
    }
}