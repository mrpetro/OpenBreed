using OpenBreed.Common;
using OpenBreed.Common.XmlDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Items
{
    public abstract class DatabaseEntryVM
    {

        #region Public Fields

        public readonly DatabaseVM Owner;

        #endregion Public Fields

        #region Public Constructors

        public DatabaseEntryVM(DatabaseVM owner, EntryEditorVM editor)
        {
            Owner = owner;
            Editor = editor;
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual string Name { get; private set; }
        public string Description { get; private set; }
        public EntryEditorVM Editor { get; }

        #endregion Public Properties

        #region Public Methods

        public virtual void Load(IEntry entry)
        {
            Name = entry.Name;
            //Description = itemDef.Description;
        }

        public virtual void Open()
        {

        }

        #endregion Public Methods

    }
}
