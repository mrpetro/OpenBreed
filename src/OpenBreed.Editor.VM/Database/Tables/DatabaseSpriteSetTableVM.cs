using OpenBreed.Common;
using OpenBreed.Common.Sprites;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DatabaseSpriteSetTableVM : DbTableVM
    {
        #region Private Fields

        private IRepository<ISpriteSetEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseSpriteSetTableVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Sprite sets"; } }

        #endregion Public Properties

    }
}
