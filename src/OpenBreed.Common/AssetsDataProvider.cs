using EPF;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common
{
    public delegate string ExpandVariablesDelegate(string text);

    public class AssetsDataProvider
    {
        #region Private Fields

        private readonly Dictionary<string, AssetBase> _openedAssets = new Dictionary<string, AssetBase>();
        private Dictionary<string, EPFArchive> _openedArchives = new Dictionary<string, EPFArchive>();

        #endregion Private Fields

        #region Public Constructors

        public AssetsDataProvider(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        #endregion Public Constructors

        #region Public Properties

        public static ExpandVariablesDelegate ExpandVariables { get; set; }
        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public AssetBase GetAsset(string name)
        {
            AssetBase asset = null;
            if (_openedAssets.TryGetValue(name, out asset))
                return asset;

            var entry = UnitOfWork.GetRepository<IAssetEntry>().GetById(name);
            if (entry == null)
                throw new Exception($"Asset error: {name}" );

            asset = CreateAsset(entry);

            return asset;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void CloseAll()
        {
            foreach (var openedArchive in _openedArchives)
                openedArchive.Value.Dispose();

            _openedArchives.Clear();
        }

        internal EPFArchive GetArchive(string archivePath)
        {
            string normalizedPath = IOHelper.GetNormalizedPath(archivePath);

            EPFArchive archive = null;
            if (!_openedArchives.TryGetValue(normalizedPath, out archive))
            {
                File.Copy(normalizedPath, normalizedPath + ".bkp", true);
                archive = EPFArchive.ToExtract(File.Open(normalizedPath, FileMode.Open), true);
                _openedArchives.Add(normalizedPath, archive);
            }

            return archive;
        }

        internal void LockSource(AssetBase source)
        {
            _openedAssets.Add(source.Name, source);
        }

        internal void ReleaseSource(AssetBase source)
        {
            _openedAssets.Remove(source.Name);
        }

        #endregion Internal Methods

        #region Private Methods

        private AssetBase CreateFileAsset(IFileAssetEntry asset)
        {
            return new FileAsset(this, asset.Id, asset.FilePath);
        }

        private AssetBase CreateEPFArchiveAsset(IEPFArchiveAssetEntry asset)
        {
            return new EPFArchiveFileAsset(this, asset.Id, asset.ArchivePath, asset.EntryName);
        }

        private AssetBase CreateAsset(IAssetEntry asset)
        {
            if (asset is IFileAssetEntry)
                return CreateFileAsset((IFileAssetEntry)asset);
            else if (asset is IEPFArchiveAssetEntry)
                return CreateEPFArchiveAsset((IEPFArchiveAssetEntry)asset);
            else
                throw new NotImplementedException("Unknown sourceDef");
        }

        #endregion Private Methods
    }
}
