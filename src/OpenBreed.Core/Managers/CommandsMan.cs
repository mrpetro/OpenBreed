using OpenBreed.Core.Commands;
using OpenBreed.Core.Components;
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

        private Queue<ICommand> messageQueue = new Queue<ICommand>();

        public void ExecuteEnqueued()
        {
            while (messageQueue.Count > 0)
            {
                var cmd = messageQueue.Dequeue();
                Execute(cmd);
            }
        }

        public void LogUnhandled()
        {


            //core.Logging.Warning($"Entity '{cmd.EntityId}' has missing FSM transition from state '{fromStateName}' using impulse '{impulseName}'.");
        }

        private void Execute(ICommand msg)
        {
            if (!handlers.TryGetValue(msg.GetType(), out (MethodInfo Method, object Target) handler))
                return;

            handler.Method.Invoke(handler.Target, new object[] {Core,  msg });
        }

        private bool TryHandle(ICommand msg)
        {
            if (!handlers.TryGetValue(msg.GetType(), out (MethodInfo Method, object Target) handler))
                return false;

            messageQueue.Enqueue(msg);

            //handler.Method.Invoke(handler.Target, new object[] { msg });
            return true;
        }

        public void Post(ICommand msg)
        {
            Debug.Assert(msg != null);

            if (TryHandle(msg))
                return;

            Core.Logging.Warning($"Command '{msg.GetType()}' not registered.");
        }

        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods
    }
}