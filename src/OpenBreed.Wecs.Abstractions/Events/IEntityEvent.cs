using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Abstractions.Events
{
    public interface IEntityEvent
    {
        #region Public Properties

        int EntityId { get; }

        #endregion Public Properties
    }
}