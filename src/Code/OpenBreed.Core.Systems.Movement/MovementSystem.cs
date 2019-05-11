using OpenBreed.Core.Systems.Movement.Components;
using System.Collections.Generic;

namespace OpenBreed.Core.Systems.Movement
{
    public class MovementSystem : WorldSystem<IMovementComponent>, IMovementSystem
    {
        #region Private Fields

        private List<IMovementComponent> components;

        #endregion Private Fields

        #region Public Constructors

        public MovementSystem()
        {
            components = new List<IMovementComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Update(float dt)
        {
            for (int i = 0; i < components.Count; i++)
                components[i].Update(dt);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void AddComponent(IMovementComponent component)
        {
            components.Add(component);
        }

        protected override void RemoveComponent(IMovementComponent component)
        {
            components.Remove(component);
        }

        #endregion Protected Methods
    }
}