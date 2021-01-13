using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Systems.Physics.Commands
{
    public struct BodyOffCommand : ICommand
    {
        #region Public Fields

        public const string TYPE = "BODY_OFF";

        #endregion Public Fields

        #region Public Constructors

        public BodyOffCommand(int entityId)
        {
            EntityId = entityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Name { get { return TYPE; } }

        #endregion Public Properties
    }
}
