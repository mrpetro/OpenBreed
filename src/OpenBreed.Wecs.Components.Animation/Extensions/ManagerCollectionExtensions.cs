using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Wecs.Components.Animation.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Animation.Extensions
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupAnimationComponents(this IManagerCollection manCollection)
        {
            XmlComponentsList.RegisterComponentType<XmlAnimationComponent>();

            manCollection.AddTransient<AnimationComponentBuilder>(() => new AnimationComponentBuilder(manCollection.GetManager<IClipMan>()));

            manCollection.AddSingleton<AnimationComponentFactory>(() => new AnimationComponentFactory(manCollection));

            var entityFactory = manCollection.GetManager<IEntityFactory>();
            entityFactory.RegisterComponentFactory<XmlAnimationComponent>(manCollection.GetManager<AnimationComponentFactory>());
        }
    }
}
