using OpenBreed.Wecs;
using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Wecs.Components
{
    public class TurretTargetComponent : IEntityComponent
    {
        #region Public Constructors

        public TurretTargetComponent(int targetEntityId = WecsConsts.NO_ENTITY_ID)
        {
            TargetEntityId = targetEntityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int TargetEntityId { get; set; }

        #endregion Public Properties
    }
}