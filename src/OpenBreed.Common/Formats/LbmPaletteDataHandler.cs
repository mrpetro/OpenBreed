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
    public class LbmPaletteDataHandler : AssetDataHandlerBase<IDbPaletteFromLbm>
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly IBitmapProvider bitmapProvider;
        private readonly IDrawingFactory drawingFactory;
        private readonly Lazy<ImagesDataProvider> lazyImageDataProvider;

        #endregion Private Fields

        #region Public Constructors

        public LbmPaletteDataHandler(
            IRepositoryProvider repositoryProvider,
            IBitmapProvider bitmapProvider,
            IDrawingFactory drawingFactory,
            Lazy<ImagesDataProvider> lazyImageDataProvider)
        {
            this.repositoryProvider = repositoryProvider;
            this.bitmapProvider = bitmapProvider;
            this.drawingFactory = drawingFactory;
            this.lazyImageDataProvider = lazyImageDataProvider;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void Save(IDbPaletteFromLbm dbEntry, object model)
        {
            throw new NotImplementedException();
        }

        protected override object Load(IDbPaletteFromLbm dbEntry)
        {
            var imageEntry = repositoryProvider.GetRepository<IDbImage>().GetById(dbEntry.ImageRef);

            if (imageEntry is null)
            {
                throw new Exception($"Unknown ImageRef '{dbEntry.ImageRef}'.");
            }

            var image = lazyImageDataProvider.Value.GetImage(imageEntry);

            if (image is null)
            {
                throw new Exception($"Image problem '{imageEntry.Id}'.");
            }

            return PalettesDataHelper.Create(image.Palette);
        }

        #endregion Protected Methods
    }
}