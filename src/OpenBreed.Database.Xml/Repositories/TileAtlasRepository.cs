using OpenBreed.Database.EFCore;
using OpenBreed.Database.EFCore.DbEntries;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Xml.Repositories
{
    public class TileAtlasRepository : IRepository<IDbTileAtlas>
    {
        private OpenBreedDbContext dataEx;

        public TileAtlasRepository(OpenBreedDbContext dataEx, Tables.XmlDbTileAtlasTableDef tableDef)
        {
            this.dataEx = dataEx;

            if(!dataEx.TileAtlases.Any())
                MigrateTable(tableDef);
        }


        private void MigrateTable(Tables.XmlDbTileAtlasTableDef tableDef)
        {
            foreach (var item in tableDef.Items)
                Add(item);
 
            dataEx.SaveChanges();
        }

        public IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(IDbTileAtlasFromBlk);
                yield return typeof(IDbTileAtlasFromImage);
            }
        }

        public IEnumerable<IDbEntry> Entries => dataEx.DataSources;

        public string Name => "Tile Atlases";

        public void Add(IDbTileAtlas entry)
        {
            if (entry is DbTileAtlasFromImage tileAtlasFromImage)
            {
                dataEx.Add(new DbTileAtlasFromImage()
                {
                    Id = tileAtlasFromImage.Id,
                    Description = tileAtlasFromImage.Description,
                    DataRef = tileAtlasFromImage.DataRef,
                    PaletteRefs = tileAtlasFromImage.PaletteRefs.ToList(),
                }); ; ;
            }
            else if (entry is IDbTileAtlasFromBlk tileAtlasFromBlk)
            {
                dataEx.Add(new DbTileAtlasFromBlk()
                {
                    Id = tileAtlasFromBlk.Id,
                    Description = tileAtlasFromBlk.Description,
                    DataRef = tileAtlasFromBlk.DataRef,
                    PaletteRefs = tileAtlasFromBlk.PaletteRefs.ToList(),
                });
            }
        }

        public IDbEntry Find(string name)
        {
            return GetById(name);
        }

        public IDbTileAtlas GetById(string id)
        {
            var found = dataEx.TileAtlases.Find(id);

            if (found is null)
                return null;

            switch (found.TypeId)
            {
                case TileAtlasTypeEnum.DbTileAtlasFromImage:
                    return dataEx.TileAtlasesFromImages.Find(id);
                case TileAtlasTypeEnum.DbTileAtlasFromBlk:
                    return dataEx.TileAtlasesFromBlk.Find(id);
                default:
                    throw new NotImplementedException($"Unknown id = {id}");
            }
        }

        private IDbTileAtlas GetByIdAndType(string id, TileAtlasTypeEnum type)
        {
            switch (type)
            {
                case TileAtlasTypeEnum.DbTileAtlasFromImage:
                    return dataEx.TileAtlasesFromImages.Find(id);
                case TileAtlasTypeEnum.DbTileAtlasFromBlk:
                    return dataEx.TileAtlasesFromBlk.Find(id);
                default:
                    throw new NotImplementedException($"Unknown id = {id}");
            }
        }

        public IDbTileAtlas GetNextTo(IDbTileAtlas entry)
        {
            bool found = false;

            foreach (var item in dataEx.TileAtlases)
            {
                if (found)
                    return GetByIdAndType(item.Id, item.TypeId);

                found = (item.Id == entry.Id);
            }

            return null;
        }

        public IDbTileAtlas GetPreviousTo(IDbTileAtlas entry)
        {
            DbTileAtlas previous = null;

            foreach (var item in dataEx.TileAtlases)
            {
                if (item.Id == entry.Id)
                    break;

                previous = item;
            }

            if (previous is null)
                return null;

            return GetByIdAndType(previous.Id, previous.TypeId);
        }

        public IDbEntry New(string newId, Type entryType = null)
        {
            throw new NotImplementedException();
        }

        public void Remove(IDbTileAtlas entry)
        {
            throw new NotImplementedException();
        }

        public void Update(IDbTileAtlas entry)
        {
            throw new NotImplementedException();
        }
    }
}
