using OpenBreed.Common.Formats;
using System.Collections.Generic;

namespace OpenBreed.Common.Assets
{
    public interface IAssetEntry : IEntry
    {
        #region Public Properties

        string DataSourceRef { get; }
        string FormatType { get; }
        List<FormatParameter> Parameters { get; }

        #endregion Public Properties
    }
}