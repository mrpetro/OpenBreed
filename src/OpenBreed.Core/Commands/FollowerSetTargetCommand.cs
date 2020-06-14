using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Commands
{
    public class FollowerSetTargetCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "FOLLOWER_SET_TARGET";

        #endregion Public Fields

        #region Public Constructors

        public FollowerSetTargetCommand(int followerEntityId, int targetEntityId)
        {
            EntityId = followerEntityId;
            TargetEntityId = targetEntityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int TargetEntityId { get; }

        public int EntityId { get; }

        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}
