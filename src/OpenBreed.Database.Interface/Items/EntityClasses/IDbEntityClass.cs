using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.EntityClasses
{
    public interface IDbEntityClass : IDbEntry
    {
        #region Public Properties

        string ParentId{ get; set; }

        #endregion Public Properties
    }
}
