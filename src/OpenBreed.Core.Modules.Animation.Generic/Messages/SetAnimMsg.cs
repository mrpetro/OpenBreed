using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Animation.Messages
{
    public struct SetAnimMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "SET_ANIM";

        #endregion Public Fields

        #region Public Constructors

        public SetAnimMsg(IEntity entity, string name)
        {
            Entity = entity;
            Name = name;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public string Type { get { return TYPE; } }
        public string Name { get; }

        #endregion Public Properties
    }
}