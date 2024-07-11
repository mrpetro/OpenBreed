using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Sprites
{
    public interface IDbSpriteAtlasFromSpr : IDbSpriteAtlas
    {
        string DataRef { get; }

        ReadOnlyCollection<IDbPoint2i> Sizes { get; }
    }
}
