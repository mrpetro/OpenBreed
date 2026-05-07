using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public class DbTablesEditorMainVM : BaseViewModel
    {
        #region Public Constructors

        public DbTablesEditorMainVM(
            DbTablesEditorVM tablesEditor,
            DbEntriesEditorVM entriesEditor)
        {
            TablesEditor = tablesEditor;
            EntriesEditor = entriesEditor;
            TablesEditor.EntryEditorOpener = OpenEntryEditor;
        }

        #endregion Public Constructors

        #region Public Properties

        public DbTablesEditorVM TablesEditor { get; }

        public DbEntriesEditorVM EntriesEditor { get; }

        #endregion Public Properties

        #region Internal Methods

        internal EntryEditorVM OpenEntryEditor(string tableName, string entryId)
        {
            var entryEditor = EntriesEditor.OpenOrActivateEditor(tableName, entryId);

            return entryEditor;
        }

        #endregion Internal Methods
    }
}