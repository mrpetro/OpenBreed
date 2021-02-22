using OpenBreed.Common;
using OpenBreed.Rendering.Interface.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Rendering.Extensions
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupRenderingSystems(this IManagerCollection manCollection)
        {
            var systemFactory = manCollection.GetManager<ISystemFactory>();
            systemFactory.Register(() => new SpriteSystem(manCollection.GetManager<ISpriteMan>()));

            //systemFactory.Register<TileSystem>(() => new TileSystem(manCollection.GetManager<ILogger>()));
            systemFactory.Register(() => new TextPresenterSystem(manCollection.GetManager<IFontMan>()));
            systemFactory.Register(() => new TextSystem(manCollection.GetManager<IFontMan>()));
        }
    }
}
