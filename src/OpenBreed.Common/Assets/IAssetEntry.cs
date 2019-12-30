using OpenBreed.Common.Formats;

namespace OpenBreed.Common.Assets
{
    public interface IAssetEntry : IEntry
    {
        #region Public Properties

        IFormatEntry Format { get; }

        string DataSourceRef { get; }

        #endregion Public Properties
    }
}