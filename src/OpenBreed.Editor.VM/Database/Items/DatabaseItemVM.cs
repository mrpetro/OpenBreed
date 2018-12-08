using OpenBreed.Common.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
{
    public class DatabaseItemVM
    {
        #region Public Fields

        public readonly DatabaseVM Owner;

        #endregion Public Fields

        #region Public Constructors

        public DatabaseItemVM(DatabaseVM owner)
        {
            Owner = owner;
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual string Name { get; private set; }

        public string Description { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public virtual void Load(DatabaseItemDef itemDef)
        {
            Name = itemDef.Name;
            Description = itemDef.Description;
        }

        #endregion Public Methods

    }
}
