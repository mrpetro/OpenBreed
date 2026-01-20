using OpenBreed.Physics.Interface;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Systems.Projectile
{
    public interface IOnActorTouchObstacleSystem : IActionOnTriggerSystem
    {
        #region Public Methods

        void OnTouch(
            IFixture actorFixture,
            IEntity actorEntity,
            IFixture triggerFixture,
            IEntity triggerEntity,
            Vector2 projection);

        #endregion Public Methods
    }
}