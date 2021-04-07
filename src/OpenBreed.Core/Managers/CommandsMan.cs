using OpenBreed.Common.Logging;
using OpenBreed.Core.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace OpenBreed.Core.Managers
{
    internal class CommandsMan : ICommandsMan
    {
        #region Private Fields

        private readonly Dictionary<Type, ICommandHandler> commandHandlers = new Dictionary<Type, ICommandHandler>();
        private readonly Dictionary<Type, ICommandHandler> handlersEx = new Dictionary<Type, ICommandHandler>();

        private readonly Dictionary<Type, (MethodInfo Method, object Target)> handlers = new Dictionary<Type, (MethodInfo Method, object Target)>();
        private readonly ILogger logger;
        private Queue<ICommand> messageQueue = new Queue<ICommand>();

        #endregion Private Fields

        #region Internal Constructors

        internal CommandsMan(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void RegisterHandler<TCommand>(ICommandHandler<TCommand> commandHandler) where TCommand : ICommand
        {
            var commandType = typeof(TCommand);

            if (commandHandlers.ContainsKey(commandType))
                throw new InvalidOperationException($"Handler for command types '{commandType}' already registered.");

            commandHandlers.Add(commandType, commandHandler);
        }

        public void RegisterCommand<TCommand>(ICommandHandler cmdHandler) where TCommand : ICommand
        {
            handlersEx.Add(typeof(TCommand), cmdHandler);
        }

        public void Register<T>(Func<ICore, T, bool> cmdHandler)
        {
            handlers.Add(typeof(T), (cmdHandler.Method, cmdHandler.Target));
        }

        public void ExecuteEnqueued(ICore core)
        {
            while (messageQueue.Count > 0)
            {
                var cmd = messageQueue.Dequeue();
                Execute(core, cmd);
            }
        }

        public void Post(ICommand msg)
        {
            Debug.Assert(msg != null);

            if (TryHandleEx(msg))
                return;

            if (TryHandle(msg))
                return;

            logger.Warning($"Command '{msg.GetType()}' not registered.");
        }

        #endregion Public Methods

        #region Private Methods

        private void Execute(ICore core, ICommand msg)
        {
            if (!handlers.TryGetValue(msg.GetType(), out (MethodInfo Method, object Target) handler))
                return;

            handler.Method.Invoke(handler.Target, new object[] { core, msg });
        }

        private bool TryHandleEx(ICommand msg)
        {
            if (handlersEx.TryGetValue(msg.GetType(), out ICommandHandler commandHandler))
            {
                commandHandler.Handle(msg);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool TryHandle(ICommand msg)
        {



            if (!handlers.TryGetValue(msg.GetType(), out (MethodInfo Method, object Target) handler))
                return false;

            messageQueue.Enqueue(msg);

            //handler.Method.Invoke(handler.Target, new object[] { msg });
            return true;
        }

        #endregion Private Methods
    }
}