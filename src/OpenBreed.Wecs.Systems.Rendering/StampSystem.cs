using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Rendering
{
    [RequireEntityWith(typeof(StampPutterComponent))]
    public class StampSystem : UpdatableSystemBase<StampSystem>
    {
        #region Private Fields

        private ITileGrid tileGrid;

        #endregion Private Fields

        #region Public Constructors

        public StampSystem(IWorld world) :
            base(world)
        {
            tileGrid = world.GetModule<ITileGrid>();
        }

        #endregion Public Constructors

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var stampPutterCmp = entity.TryGet<StampPutterComponent>();

            if (stampPutterCmp is null)
                return;

            try
            {
                tileGrid.ModifyTiles(stampPutterCmp.Position, stampPutterCmp.StampId);
            }
            finally
            {
                entity.Remove<StampPutterComponent>();
            }
        }

        #endregion Protected Methods
    }
}