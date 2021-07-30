using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Actions
{
    public interface IDbAction
    {
        #region Public Properties

        string Description { get; set; }
        int Id { get; set; }
        string Name { get; set; }

        IDbActionPresentation Presentation { get; }
        IDbActionTriggers Triggers { get; }

        #endregion Public Properties
    }
}
