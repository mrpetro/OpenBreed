using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbEntityTemplateEntryVM : DbEntryVM
    {
        #region Private Fields

        private IEntityTemplateEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbEntityTemplateEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as IEntityTemplateEntry ?? throw new InvalidOperationException($"Expected {nameof(IEntityTemplateEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}