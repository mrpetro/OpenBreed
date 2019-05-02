using OpenBreed.Game.Common;
using OpenBreed.Game.Movement.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Movement
{
    public class MovementSystem : WorldSystem<IMovementComponent>
    {
        private List<IMovementComponent> components;

        public MovementSystem()
        {
            components = new List<IMovementComponent>();
        }

        protected override void AddComponent(IMovementComponent component)
        {
            components.Add(component);
        }

        protected override void RemoveComponent(IMovementComponent component)
        {
            components.Remove(component);
        }

        public override void Update(float dt)
        {
            for (int i = 0; i < components.Count; i++)
                components[i].Update(dt);
        }
    }
}
