using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database
{
    public class DatabaseViewerVM : BaseViewModel
    {

        #region Public Fields

        public readonly EditorVM Root;

        #endregion Public Fields

        #region Internal Constructors

        internal DatabaseViewerVM(EditorVM root)
        {
            Root = root;

            TableSelector = new DatabaseTableSelectorVM(this);
            TableViewer = new DatabaseTableViewerVM(this);
        }

        #endregion Internal Constructors

        #region Public Properties

        public DatabaseTableSelectorVM TableSelector { get; private set; }
        public DatabaseTableViewerVM TableViewer { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Connect()
        {
            TableSelector.Connect();
            TableViewer.Connect();
        }

        #endregion Public Methods

    }
}
