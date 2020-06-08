using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Systems
{
    /// <summary>
    /// System that controls entity commands of adding and removing from world.
    /// </summary>
    public class WorldBouncerSystem : WorldSystem, ICommandExecutor
    {
        private readonly List<int> entities = new List<int>();

        private readonly CommandHandler cmdHandler;

        public WorldBouncerSystem(ICore core) : base(core)
        {
            cmdHandler = new CommandHandler(this);
        }

        protected override void OnAddEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override void OnRemoveEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
