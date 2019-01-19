using OpenBreed.Common;
using OpenBreed.Common.Maps;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DatabaseLevelTableVM : DbTableVM
    {
        #region Private Fields

        private IRepository<ILevelEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseLevelTableVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Levels"; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IRepository repository)
        {
            _repository = repository as IRepository<ILevelEntry> ?? throw new InvalidOperationException($"Expected {nameof(IRepository<ILevelEntry>)}");
        }

        #endregion Public Methods
    }
}
