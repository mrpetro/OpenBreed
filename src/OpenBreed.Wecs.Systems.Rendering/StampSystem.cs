using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class StampSystem : UpdatableSystemBase
    {
        #region Private Fields

        private ITileGrid tileGrid;

        #endregion Private Fields

        #region Public Constructors

        public StampSystem()
        {
            RequireEntityWith<StampPutterComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            tileGrid = world.GetModule<ITileGrid>();
        }

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