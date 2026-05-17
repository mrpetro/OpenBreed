using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Wecs.Abstractions;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.Game.Wecs.Components.Xml
{
    [XmlRoot("Resurrectable")]
    public class XmlResurrectableComponent : XmlComponentTemplate, IResurrectableComponentTemplate
    {
        #region Public Properties

        [XmlElement("WorldName")]
        public string WorldName { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            var worldMan = serviceProvider.GetRequiredService<IWorldMan>();

            var worldId = WecsConsts.NO_WORLD_ID;

            var world = WorldName is null ? null : worldMan.GetByName(WorldName);

            if (world is not null)
                worldId = world.Id;

            return new ResurrectableComponent(worldId);
        }

        #endregion Public Methods
    }
}