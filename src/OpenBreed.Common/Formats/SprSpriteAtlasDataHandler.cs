using OpenBreed.Common.Data;
using OpenBreed.Common.DataSources;
using OpenBreed.Common.Extensions;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Readers.Images.IFF;
using OpenBreed.Common.Tools.Collections;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Model;
using OpenBreed.Model.Images;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Sounds;
using OpenBreed.Model.Sprites;
using OpenBreed.Model.Tiles;
using OpenBreed.Reader.Legacy.Images.ACBM;
using OpenBreed.Reader.Legacy.Maps.MAP;
using OpenBreed.Reader.Legacy.Palettes;
using OpenBreed.Reader.Legacy.Sprites.SPR;
using OpenBreed.Reader.Legacy.Tiles.BLK;
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
    public class SprSpriteAtlasDataHandler : AssetDataHandlerBase<IDbSpriteAtlasFromSpr>
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly IBitmapProvider bitmapProvider;
        private readonly IDrawingFactory drawingFactory;
        private readonly DataSourceProvider dataSourceProvider;

        #endregion Private Fields

        #region Public Constructors

        public SprSpriteAtlasDataHandler(
            IRepositoryProvider repositoryProvider,
            IBitmapProvider bitmapProvider,
            IDrawingFactory drawingFactory,
            DataSourceProvider dataSourceProvider)
        {
            this.repositoryProvider = repositoryProvider;
            this.bitmapProvider = bitmapProvider;
            this.drawingFactory = drawingFactory;
            this.dataSourceProvider = dataSourceProvider;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void Save(IDbSpriteAtlasFromSpr dbEntry, object model)
        {
            throw new NotImplementedException();
        }

        protected override object Load(IDbSpriteAtlasFromSpr dbEntry)
        {
            var ds = dataSourceProvider.GetDataSource(dbEntry.DataRef);

            if (ds is null)
            {
                throw new Exception($"Unknown DataSourceRef '{dbEntry.DataRef}'.");
            }

            return Load(ds, dbEntry.Sizes);
        }

        #endregion Protected Methods

        #region Private Methods

        private object Load(DataSourceBase ds, IList<IDbPoint2i> sizes)
        {
            ds.Stream.Seek(0, SeekOrigin.Begin);

            var spriteSetBuilder = SpriteSetBuilder.NewSpriteSet();

            if (sizes.Any())
            {
                var oddSprReader = new ODDSPRReader(spriteSetBuilder, sizes.Select(item => (item.X, item.Y)).ToArray());
                return oddSprReader.Read(ds.Stream);
            }

            var sprReader = new SPRReader(spriteSetBuilder);
            return sprReader.Read(ds.Stream);
        }

        #endregion Private Methods
    }
}