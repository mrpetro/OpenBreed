using OpenBreed.Common;
using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Items.Props;
using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Props;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DatabasePropSetItemVM : DatabaseItemVM
    {
        #region Private Fields

        private IPropSetEntity _entry;

        #endregion Private Fields

        #region Public Constructors

        public DatabasePropSetItemVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load(IEntity entry)
        {
            _entry = entry as IPropSetEntity ?? throw new InvalidOperationException($"Expected {nameof(IPropSetEntity)}");

            base.Load(entry);
        }

        public override void Open()
        {
            Owner.Root.PropSetEditor.TryLoad(_entry.Name);
            Owner.OpenedItem = this;
        }

        #endregion Public Methods
    }
}
