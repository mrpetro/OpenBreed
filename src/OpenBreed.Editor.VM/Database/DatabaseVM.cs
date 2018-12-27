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
using OpenBreed.Common.Database;
using System.ComponentModel;
using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Database.Tables.Sources;
using OpenBreed.Common.Database.Items.Levels;
using OpenBreed.Common.Database.Items.Images;
using OpenBreed.Common.Database.Tables;
using OpenBreed.Common.Database.Tables.Images;
using OpenBreed.Editor.VM.Database.Tables;
using OpenBreed.Common.Database.Tables.Levels;
using OpenBreed.Common.Database.Tables.Props;
using OpenBreed.Common.Database.Items.Props;
using OpenBreed.Common.Database.Items.Tiles;
using OpenBreed.Common.Database.Tables.Tiles;
using OpenBreed.Common.Database.Items.Sprites;
using OpenBreed.Common.Database.Tables.Sprites;
using OpenBreed.Common.Database.Items.Palettes;
using OpenBreed.Common.Database.Tables.Palettes;

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

        public LevelDef GetLevelDef(string levelName)
        {
            return UnitOfWork.GetLevelDef(levelName);

        }

        public PaletteDef GetPaletteDef(string paletteName)
        {
            return UnitOfWork.GetPaletteDef(paletteName);
        }

        public PropertySetDef GetPropSetDef(string propertySetName)
        {
            return UnitOfWork.GetPropSetDef(propertySetName);
        }

        public SpriteSetDef GetSpriteSetDef(string spriteSetName)
        {
            return UnitOfWork.GetSpriteSetDef(spriteSetName);
        }

        public TileSetDef GetTileSetDef(string tileSetName)
        {
            return UnitOfWork.GetTileSetDef(tileSetName);
        }

        #endregion Public Methods

        #region Internal Methods

        internal DatabaseItemVM CreateItem(DatabaseItemDef itemDef)
        {
            if (itemDef is ImageDef)
                return new DatabaseImageItemVM(this);
            else if (itemDef is LevelDef)
                return new DatabaseLevelItemVM(this);
            else if (itemDef is SourceDef)
                return new DatabaseSourceItemVM(this);
            else if (itemDef is PropertySetDef)
                return new DatabasePropSetItemVM(this);
            else if (itemDef is TileSetDef)
                return new DatabaseTileSetItemVM(this);
            else if (itemDef is SpriteSetDef)
                return new DatabaseSpriteSetItemVM(this);
            else if (itemDef is PaletteDef)
                return new DatabasePaletteItemVM(this);
            else if (itemDef is DatabaseTableDef)
                return CreateTable((DatabaseTableDef)itemDef);
            else
                throw new NotImplementedException(itemDef.ToString());
        }

        internal DatabaseTableVM CreateTable(DatabaseTableDef tableDef)
        {
            if (tableDef is DatabaseImageTableDef)
                return new DatabaseImageTableVM(this);
            else if (tableDef is DatabaseLevelTableDef)
                return new DatabaseLevelTableVM(this);
            else if (tableDef is DatabasePropertySetTableDef)
                return new DatabasePropertySetTableVM(this);
            else if (tableDef is DatabaseSourceTableDef)
                return new DatabaseSourceTableVM(this);
            else if (tableDef is DatabaseTileSetTableDef)
                return new DatabaseTileSetTableVM(this);
            else if (tableDef is DatabaseSpriteSetTableDef)
                return new DatabaseSpriteSetTableVM(this);
            else if (tableDef is DatabasePaletteTableDef)
                return new DatabasePaletteTableVM(this);
            else
                throw new NotImplementedException(tableDef.ToString());
        }

        internal IEnumerable<DatabaseTableVM> GetTables()
        {
            foreach (var tableDef in UnitOfWork.GetTables())
            {
                var tableVM = CreateTable(tableDef);
                tableVM.Load(tableDef);
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
