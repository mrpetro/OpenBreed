using OpenBreed.Common;
using OpenBreed.Common.Palettes;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DatabasePaletteTableVM : DbTableVM
    {
        #region Private Fields

        private IRepository<IPaletteEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DatabasePaletteTableVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Palettes"; } }

        #endregion Public Properties

    }
}
