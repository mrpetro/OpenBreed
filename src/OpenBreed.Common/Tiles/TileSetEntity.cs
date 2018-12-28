using OpenBreed.Common.Formats;
using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Tiles
{
    public class TileSetEntity : EntityBase
    {
        public DataFormat Format { get; }

        protected readonly TileSetsRepository _repository;

        internal TileSetEntity(TileSetsRepository repository, string name, DataFormat format)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
            Name = name;
            Format = format;
        }

        public TileSetModel GetModel()
        {
            return Format.Load() as TileSetModel;
        }
    }
}
