using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Levels.Builders;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Database.Resources;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Sources;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.Tiles;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Props;

namespace OpenBreed.Editor.VM.Levels
{
    public class ProjectDef : IDisposable
    {
        public MapModel Map { get; private set; }
        public TileSetModel TileSet { get; private set; }
        public PropertySetModel PropertySet { get; private set; }
        public List<SpriteSetModel> SpriteSets { get; private set; }

        public string Name { get; set; }

        internal ProjectDef(LevelBuilder builder)
        {
            Map = builder.Map;
            PropertySet = builder.PropertySet;
            Name = builder.Name;
        }

        public void Dispose()
        {
            Map.Dispose();
            PropertySet.Dispose();
        }
    }
}
