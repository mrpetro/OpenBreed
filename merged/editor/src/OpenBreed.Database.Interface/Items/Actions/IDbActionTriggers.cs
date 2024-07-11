using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Actions
{
    public interface IDbActionTriggers
    {
        #region Public Properties

        string OnCollisionEnter { get; set; }

        string OnCollisionLeave { get; set; }

        string OnDestroy { get; set; }

        string OnLoad { get; set; }

        #endregion Public Properties

        #region Public Methods

        IDbActionTriggers Copy();

        #endregion Public Methods
    }
}