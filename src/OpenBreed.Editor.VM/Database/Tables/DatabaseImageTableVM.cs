using OpenBreed.Common;
using OpenBreed.Common.Images;
using OpenBreed.Editor.VM.Database.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Database.Tables
{
    public class DatabaseImageTableVM : DbTableVM
    {

        #region Private Fields

        private IRepository<IImageEntry> _repository;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseImageTableVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string Name { get { return "Images"; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IRepository repository)
        {
            _repository = repository as IRepository<IImageEntry> ?? throw new InvalidOperationException($"Expected {nameof(IRepository<IImageEntry>)}");
        }

        #endregion Public Methods
    }
}
