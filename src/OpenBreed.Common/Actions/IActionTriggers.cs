using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Actions
{
    public interface IActionTriggers
    {
        #region Public Properties

        string OnCollisionEnter { get; set; }
        string OnCollisionLeave { get; set; }
        string OnDestroy { get; set; }
        string OnLoad { get; set; }

        #endregion Public Properties
    }
}
