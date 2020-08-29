using OpenBreed.Common;
using OpenBreed.Common.Sprites;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DbSpriteSetTableVM : DbTableVM
    {
        #region Private Fields

        private readonly IRepository<ISpriteSetEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DbSpriteSetTableVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Sprite sets"; } }

        #endregion Public Properties

    }
}
