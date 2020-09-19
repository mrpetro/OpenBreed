using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Maps;
using System;
using System.ComponentModel;
using System.Drawing;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteEditorVM : EntryEditorBaseExVM<IPaletteEntry>
    {
        #region Private Fields

        private IEntryEditor<IPaletteEntry> subeditor;

        #endregion Private Fields

        #region Public Constructors

        public PaletteEditorVM(IRepository repository) : base(repository)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntryEditor<IPaletteEntry> Subeditor
        {
            get { return subeditor; }
            private set { SetProperty(ref subeditor, value); }
        }

        public override string EditorName { get { return "Palette Editor"; } }

        #endregion Public Properties

        #region Protected Methods

        protected override void UpdateVM(IPaletteEntry entry)
        {
            if (entry is IPaletteFromBinaryEntry)
                Subeditor = new PaletteFromBinaryEditorVM(this);
            else if (entry is IPaletteFromMapEntry)
                Subeditor = new PaletteFromMapEditorVM(this);
            else
                throw new NotImplementedException();

            base.UpdateVM(entry);
            Subeditor.UpdateVM(entry);
        }

        protected override void UpdateEntry(IPaletteEntry target)
        {
            base.UpdateEntry(target);
            Subeditor.UpdateEntry(target);
        }

        #endregion Protected Methods
    }
}