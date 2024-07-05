using OpenBreed.Common.Data;
using OpenBreed.Common.DataSources;
using OpenBreed.Common.Extensions;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Readers.Images.IFF;
using OpenBreed.Common.Tools.Collections;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Model;
using OpenBreed.Model.Images;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Sounds;
using OpenBreed.Reader.Legacy.Images.ACBM;
using OpenBreed.Reader.Legacy.Maps.MAP;
using OpenBreed.Reader.Legacy.Palettes;
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
    public class BinaryPaletteDataHandler : AssetDataHandlerBase<IDbPaletteFromBinary>
    {
        #region Private Fields

        private readonly IBitmapProvider bitmapProvider;
        private readonly IDrawingFactory drawingFactory;
        private readonly DataSourceProvider dataSourceProvider;

        #endregion Private Fields

        #region Public Constructors

        public BinaryPaletteDataHandler(
            IBitmapProvider bitmapProvider,
            IDrawingFactory drawingFactory,
            DataSourceProvider dataSourceProvider)
        {
            this.bitmapProvider = bitmapProvider;
            this.drawingFactory = drawingFactory;
            this.dataSourceProvider = dataSourceProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        protected override void Save(IDbPaletteFromBinary dbEntry, object model)
        {
            var ds = dataSourceProvider.GetDataSource(dbEntry.DataRef);

            if (ds is null)
            {
                throw new Exception($"Unknown DataSourceRef '{dbEntry.DataRef}'.");
            }

            throw new NotImplementedException();
        }

        protected override object Load(IDbPaletteFromBinary dbEntry)
        {
            var ds = dataSourceProvider.GetDataSource(dbEntry.DataRef);

            if (ds is null)
            {
                throw new Exception($"Unknown DataSourceRef '{dbEntry.DataRef}'.");
            }

            return Load(ds, dbEntry.DataStart, dbEntry.ColorsNo, dbEntry.Mode);
        }

        #endregion Protected Methods

        #region Private Methods

        private object Load(DataSourceBase ds, int dataStart, int colorsNo, PaletteMode mode)
        {
            ds.Stream.Seek(dataStart, SeekOrigin.Begin);

            var paletteBuilder = PaletteBuilder.NewPaletteModel();
            var paletteReader = new PaletteReader(paletteBuilder, PalettesDataHelper.ToPaletteMode(mode), colorsNo);
            return paletteReader.Read(ds.Stream);
        }

        #endregion Private Methods
    }
}