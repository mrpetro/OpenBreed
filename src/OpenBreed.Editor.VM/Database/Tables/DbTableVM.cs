using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Tables;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Database.Entries;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public abstract class DbTableVM : BaseViewModel
    {
        #region Protected Constructors

        protected DbTableVM()
        {
            Entries = new BindingList<Entries.DbEntryVM>();
            Entries.ListChanged += (s, a) => OnPropertyChanged(nameof(Entries));
        }

        #endregion Protected Constructors

        #region Public Properties

        public BindingList<DbEntryVM> Entries { get; }

        public abstract string Name { get; }

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods

    }
}
