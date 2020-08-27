using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Editor.VM.Database.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DbAssetTableVM : DbTableVM
    {

        #region Private Fields

        private readonly IRepository<IAssetEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DbAssetTableVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Assets"; } }

        #endregion Public Properties


    }
}
