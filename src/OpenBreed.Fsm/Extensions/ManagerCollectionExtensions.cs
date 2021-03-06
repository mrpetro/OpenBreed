﻿using OpenBreed.Common;
using OpenBreed.Fsm.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Fsm.Extensions
{
    public static class ManagerCollectionExtensions
    {
        #region Public Methods

        public static void SetupFsmComponents(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<IFsmMan>(() => new FsmMan());

            XmlComponentsList.RegisterComponentType<XmlFsmComponent>();

            manCollection.AddSingleton<FsmComponentFactory>(() => new FsmComponentFactory(manCollection));
            manCollection.AddTransient<FsmComponentBuilder>(() => new FsmComponentBuilder(manCollection.GetManager<IFsmMan>()));

            var entityFactory = manCollection.GetManager<IEntityFactory>();
            entityFactory.RegisterComponentFactory<XmlFsmComponent>(manCollection.GetManager<FsmComponentFactory>());
        }

        #endregion Public Methods
    }
}