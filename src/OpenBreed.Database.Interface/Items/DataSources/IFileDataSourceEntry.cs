using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.DataSources
{
    public interface IFileDataSourceEntry : IDataSourceEntry
    {
        #region Public Properties

        string FilePath { get; set; }

        #endregion Public Properties
    }
}
