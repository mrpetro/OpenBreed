using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Actions
{
    public interface IActionEntry
    {
        #region Public Properties

        string Description { get; }
        int Id { get; }
        string Name { get; }

        IActionPresentation Presentation { get; }
        IActionTriggers Triggers { get; }

        #endregion Public Properties
    }
}
