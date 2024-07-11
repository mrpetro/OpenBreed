using OpenBreed.Common.Data;
using OpenBreed.Common.DataSources;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Readers.Images.IFF;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Model.Images;
using OpenBreed.Model.Sounds;
using OpenBreed.Reader.Legacy.Images.ACBM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class AcbmImageDataHandler : AssetDataHandlerBase<IDbAcbmImage>
    {
        #region Private Fields

        private readonly IBitmapProvider bitmapProvider;

        private readonly DataSourceProvider dataSourceProvider;

        #endregion Private Fields

        #region Public Constructors

        public AcbmImageDataHandler(
            IBitmapProvider bitmapProvider,
            DataSourceProvider dataSourceProvider)
        {
            this.bitmapProvider = bitmapProvider;
            this.dataSourceProvider = dataSourceProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public static ACBMImageReader.ACBMPaletteMode ToACBMPaletteMode(string paletteMode)
        {
            switch (paletteMode)
            {
                case "NONE":
                    return ACBMImageReader.ACBMPaletteMode.NONE;

                case "16BIT":
                    return ACBMImageReader.ACBMPaletteMode.PALETTE_16BIT;

                case "32BIT":
                    return ACBMImageReader.ACBMPaletteMode.PALETTE_32BIT;

                default:
                    throw new InvalidOperationException(paletteMode);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Save(IDbAcbmImage dbEntry, object model)
        {
            throw new NotImplementedException(nameof(Save));
        }

        protected override object Load(IDbAcbmImage dbEntry)
        {
            var ds = dataSourceProvider.GetDataSource(dbEntry.DataRef);

            if (ds is null)
            {
                throw new Exception($"Unknown DataSourceRef '{dbEntry.DataRef}'.");
            }

            var paletteFormat = ToACBMPaletteMode(dbEntry.PaletteMode);

            return Load(
                ds,
                dbEntry.Width,
                dbEntry.Height,
                dbEntry.BitPlanesNo,
                paletteFormat);
        }

        #endregion Protected Methods

        #region Private Methods

        private object Load(DataSourceBase ds, int width, int height, int bitPlanesNo, ACBMImageReader.ACBMPaletteMode paletteMode)
        {
            //Remember to set source stream to beginning
            ds.Stream.Seek(0, SeekOrigin.Begin);

            var imageBuilder = ImageBuilder.NewImage(bitmapProvider);
            var reader = new ACBMImageReader(imageBuilder, width, height, bitPlanesNo, paletteMode);
            return reader.Read(ds.Stream);
        }

        #endregion Private Methods
    }
}