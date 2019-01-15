using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Editor.VM.Assets;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database.Items;
using OpenBreed.Editor.VM.Images;
using OpenBreed.Editor.VM.Levels;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Sounds;
using OpenBreed.Editor.VM.Tiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public class DbEditorVM : BaseViewModel
    {

        #region Private Fields

        private readonly EntryEditorFactory _entryEditorFactory = new EntryEditorFactory();
        private DatabaseVM _currentDb;
        private IDatabase _edited;

        private ProjectState _state;

        #endregion Private Fields

        #region Public Constructors

        public DbEditorVM(EditorVM root)
        {
            Root = root;

            DbTablesEditor = new DbTablesEditorVM();

            _entryEditorFactory.Register<DbTileSetEntryVM, TileSetEditorVM>();
            _entryEditorFactory.Register<DbPropSetEntryVM, PropSetEditorVM>();
            _entryEditorFactory.Register<DbPaletteEntryVM, PaletteEditorVM>();
            _entryEditorFactory.Register<DbImageEntryVM, ImageEditorVM>();
            _entryEditorFactory.Register<DbSoundEntryVM, SoundEditorVM>();
            _entryEditorFactory.Register<DbLevelEntryVM, LevelEditorVM>();
            _entryEditorFactory.Register<DbAssetEntryVM, AssetEditorVM>();

            DbEntryEditors = new BindingList<EntryEditorVM>();
            DbEntryEditors.ListChanged += (s, a) => OnPropertyChanged(nameof(DbEntryEditors));

        }

        #endregion Public Constructors

        #region Public Properties


        public DatabaseVM CurrentDb
        {
            get { return _currentDb; }
            set { SetProperty(ref _currentDb, value); }
        }

        public DbTablesEditorVM DbTablesEditor { get; }

        public Action<EntryEditorVM> EditorOpeningAction { get; set; }

        public BindingList<EntryEditorVM> DbEntryEditors { get; }

        public string FilePath { get; private set; }

        public bool IsModified { get; internal set; }

        public string Name { get; set; }

        public EditorVM Root { get; private set; }

        public ProjectState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void EditModel(IDatabase model)
        {
            //Unsubscribe to previous edited item changes
            if (CurrentDb != null)
                CurrentDb.PropertyChanged -= CurrentDb_PropertyChanged;

            _edited = model;

            var vm = new DatabaseVM(this.Root, model);
            UpdateVM(model, vm);
            CurrentDb = vm;
            CurrentDb.PropertyChanged += CurrentDb_PropertyChanged;
        }

        public EntryEditorVM OpenEditor(Type entryType)
        {
            var editor = _entryEditorFactory.CreateEditor(entryType);
            DbEntryEditors.Add(editor);

            if(EditorOpeningAction != null)
                EditorOpeningAction(editor);

            editor.CloseAction = () => OnEditorClose(editor);

            return editor;
        }

        private void OnEditorClose(EntryEditorVM editor)
        {
            DbEntryEditors.Remove(editor);
        }

        public IDatabase OpenXmlDatabase(string xmlFilePath)
        {
            return new XmlDatabase(xmlFilePath, DatabaseMode.Read);
        }
        public bool TryCloseDatabase()
        {
            return DbEditorVMHelper.TryCloseDatabase(this);
        }

        public void TryOpenDatabase()
        {
            DbEditorVMHelper.TryOpenDatabase(this);
        }

        public void TrySaveDatabase()
        {
            DbEditorVMHelper.TrySaveDatabase(this);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Save()
        {
            throw new NotImplementedException();
        }

        #endregion Internal Methods

        #region Private Methods

        private void CurrentDb_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void UpdateVM(IDatabase source, DatabaseVM target)
        {
            target.Name = source.Name;
        }

        #endregion Private Methods

        //internal DbEntryVM CreateItem(IEntry entry)
        //{
        //    if (entry is IImageEntry)
        //        return new DbImageEntryVM(this, Root.ImageEditor);
        //    else if (entry is ISoundEntry)
        //        return new DbSoundEntryVM(this, Root.SoundEditor);
        //    else if (entry is ILevelEntry)
        //        return new DbLevelEntryVM(this, null);
        //    else if (entry is ISourceEntry)
        //        return new DbSourceEntryVM(this, null);
        //    else if (entry is IPropSetEntry)
        //        return new DbPropSetEntryVM(this, Root.PropSetEditor);
        //    else if (entry is ITileSetEntry)
        //        return new DbTileSetEntryVM(this, Root.TileSetEditor);
        //    else if (entry is ISpriteSetEntry)
        //        return new DbSpriteSetEntryVM(this, null);
        //    else if (entry is IPaletteEntry)
        //        return new DbPaletteEntryVM(this, Root.PaletteEditor);
        //    else
        //        throw new NotImplementedException(entry.ToString());
        //}

        //internal DatabaseTableVM CreateTable(IRepository repository)
        //{
        //    if (repository is IRepository<IImageEntry>)
        //        return new DatabaseImageTableVM(this);
        //    if (repository is IRepository<ISoundEntry>)
        //        return new DatabaseSoundTableVM(this);
        //    else if (repository is IRepository<ILevelEntry>)
        //        return new DatabaseLevelTableVM(this);
        //    else if (repository is IRepository<IPropSetEntry>)
        //        return new DatabasePropertySetTableVM(this);
        //    else if (repository is IRepository<ISourceEntry>)
        //        return new DatabaseSourceTableVM(this);
        //    else if (repository is IRepository<ITileSetEntry>)
        //        return new DatabaseTileSetTableVM(this);
        //    else if (repository is IRepository<ISpriteSetEntry>)
        //        return new DatabaseSpriteSetTableVM(this);
        //    else if (repository is IRepository<IPaletteEntry>)
        //        return new DatabasePaletteTableVM(this);
        //    else
        //        throw new NotImplementedException(repository.ToString());
        //}

        //internal IEnumerable<DatabaseTableVM> GetTables()
        //{
        //    foreach (var repository in UnitOfWork.Repositories)
        //    {
        //        var tableVM = CreateTable(repository);
        //        tableVM.Load(repository);
        //        yield return tableVM;
        //    }
        //}
    }
}
