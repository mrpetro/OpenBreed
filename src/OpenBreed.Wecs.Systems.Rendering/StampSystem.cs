using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class StampSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
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

        public void Update(float dt)
        {
            foreach (var entity in entities)
            {
                var stampPutterCmp = entity.TryGet<StampPutterComponent>();

                if (stampPutterCmp is null)
                    continue;

                try
                {
                    tileGrid.ModifyTiles(stampPutterCmp.Position, stampPutterCmp.StampId);
                }
                finally
                {
                    entity.Remove<StampPutterComponent>();
                }
            }
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
            //foreach (var item in Entities)
            //{
            //}
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(Entity entity) => entities.Contains(entity);

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods
    }
}