using OpenBreed.Common.Data;
using OpenBreed.Common.DataSources;
using OpenBreed.Common.Extensions;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Readers.Images.IFF;
using OpenBreed.Common.Tools.Collections;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Model.Images;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Sounds;
using OpenBreed.Reader.Legacy.Images.ACBM;
using OpenBreed.Reader.Legacy.Maps.MAP;
using OpenBreed.Writer.Legacy.Maps.MAP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class MapPaletteDataHandler : AssetDataHandlerBase<IDbPaletteFromMap>
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly IBitmapProvider bitmapProvider;
        private readonly IDrawingFactory drawingFactory;
        private readonly Lazy<MapsDataProvider> lazyMapsDataProvider;

        #endregion Private Fields

        #region Public Constructors

        public MapPaletteDataHandler(
            IBitmapProvider bitmapProvider,
            IDrawingFactory drawingFactory,
            Lazy<MapsDataProvider> mapsDataProvider,
            IRepositoryProvider repositoryProvider)
        {
            this.bitmapProvider = bitmapProvider;
            this.drawingFactory = drawingFactory;
            this.lazyMapsDataProvider = mapsDataProvider;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void Save(IDbPaletteFromMap dbEntry, object model)
        {
            var mapEntry = repositoryProvider.GetRepository<IDbMap>().GetById(dbEntry.MapRef);

            if (mapEntry is null)
            {
                throw new Exception($"Unknown MapRef '{dbEntry.MapRef}'.");
            }

            var mapModel = lazyMapsDataProvider.Value.GetMap(mapEntry);

            if (mapModel is null)
            {
                throw new Exception($"Map problem '{mapEntry.Id}'.");
            }

            throw new NotImplementedException();
        }
        protected override object Load(IDbPaletteFromMap dbEntry)
        {
            var mapEntry = repositoryProvider.GetRepository<IDbMap>().GetById(dbEntry.MapRef);

            if (mapEntry is null)
            {
                throw new Exception($"Unknown MapRef '{dbEntry.MapRef}'.");
            }

            var mapModel = lazyMapsDataProvider.Value.GetMap(mapEntry);

            if (mapModel is null)
            {
                throw new Exception($"Map problem '{mapEntry.Id}'.");
            }

            return PalettesDataHelper.FromMapModel(mapModel, dbEntry);
        }

        #endregion Protected Methods
    }
}