using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Maps.Builders
{
    public class MapBuilder
    {
        internal MapPropertiesModel Properties = null;
        internal MapMissionModel Mission = null;
        internal MapBodyModel Body = null;

        public static MapBuilder NewMapModel()
        {
            return new MapBuilder();
        }

        public MapBuilder SetProperties(MapPropertiesModel properties)
        {
            Properties = properties;
            return this;
        }

        public MapBuilder SetBody(MapBodyModel body)
        {
            Body = body;
            return this;
        }

        public MapBuilder SetMission(MapMissionModel mission)
        {
            Mission = mission;
            return this;
        }

        public MapModel Build()
        {
            return new MapModel(this);
        }
    }
}
