using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.Tiles;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.EFCore.DbEntries
{
    public enum TileAtlasTypeEnum
    {
        DbTileAtlasFromImage = 0,
        DbTileAtlasFromBlk = 1,
    }

    public class TileAtlasType
    {
        public TileAtlasTypeEnum Id { get; set; }
        public string Name { get; set; }
    }

    public class DbTileAtlas : DbEntry
    {
        public DbTileAtlas()
        { }

        protected DbTileAtlas(DbTileAtlas other)
            : base(other)
        {
        }

        public TileAtlasTypeEnum TypeId { get; set; }
        public TileAtlasType Type { get; set; }

        public override IDbEntry Copy()
        {
            throw new NotImplementedException();
        }
    }

    public class DbTileAtlasFromImage : DbTileAtlas, IDbTileAtlasFromImage
    {
        public DbTileAtlasFromImage()
        {
            TypeId = TileAtlasTypeEnum.DbTileAtlasFromImage;
        }

        protected DbTileAtlasFromImage(DbTileAtlasFromImage other)
            : base(other)
        {
            TypeId = TileAtlasTypeEnum.DbTileAtlasFromBlk;
        }

        public string DataRef { get; set; }

        public List<string> PaletteRefs { get; set; }

        public override IDbEntry Copy()
        {
            return new DbTileAtlasFromImage(this);
        }
    }

    public class DbTileAtlasFromBlk : DbTileAtlas, IDbTileAtlasFromBlk
    {
        public DbTileAtlasFromBlk()
        {
            TypeId = TileAtlasTypeEnum.DbTileAtlasFromBlk;
        }

        protected DbTileAtlasFromBlk(DbTileAtlasFromBlk other)
            : base(other)
        {
            TypeId = TileAtlasTypeEnum.DbTileAtlasFromBlk;
        }

        public string DataRef { get; set; }

        public List<string> PaletteRefs { get; set; }

        public override IDbEntry Copy()
        {
            return new DbTileAtlasFromBlk(this);
        }
    }
}