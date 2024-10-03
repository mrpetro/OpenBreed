using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Database.EFCore.DbEntries
{
    public enum TileAtlasTypeEnum
    {
        DbTileAtlasFromImage = 0,
        DbTileAtlasFromBlk = 1,
    }

    public class TileAtlasType
    {
        #region Public Properties

        public TileAtlasTypeEnum Id { get; set; }
        public string Name { get; set; }

        #endregion Public Properties
    }

    public class DbTileAtlas : DbEntry
    {
        #region Public Constructors

        public DbTileAtlas()
        { }

        #endregion Public Constructors

        #region Protected Constructors

        protected DbTileAtlas(DbTileAtlas other)
            : base(other)
        {
        }

        #endregion Protected Constructors

        #region Public Properties

        public TileAtlasTypeEnum TypeId { get; set; }
        public TileAtlasType Type { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }

    public class DbTileAtlasFromImage : DbTileAtlas, IDbTileAtlasFromBlk
    {
        #region Public Constructors

        public DbTileAtlasFromImage()
        {
            TypeId = TileAtlasTypeEnum.DbTileAtlasFromImage;
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected DbTileAtlasFromImage(DbTileAtlasFromImage other)
            : base(other)
        {
            TypeId = TileAtlasTypeEnum.DbTileAtlasFromBlk;
            PaletteRefs = other.PaletteRefs.ToList();
        }

        #endregion Protected Constructors

        #region Public Properties

        public string DataRef { get; set; }

        public List<string> PaletteRefs { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy()
        {
            return new DbTileAtlasFromImage(this);
        }

        #endregion Public Methods
    }

    public class DbTileAtlasFromBlk : DbTileAtlas, IDbTileAtlasFromBlk
    {
        #region Public Constructors

        public DbTileAtlasFromBlk()
        {
            TypeId = TileAtlasTypeEnum.DbTileAtlasFromBlk;
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected DbTileAtlasFromBlk(DbTileAtlasFromBlk other)
            : base(other)
        {
            TypeId = TileAtlasTypeEnum.DbTileAtlasFromBlk;
            PaletteRefs = other.PaletteRefs.ToList();
        }

        #endregion Protected Constructors

        #region Public Properties

        public string DataRef { get; set; }

        public List<string> PaletteRefs { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy()
        {
            return new DbTileAtlasFromBlk(this);
        }

        #endregion Public Methods
    }
}