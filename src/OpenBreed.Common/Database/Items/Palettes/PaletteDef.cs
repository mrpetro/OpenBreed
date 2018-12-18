using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Database.Items.Palettes
{
    public class PaletteDef : DatabaseItemDef
    {
        public string SourceRef { get; set; }
        public FormatDef Format { get; set; }
    }
}
