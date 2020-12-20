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

        private readonly Dictionary<Type, (MethodInfo Method, object Target)> handlers = new Dictionary<Type, (MethodInfo Method, object Target)>();
        private readonly ICore core;
        private readonly ILogger logger;
        private Queue<ICommand> messageQueue = new Queue<ICommand>();

        #endregion Private Fields

        #region Internal Constructors

        internal CommandsMan(ICore core, ILogger logger)
        {
            this.core = core;
            this.logger = logger;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Register<T>(Func<ICore, T, bool> cmdHandler)
        {
            handlers.Add(typeof(T), (cmdHandler.Method, cmdHandler.Target));
        }

        public void ExecuteEnqueued()
        {
            while (messageQueue.Count > 0)
            {
                var cmd = messageQueue.Dequeue();
                Execute(cmd);
            }
        }

        public void Post(ICommand msg)
        {
            Debug.Assert(msg != null);

            if (TryHandle(msg))
                return;

            logger.Warning($"Command '{msg.GetType()}' not registered.");
        }

        #endregion Public Methods

        #region Private Methods

        private void Execute(ICommand msg)
        {
            if (!handlers.TryGetValue(msg.GetType(), out (MethodInfo Method, object Target) handler))
                return;

            handler.Method.Invoke(handler.Target, new object[] { core, msg });
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