using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM;
using OpenBreed.Common.Sources;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Maps.Tools;
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

        public ProjectState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public List<SourceDef> GetAllSourcesOfType(string type)
        {
            return _databaseDef.Tables.OfType<DatabaseSourceTableDef>().FirstOrDefault().Items.FindAll(item => item.Type == type);
        }

        public PropertySetDef GetPropertySetDef(string propertySetName)
        {
            var propertySetDef = _databaseDef.Tables.OfType<DatabasePropertySetTableDef>().FirstOrDefault().Items.FirstOrDefault(item => item.Name == propertySetName);

            if (propertySetDef == null)
                throw new InvalidOperationException("Property set '" + propertySetName + "' not found!");

            return propertySetDef;
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
                return new DatabasePropertySetItemVM(this);
            if (itemDef is DatabaseTableDef)
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
