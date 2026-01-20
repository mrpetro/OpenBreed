using OpenBreed.Physics.Interface;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Systems.Projectile
{
    public interface IOnProjectileHitObstacleSystem : IActionOnTriggerSystem
    {
        #region Public Methods

        void OnHit(
            IFixture projectileFixture,
            IEntity projectileEntity,
            IFixture obstacleFixture,
            IEntity obstacleEntity,
            Vector2 projection);

        #endregion Public Methods
    }
}