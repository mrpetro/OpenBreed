using OpenBreed.Common;
using OpenBreed.Common.DataSources;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.DataSources;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DbDataSourceTableVM : DbTableVM
    {
        #region Private Fields

        private readonly IRepository<IDataSourceEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DbDataSourceTableVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Data sources"; } }

        #endregion Public Properties
    }
}