using OpenBreed.Wecs;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Wecs.Components
{
    public interface IResurrectableComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        string WorldName { get; }

        #endregion Public Properties
    }

    public class ResurrectCommandComponent : IEntityComponent
    {

    }

    public class ResurrectableComponent : IEntityComponent
    {
        #region Public Constructors

        public ResurrectableComponent(int worldId)
        {
            WorldId = worldId;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// ID of world to which entity should return
        /// </summary>
        public int WorldId { get; set; }

        #endregion Public Properties
    }

    public sealed class ResurrectableComponentFactory : ComponentFactoryBase<IResurrectableComponentTemplate>
    {
        #region Private Fields

        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public ResurrectableComponentFactory(IWorldMan worldMan)
        {
            this.worldMan = worldMan;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IResurrectableComponentTemplate template)
        {
            var worldId = WecsConsts.NO_WORLD_ID;

            var world = worldMan.GetByName(template.WorldName);

            if (world is not null)
                worldId = world.Id;

            return new ResurrectableComponent(worldId);
        }

        #endregion Protected Methods
    }
}