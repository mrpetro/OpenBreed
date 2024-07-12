using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.DataSources
{
    public interface IDbFileDataSource : IDbDataSource
    {
        #region Public Properties

        string FilePath { get; set; }

        #endregion Public Properties
    }
}
