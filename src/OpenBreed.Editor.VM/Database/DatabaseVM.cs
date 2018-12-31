using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM;
using OpenBreed.Common.Sources;
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

namespace OpenBreed.Editor.VM.Database
{
    public enum ProjectState
    {
        Closed,
        New,
        Opened,
        Closing
    }

    public class DatabaseVM : BaseViewModel
    {
        #region Private Fields

        private DatabaseItemVM _openedItem;
        private ProjectState _state;

        #endregion Private Fields

        #region Internal Constructors

        internal DatabaseVM(EditorVM root, IUnitOfWork unitOfWork)
        {
            Root = root;
            UnitOfWork = unitOfWork;
        }

        #endregion Internal Constructors

        #region Public Properties

        public string FilePath { get; private set; }
        public bool IsModified { get; internal set; }
        public string Name { get; set; }
        public DatabaseItemVM OpenedItem
        {
            get { return _openedItem; }
            set { SetProperty(ref _openedItem, value); }
        }

        public EditorVM Root { get; private set; }
        public ProjectState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        #endregion Public Properties

        #region Internal Properties

        internal IUnitOfWork UnitOfWork { get; }

        #endregion Internal Properties

        #region Public Methods

        #endregion Public Methods

        #region Internal Methods

        internal DatabaseItemVM CreateItem(IEntity entity)
        {
            if (entity is IImageEntity)
                return new DatabaseImageItemVM(this);
            else if (entity is ILevelEntity)
                return new DatabaseLevelItemVM(this);
            else if (entity is ISourceEntity)
                return new DatabaseSourceItemVM(this);
            else if (entity is IPropSetEntity)
                return new DatabasePropSetItemVM(this);
            else if (entity is ITileSetEntity)
                return new DatabaseTileSetItemVM(this);
            else if (entity is ISpriteSetEntity)
                return new DatabaseSpriteSetItemVM(this);
            else if (entity is IPaletteEntity)
                return new DatabasePaletteItemVM(this);
            else
                throw new NotImplementedException(entity.ToString());
        }

        internal DatabaseTableVM CreateTable(IRepository repository)
        {
            if (repository is IRepository<IImageEntity>)
                return new DatabaseImageTableVM(this);
            else if (repository is IRepository<ILevelEntity>)
                return new DatabaseLevelTableVM(this);
            else if (repository is IRepository<IPropSetEntity>)
                return new DatabasePropertySetTableVM(this);
            else if (repository is IRepository<ISourceEntity>)
                return new DatabaseSourceTableVM(this);
            else if (repository is IRepository<ITileSetEntity>)
                return new DatabaseTileSetTableVM(this);
            else if (repository is IRepository<ISpriteSetEntity>)
                return new DatabaseSpriteSetTableVM(this);
            else if (repository is IRepository<IPaletteEntity>)
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
