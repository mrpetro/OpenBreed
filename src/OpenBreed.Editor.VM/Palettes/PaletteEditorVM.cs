using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Palettes
{
    public class PaletteEditorVM : EntryEditorBaseVM<IPaletteEntry, PaletteVM>
    {

        #region Public Constructors

        public PaletteEditorVM(IRepository repository) : base(repository)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName { get { return "Palette Editor"; } }

        public MapEditorPalettesToolVM Palettes { get; private set; }

        #endregion Public Properties

        #region Protected Methods

        protected override PaletteVM CreateVM(IPaletteEntry entry)
        {
            if (entry is IPaletteFromBinaryEntry)
                return new PaletteFromBinaryVM();
            else if (entry is IPaletteFromMapEntry)
                return new PaletteFromMapVM();
            else
                throw new NotImplementedException();
        }

        protected override void UpdateEntry(PaletteVM source, IPaletteEntry target)
        {
            var model = DataProvider.Palettes.GetPalette(target.Id);

            for (int i = 0; i < model.Length; i++)
            {
                model.Data[i] = source.Colors[i];
            }

            base.UpdateEntry(source, target);
        }

        #endregion Protected Methods

    }
}
