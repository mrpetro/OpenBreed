using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Props;
using OpenBreed.Common.XmlDatabase.Items.Sources;
using OpenBreed.Common.XmlDatabase.Items.Sprites;
using OpenBreed.Common.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DatabaseSpriteSetItemVM : DatabaseItemVM
    {
        #region Private Fields

        private ISpriteSetEntity _entry;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseSpriteSetItemVM(DatabaseVM owner) : base(owner)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load(IEntity entry)
        {
            _entry = entry as ISpriteSetEntity ?? throw new InvalidOperationException($"Expected {nameof(ISpriteSetEntity)}");

            base.Load(entry);
        }

        public override void Open()
        {
            throw new NotImplementedException();

            //Owner.Root.PropSetEditor.TryLoad(_model);
            Owner.OpenedItem = this;
        }

        #endregion Public Methods
    }
}
