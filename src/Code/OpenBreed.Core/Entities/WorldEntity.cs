using OpenBreed.Core.Entities.Builders;

namespace OpenBreed.Core.Entities
{
    public class WorldEntity : EntityBase, IWorldEntity
    {
        #region Public Constructors

        public WorldEntity(WorldEntityBuilder builder) : base(builder.Core)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public World CurrentWorld { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public virtual void EnterWorld(World world)
        {
            world.AddEntity(this);
        }

        public virtual void LeaveWorld()
        {
            CurrentWorld.RemoveEntity(this);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Deinitialize()
        {
            //Deinitialize all entity components
            for (int i = 0; i < Components.Count; i++)
                Components[i].Deinitialize(this);

            //Forget the world in which entity was
            CurrentWorld = null;
        }

        internal void Initialize(World world)
        {
            //Remember in what world entity is
            CurrentWorld = world;

            //Initialize all entity components
            for (int i = 0; i < Components.Count; i++)
                Components[i].Initialize(this);
        }

        #endregion Internal Methods
    }
}