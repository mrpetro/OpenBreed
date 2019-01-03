using OpenBreed.Common;
using OpenBreed.Common.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DatabaseSoundItemVM : DatabaseItemVM
    {
        #region Private Fields

        private ISoundEntity _entry;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseSoundItemVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load(IEntity entry)
        {
            _entry = entry as ISoundEntity ?? throw new InvalidOperationException($"Expected {nameof(ISoundEntity)}");

            base.Load(entry);
        }

        public override void Open()
        {
            Owner.Root.SoundEditor.OpenEntry(_entry.Name);
            Owner.OpenedItem = this;
        }

        #endregion Public Methods
    }
}
