using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Interface.Items.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Interface.Data
{
    public interface ISpriteAtlasDataLoader : IDataLoader<ISpriteAtlas>
    {
        #region Public Methods

        ISpriteAtlas Load(IDbSpriteAtlas dbSpriteAtlas);

        #endregion Public Methods
    }
}