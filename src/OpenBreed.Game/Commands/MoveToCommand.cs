using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Game.Entities;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Commands
{
    public class MoveToCommand : ICommand
    {
        private AiControl controller;
        private Vector2 position;

        public MoveToCommand(IEntity actor, Vector2 position)
        {
            controller = actor.Components.OfType<AiControl>().FirstOrDefault();

            this.position = position;
        }

        public void Execute()
        {
            controller.Waypoints.Add(position);
        }
    }
}
