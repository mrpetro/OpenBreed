using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items
{
    public interface IDbPoint2i
    {
        #region Public Properties

        int X { get; set; }
        int Y { get; set; }

        #endregion Public Properties

        #region Public Methods

        IDbPoint2i Copy();

        #endregion Public Methods
    }
}