using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Abstractions.Services
{
    public interface IWecsCore
    {
        #region Public Properties

        IWorldMan Worlds { get; }
        IEntityMan Entities { get; }
        IComponentsMan Components { get; }
        IEntityClassMan Classes { get; }

        #endregion Public Properties

        #region Public Methods

        void Update(float dt);

        #endregion Public Methods
    }
}