using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DbActionSetTableVM : DbTableVM
    {
        #region Private Fields

        private readonly IRepository<IActionSetEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DbActionSetTableVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Action sets"; } }

        #endregion Public Properties

    }
}
