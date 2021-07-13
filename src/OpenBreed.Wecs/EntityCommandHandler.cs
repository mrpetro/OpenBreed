using OpenBreed.Core.Commands;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs
{
    public class EntityCommandHandler : ICommandHandler<IEntityCommand>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;
        private readonly ICommandsMan commandsMan;
        private readonly IEventsMan eventsMan;
        private readonly Dictionary<Type, Type> commandsToSystems = new Dictionary<Type, Type>();

        private Dictionary<int, List<IEntityCommand>> awaitingCommands = new Dictionary<int, List<IEntityCommand>>();

        #endregion Private Fields

        #region Public Constructors

        public EntityCommandHandler(IEntityMan entityMan, IWorldMan worldMan, ICommandsMan commandsMan, IEventsMan eventsMan)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
            this.commandsMan = commandsMan;
            this.eventsMan = eventsMan;

            commandsMan.RegisterHandler(this);

            eventsMan.Subscribe<EntityAddedEventArgs>(worldMan, OnEntityAdded);
        }

        #endregion Public Constructors

        #region Public Methods

        public void BindCommand<TCommand, TSystem>() where TCommand : IEntityCommand where TSystem : ISystem
        {
            commandsMan.RegisterCommand<TCommand>(this);

            commandsToSystems.Add(typeof(TCommand), typeof(TSystem));
        }

        public void Handle(ICommand command) => Handle((IEntityCommand)command);

        public bool Handle(IEntityCommand command)
        {
            var entity = entityMan.GetById(command.EntityId);

            //Commands for entities that are in limbo are ignored
            if (entity.WorldId == -1)
            {
                List<IEntityCommand> entityCommands;

                if (!awaitingCommands.TryGetValue(command.EntityId, out entityCommands))
                {
                    entityCommands = new List<IEntityCommand>();
                    awaitingCommands.Add(command.EntityId, entityCommands);
                }

                entityCommands.Add(command);

                return true;
            }

            return ExecuteCommand(entity.WorldId, command);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnEntityAdded(object sender, EntityAddedEventArgs a)
        {
            if (!awaitingCommands.TryGetValue(a.EntityId, out List<IEntityCommand> commands))
                return;

            for (int i = 0; i < commands.Count; i++)
                ExecuteCommand(a.WorldId, commands[i]);

            awaitingCommands.Remove(a.EntityId);
        }

        private bool ExecuteCommand(int worldId, IEntityCommand entityCommand)
        {
            var world = worldMan.GetById(worldId);

            var systemType = commandsToSystems[entityCommand.GetType()];

            var system = world.Systems.FirstOrDefault(item => item.GetType() == systemType);

            return system.EnqueueCommand(entityCommand);
        }

        #endregion Private Methods
    }
}