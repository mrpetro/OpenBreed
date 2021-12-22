using OpenBreed.Wecs.Components.Common.Xml;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Common.Extensions
{
    public static class XmlCommonComponents
    {
        public static void Setup()
        {
            XmlComponentsList.RegisterComponentType<XmlPositionComponent>();
            XmlComponentsList.RegisterComponentType<XmlGridPositionComponent>();
            XmlComponentsList.RegisterComponentType<XmlMetadataComponent>();
            XmlComponentsList.RegisterComponentType<XmlThrustComponent>();
            XmlComponentsList.RegisterComponentType<XmlVelocityComponent>();
            XmlComponentsList.RegisterComponentType<XmlTimerComponent>();
            XmlComponentsList.RegisterComponentType<XmlAngularPositionComponent>();
            XmlComponentsList.RegisterComponentType<XmlAngularVelocityComponent>();
            XmlComponentsList.RegisterComponentType<XmlAngularThrustComponent>();
            XmlComponentsList.RegisterComponentType<XmlMessagingComponent>();
            XmlComponentsList.RegisterComponentType<XmlFollowedComponent>();
        }
    }
}
