using OpenBreed.Core.Commands;
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
        private WorldActor actor;
        private Vector2 position;

        public MoveToCommand(WorldActor actor, Vector2 position)
        {
            this.actor = actor;
            this.position = position;
        }

        public void Execute()
        {
            actor.MoveTo(position);
        }
    }
}
