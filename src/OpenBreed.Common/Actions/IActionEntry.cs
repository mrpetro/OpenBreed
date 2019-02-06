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

        string Description { get; set; }
        int Id { get; set; }
        string Name { get; set; }

        IActionPresentation Presentation { get; }
        IActionTriggers Triggers { get; }

        #endregion Public Properties
    }
}
