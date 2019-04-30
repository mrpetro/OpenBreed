using OpenBreed.Game.Entities.Builders;
using System;

namespace OpenBreed.Game.Entities
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
            if (CurrentWorld != null)
                throw new InvalidOperationException("This entity can't exist in more than one world.");

            world.RegisterEntity(this);
            CurrentWorld = world;
        }

        public virtual void LeaveWorld()
        {
            if (CurrentWorld == null)
                throw new InvalidOperationException("This entity doesn't exist in any world.");

            CurrentWorld.UnregisterEntity(this);
            CurrentWorld = null;
        }

        internal void Initialize()
        {
            for (int i = 0 ; i < Components.Count; i++)
                Components[i].Initialize(this);
        }

        internal void Deinitialize()
        {
            for (int i = 0; i < Components.Count; i++)
                Components[i].Deinitialize(this);
        }

        #endregion Public Methods
    }
}