using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public abstract class DbEntryVM : BaseViewModel
    {
        private string _description;

        #region Public Fields

        #endregion Public Fields

        #region Public Constructors

        public DbEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual string Id { get; private set; }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public virtual void Load(IEntry entry)
        {
            Id = entry.Id;
            Description = entry.Description;
        }

        #endregion Public Methods

    }
}
