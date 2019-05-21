using OpenBreed.Core.Entities;
using OpenBreed.Game.Components;
using OpenBreed.Game.Entities.Builders;
using OpenTK;
using System.Linq;

namespace OpenBreed.Game.Entities
{
    public class WorldActor : WorldEntity
    {
        #region Internal Constructors

        internal WorldActor(WorldActorBuilder builder) : base(builder)
        {
            Components.Add(builder.position);
            Components.Add(builder.direction);

            if(builder.animator != null)
                Components.Add(builder.animator);

            if (builder.movement != null)
                Components.Add(builder.movement);

            if (builder.sprite != null)
                Components.Add(builder.sprite);

            if (builder.shape != null)
                Components.Add(builder.shape);

            if (builder.body != null)
                Components.Add(builder.body);

            if (builder.controller != null)
                Components.Add(builder.controller);
        }

        public void MoveTo(Vector2 position)
        {
            var aiController = Components.OfType<AIController>().FirstOrDefault();

            aiController.SetWaypoint(position);
        }

        #endregion Internal Constructors
    }
}