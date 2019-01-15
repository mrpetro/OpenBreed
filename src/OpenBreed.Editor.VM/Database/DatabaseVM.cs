using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM;
using OpenBreed.Common.Assets;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Levels;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Levels.Tools;
using OpenBreed.Editor.VM.Base;
using System.IO;
using OpenBreed.Common;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Database.Items;
using OpenBreed.Common.XmlDatabase;
using System.ComponentModel;
using OpenBreed.Common.XmlDatabase.Items.Sources;
using OpenBreed.Common.XmlDatabase.Tables.Sources;
using OpenBreed.Common.XmlDatabase.Items.Levels;
using OpenBreed.Common.XmlDatabase.Items.Images;
using OpenBreed.Common.XmlDatabase.Tables;
using OpenBreed.Common.XmlDatabase.Tables.Images;
using OpenBreed.Editor.VM.Database.Tables;
using OpenBreed.Common.XmlDatabase.Tables.Levels;
using OpenBreed.Common.XmlDatabase.Tables.Props;
using OpenBreed.Common.XmlDatabase.Items.Props;
using OpenBreed.Common.XmlDatabase.Items.Tiles;
using OpenBreed.Common.XmlDatabase.Tables.Tiles;
using OpenBreed.Common.XmlDatabase.Items.Sprites;
using OpenBreed.Common.XmlDatabase.Tables.Sprites;
using OpenBreed.Common.XmlDatabase.Items.Palettes;
using OpenBreed.Common.XmlDatabase.Tables.Palettes;
using OpenBreed.Common.Images;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Props;
using OpenBreed.Common.Tiles;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Sounds;

namespace OpenBreed.Editor.VM.Database
{
    public enum ProjectState
    {
        Closed,
        New,
        Opened,
        Closing
    }

    public class DatabaseVM : BaseViewModel, IDisposable
    {

        #region Private Fields

        private string _name;
        private ProjectState _state;

        #endregion Private Fields

        #region Internal Constructors

        internal DatabaseVM(EditorVM root, IDatabase database)
        {
            Root = root;

            UnitOfWork = ServiceLocator.Instance.RegisterService<IUnitOfWork>(database.CreateUnitOfWork());
            DataProvider = ServiceLocator.Instance.RegisterService<DataProvider>(new DataProvider(UnitOfWork));
        }

        #endregion Internal Constructors

        #region Public Properties

        public DataProvider DataProvider { get; }
        public bool IsModified { get; internal set; }
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public EditorVM Root { get; private set; }
        public ProjectState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        public IUnitOfWork UnitOfWork { get; }

        public void Dispose()
        {
            ServiceLocator.Instance.UnregisterService<DataProvider>();
            ServiceLocator.Instance.UnregisterService<IUnitOfWork>();
        }

        #endregion Public Properties

        #region Internal Methods

        internal DbEntryVM CreateItem(IEntry entry)
        {
            if (entry is IImageEntry)
                return new DbImageEntryVM(this);
            else if (entry is ISoundEntry)
                return new DbSoundEntryVM(this);
            else if (entry is ILevelEntry)
                return new DbLevelEntryVM(this);
            else if (entry is IAssetEntry)
                return new DbAssetEntryVM(this);
            else if (entry is IPropSetEntry)
                return new DbPropSetEntryVM(this);
            else if (entry is ITileSetEntry)
                return new DbTileSetEntryVM(this);
            else if (entry is ISpriteSetEntry)
                return new DbSpriteSetEntryVM(this);
            else if (entry is IPaletteEntry)
                return new DbPaletteEntryVM(this);
            else
                throw new NotImplementedException(entry.ToString());
        }

        internal DatabaseTableVM CreateTable(IRepository repository)
        {
            if (repository is IRepository<IImageEntry>)
                return new DatabaseImageTableVM(this);
            if (repository is IRepository<ISoundEntry>)
                return new DatabaseSoundTableVM(this);
            else if (repository is IRepository<ILevelEntry>)
                return new DatabaseLevelTableVM(this);
            else if (repository is IRepository<IPropSetEntry>)
                return new DatabasePropertySetTableVM(this);
            else if (repository is IRepository<IAssetEntry>)
                return new DatabaseAssetTableVM(this);
            else if (repository is IRepository<ITileSetEntry>)
                return new DatabaseTileSetTableVM(this);
            else if (repository is IRepository<ISpriteSetEntry>)
                return new DatabaseSpriteSetTableVM(this);
            else if (repository is IRepository<IPaletteEntry>)
                return new DatabasePaletteTableVM(this);
            else
                throw new NotImplementedException(repository.ToString());
        }

        internal IEnumerable<DatabaseTableVM> GetTables()
        {
            foreach (var repository in UnitOfWork.Repositories)
            {
                var tableVM = CreateTable(repository);
                tableVM.Load(repository);
                yield return tableVM;
            }
        }

        internal void Save()
        {
            throw new NotImplementedException();
        }

        #endregion Internal Methods

    }
}
