using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public class DbTablesEditorVM : BaseViewModel
    {

        #region Public Fields

        #endregion Public Fields

        #region Internal Constructors

        internal DbTablesEditorVM()
        {
            DbTableSelector = new DbTableSelectorVM();
            DbTableEditor = new DbTableEditorVM();
        }

        #endregion Internal Constructors

        #region Public Properties

        public DbTableSelectorVM DbTableSelector { get; private set; }
        public DbTableEditorVM DbTableEditor { get; private set; }

        #endregion Public Properties

    }
}
