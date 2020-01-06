using OpenBreed.Common;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public abstract class DbEntryVM : BaseViewModel
    {
        #region Private Fields

        private string description;

        private string id;

        #endregion Private Fields

        #region Public Constructors

        public DbEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public abstract IEntry Entry { get; }

        public virtual string Id
        {
            get { return id; }
            private set { SetProperty(ref id, value); }
        }

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
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