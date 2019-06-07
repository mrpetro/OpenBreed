using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Systems.Control.Components
{
    public interface IAutoController : IControllerComponent
    {
        #region Public Methods

        void Update(float dt);

        #endregion Public Methods
    }
}
