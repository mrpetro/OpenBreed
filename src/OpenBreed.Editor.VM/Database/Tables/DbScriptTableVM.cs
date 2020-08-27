using OpenBreed.Common;
using OpenBreed.Common.Texts;
using OpenBreed.Database.Interface.Items.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DbScriptTableVM : DbTableVM
    {
        #region Private Fields

        private readonly IRepository<IScriptEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DbScriptTableVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Scripts"; } }

        #endregion Public Properties

    }
}
