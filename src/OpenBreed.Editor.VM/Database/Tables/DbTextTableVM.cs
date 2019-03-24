using OpenBreed.Common;
using OpenBreed.Common.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DbTextTableVM : DbTableVM
    {
        #region Private Fields

        private readonly IRepository<ITextEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DbTextTableVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Texts"; } }

        #endregion Public Properties

    }
}
