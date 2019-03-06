using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data2
{
    public interface IDataEntry : IEntry
    {
        #region Public Properties

        string AssetRef { get; }
        IFormatEntry Format { get; }

        #endregion Public Properties
    }
}
