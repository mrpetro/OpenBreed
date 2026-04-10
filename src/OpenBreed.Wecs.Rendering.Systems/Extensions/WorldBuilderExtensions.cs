using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Rendering.Systems.Extensions
{
    public static class WorldBuilderExtensions
    {
        public static IWorldBuilder AddRenderingSystems(this IWorldBuilder builder)
        {
            builder.AddSystem<StampPutterSystem>();
            builder.AddSystem<TilePutterSystem>();
            builder.AddSystem<TileRenderSystem>();
            builder.AddSystem<SpriteSystem>();
            builder.AddSystem<PictureSystem>();
            builder.AddSystem<TextSystem>();
            builder.AddSystem<ViewportSystem>();

            return builder;
        }
    }
}
