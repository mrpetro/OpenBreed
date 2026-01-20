using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Abstractions.Systems
{
    public interface IOnWorldUpdateActionSystem : IActionOnTriggerSystem
    {
        #region Public Methods

        void OnUpdate(IEntity entity, IWorld world);

        #endregion Public Methods
    }
}