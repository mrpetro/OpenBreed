using OpenBreed.Core;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Physics.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Teleport
{
    public class TeleportJob : IJob
    {
        #region Private Fields

        private IEntity entity;
        private Vector2 newPosition;
        private bool cancelMovement;

        #endregion Private Fields

        #region Public Constructors

        public TeleportJob(IEntity entity, Vector2 newPosition, bool cancelMovement)
        {
            this.entity = entity;
            this.newPosition = newPosition;
            this.cancelMovement = cancelMovement;
        }

        #endregion Public Constructors

        #region Public Properties

        public Action<IJob> Complete { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Execute()
        {
            var posCmp = entity.Components.OfType<Position>().First();
            posCmp.Value = newPosition;

            if (cancelMovement)
            {
                var velocityCmp = entity.Components.OfType<Velocity>().First();
                velocityCmp.Value = Vector2.Zero;

                var thrustCmp = entity.Components.OfType<Thrust>().First();
                thrustCmp.Value = Vector2.Zero;
            }

            Complete(this);
        }

        public void Update(float dt)
        {
        }

        public void Dispose()
        {
        }

        #endregion Public Methods


    }
}
