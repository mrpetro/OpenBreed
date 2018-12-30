using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Maps
{
    public interface ILevelEntity : IEntity
    {
        string SourceRef { get; }
        IFormatEntity Format { get; }

        string TileSetRef { get; }
        string PropertySetRef { get; }

        List<string> SpriteSetRefs { get; }
        List<string> PaletteRefs { get; }
    }
}
