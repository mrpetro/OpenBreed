using OpenBreed.Common;
using OpenBreed.Common.Sounds;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DatabaseSoundTableVM : DbTableVM
    {

        #region Private Fields

        private IRepository<ISoundEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseSoundTableVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Sounds"; } }

        #endregion Public Properties

    }
}
