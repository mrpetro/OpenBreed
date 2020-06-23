using OpenBreed.Core.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Commands
{
    public class DestroyEntityCommand : ICommand
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
        public string Name { get { return TYPE; } }

        #endregion Public Properties
    }
}
