﻿using System;
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

        private DatabaseDef _databaseDef;
        private DatabaseItemVM _openedItem;
        private ProjectState _state;

        #endregion Private Fields

        #region Internal Constructors

        internal DatabaseVM(EditorVM root)
        {
            Root = root;
        }

        #endregion Internal Constructors

        #region Public Properties

        public string FilePath { get; private set; }

        public string Name { get; set; }


        public DatabaseItemVM OpenedItem
        {
            get { return _openedItem; }
            set { SetProperty(ref _openedItem, value); }
        }

        public EditorVM Root { get; private set; }

        internal void Save()
        {
            throw new NotImplementedException();
        }

        public ProjectState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        public bool IsModified { get; internal set; }

        #endregion Public Properties

        #region Public Methods

        public PaletteDef GetPaletteDef(string paletteName)
        {
            var paletteDef = _databaseDef.Tables.OfType<DatabasePaletteTableDef>().FirstOrDefault().Items.FirstOrDefault(item => item.Name == paletteName);

            if (paletteDef == null)
                throw new InvalidOperationException("Palette '" + paletteName + "' not found!");

            return paletteDef;
        }

        public LevelDef GetLevelDef(string levelName)
        {
            var levelDef = _databaseDef.Tables.OfType<DatabaseLevelTableDef>().FirstOrDefault().Items.FirstOrDefault(item => item.Name == levelName);

            if (levelDef == null)
                throw new InvalidOperationException("Level '" + levelName + "' not found!");

            return levelDef;
        }

        public PropertySetDef GetPropSetDef(string propertySetName)
        {
            var propertySetDef = _databaseDef.Tables.OfType<DatabasePropertySetTableDef>().FirstOrDefault().Items.FirstOrDefault(item => item.Name == propertySetName);

            if (propertySetDef == null)
                throw new InvalidOperationException("Property set '" + propertySetName + "' not found!");

            return propertySetDef;
        }

        public TileSetDef GetTileSetDef(string tileSetName)
        {
            var tileSetDef = _databaseDef.Tables.OfType<DatabaseTileSetTableDef>().FirstOrDefault().Items.FirstOrDefault(item => item.Name == tileSetName);

            if (tileSetDef == null)
                throw new InvalidOperationException("Tile set '" + tileSetName + "' not found!");

            return tileSetDef;
        }

        public SpriteSetDef GetSpriteSetDef(string spriteSetName)
        {
            var spriteSetDef = _databaseDef.Tables.OfType<DatabaseSpriteSetTableDef>().FirstOrDefault().Items.FirstOrDefault(item => item.Name == spriteSetName);

            if (spriteSetDef == null)
                throw new InvalidOperationException("Sprite set '" + spriteSetName + "' not found!");

            return spriteSetDef;
        }

        public SourceDef GetSourceDef(string sourceRef)
        {
            var sourceDef = _databaseDef.Tables.OfType<DatabaseSourceTableDef>().FirstOrDefault().Items.FirstOrDefault(item => item.Name == sourceRef);

            if (sourceDef == null)
                throw new InvalidOperationException("Source " + sourceRef + " not found!");

            return sourceDef;
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
            foreach (var tableDef in _databaseDef.Tables)
            {
                var tableVM = CreateTable(tableDef);
                tableVM.Load(tableDef);
                yield return tableVM;
            }
        }

        internal void Load(string filePath )
        {
            _databaseDef = Tools.RestoreFromXml<DatabaseDef>(filePath);
            FilePath = filePath;
        }

        #endregion Internal Methods

    }
}
