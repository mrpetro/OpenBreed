using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DatabaseAssetTableVM : DbTableVM
    {

        #region Private Fields

        private IRepository<IAssetEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseAssetTableVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Assets"; } }

        #endregion Public Properties


    }
}
