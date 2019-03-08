using OpenBreed.Common;
using OpenBreed.Common.Tiles;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DbTileSetTableVM : DbTableVM
    {
        #region Private Fields

        private IRepository<ITileSetEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DbTileSetTableVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Tile sets"; } }

        #endregion Public Properties

    }
}
