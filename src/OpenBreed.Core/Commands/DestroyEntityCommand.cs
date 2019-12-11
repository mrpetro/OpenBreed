using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Commands
{
    public class DestroyEntityCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "ENTITY_DESTROY";

        #endregion Public Fields

        #region Public Constructors

        public DestroyEntityCommand(int entityId)
        {
            EntityId = entityId;
        }

        #endregion Public Constructors

        #region Public Properties
        public int EntityId { get; }
        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}
