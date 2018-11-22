using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Maps.Builders;
using OpenBreed.Common.Maps.Readers.MAP;

using OpenBreed.Common.Tiles;

namespace OpenBreed.Common.Maps
{
    public delegate void TileSetChangedEventHandler(object sender, TileSetChangedEventArgs e);

    public class TileSetChangedEventArgs : EventArgs
    {
        public TileSetModel TileSet { get; set; }

        public TileSetChangedEventArgs(TileSetModel tileSet)
        {
            TileSet = tileSet;
        }
    }

    public delegate void PaletteChangedEventHandler(object sender, PaletteChangedEventArgs e);

    public class PaletteChangedEventArgs : EventArgs
    {
        public PaletteModel Palette { get; set; }

        public PaletteChangedEventArgs(PaletteModel palette)
        {
            Palette = palette;
        }
    }

    public class MapModel : IDisposable
    {
        private MapPropertiesModel _properties;
        private MapMissionModel _mission;
        private MapBodyModel _body;

        public MapPropertiesModel Properties
        {
            get { return _properties; }
            private set
            {
                _properties = value;
                _properties.Owner = this;
            }
        }

        public MapMissionModel Mission
        {
            get { return _mission; }
            private set
            {
                _mission = value;
                _mission.Owner = this;
            }
        }

        public MapBodyModel Body
        {
            get { return _body; }
            private set
            {
                _body = value;
                _body.Owner = this;
            }
        }

        internal MapModel(MapBuilder builder)
        {
            Properties = builder.Properties;
            Mission = builder.Mission;
            Body = builder.Body;
        }

        public void Dispose()
        {
        }
    }
}
