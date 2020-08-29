using OpenBreed.Database.Interface.Items;
using System.Collections.Generic;

namespace OpenBreed.Database.Interface.Items.Assets
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