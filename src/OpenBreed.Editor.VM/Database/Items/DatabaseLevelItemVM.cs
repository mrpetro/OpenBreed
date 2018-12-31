using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Levels;
using OpenBreed.Common.Maps;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DatabaseLevelItemVM : DatabaseItemVM
    {
        #region Private Fields

        private ILevelEntity _entry;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseLevelItemVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load(IEntity entry)
        {
            _entry = entry as ILevelEntity ?? throw new InvalidOperationException($"Expected {nameof(ILevelEntity)}");

            base.Load(entry);     
        }

        public override void Open()
        {
            Owner.Root.LevelEditor.Load(_entry.Name);
            Owner.OpenedItem = this;
        }

        #endregion Public Methods
    }
}
