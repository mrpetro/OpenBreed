using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Database.Resources;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Sources;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.Tiles;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Props;

namespace OpenBreed.Editor.VM.Levels.Builders
{
    public class LevelBuilder
    {
        internal BaseSource Source;
        internal int Id;
        internal string Name;
        internal MapModel Map;
        internal PropertySetModel PropertySet;

        public LevelBuilder()
        {
            Name = string.Empty;
        }

        public LevelBuilder SetSource(BaseSource source)
        {
            Source = source;
            return this;
        }

        internal LevelBuilder SetId(int id)
        {
            Id = id;
            return this;
        }

        internal LevelBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        public LevelBuilder SetMap(MapModel map)
        {
            Map = map;
            return this;
        }

        public LevelBuilder SetPropertySet(PropertySetModel propertySet)
        {
            PropertySet = propertySet;
            return this;
        }

        public static LevelBuilder NewLevel()
        {
            return new LevelBuilder();
        }

        public ProjectDef Build()
        {
            return new ProjectDef(this);
        }
    }
}
