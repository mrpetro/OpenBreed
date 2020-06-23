using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace OpenBreed.Core.Managers
{
    public class CommandsMan
    {
        #region Public Constructors

        public CommandsMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors
        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        private readonly Dictionary<Type, (MethodInfo Method, object Target)> handlers = new Dictionary<Type, (MethodInfo Method, object Target)>();

        public void Register<T>(Func<ICore, T, bool> cmdHandler)
        {
            handlers.Add(typeof(T), (cmdHandler.Method, cmdHandler.Target));
        }

        private Queue<IMsg> messageQueue = new Queue<IMsg>();

        public void ExecuteEnqueued()
        {
            while (messageQueue.Count > 0)
            {
                var cmd = messageQueue.Dequeue();
                Execute(cmd);
            }
        }

        private void Execute(IMsg msg)
        {
            if (!handlers.TryGetValue(msg.GetType(), out (MethodInfo Method, object Target) handler))
                return;

            handler.Method.Invoke(handler.Target, new object[] {Core,  msg });
        }

        private bool TryHandle(IMsg msg)
        {
            if (!handlers.TryGetValue(msg.GetType(), out (MethodInfo Method, object Target) handler))
                return false;

            messageQueue.Enqueue(msg);

            //handler.Method.Invoke(handler.Target, new object[] { msg });
            return true;
        }

        public void Post(IMsg msg)
        {
            Debug.Assert(msg != null);

            if (TryHandle(msg))
                return;

            if (msg is IEntityCommand)
            {
                Post((IEntityCommand)msg);
                return;
            }
            else if (msg is IWorldCommand)
            {
                Post((IWorldCommand)msg);
                return;
            }
            else
                Core.Logging.Warning($"Command '{msg.GetType()}' not registered.");
        }

        #endregion Public Methods

        #region Private Methods

        private void Post(IEntityCommand msg)
        {
            Core.Worlds.HandleCmd(msg);
        }

        private void Post(IWorldCommand msg)
        {
            Core.Worlds.HandleCmd(msg);
        }

        #endregion Private Methods
    }
}