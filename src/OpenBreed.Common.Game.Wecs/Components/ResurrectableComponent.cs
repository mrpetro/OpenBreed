using OpenBreed.Wecs.Abstractions;
using OpenBreed.Wecs.Components;

namespace OpenBreed.Common.Game.Wecs.Components
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
}