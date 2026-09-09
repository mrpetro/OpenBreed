using OpenBreed.Core.Abstractions.Managers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenBreed.Fsm
{
    public class FsmMachine<TContext, TState> : IFsmMachine<TContext, TState> where TState : Enum
    {
        private readonly record struct QueuedImpulse(
            TContext Context,
            IFsmImpulse Impulse);

        #region Private Fields

        private readonly Func<TContext, TState> stateGetter;
        private readonly Action<TContext, TState> stateSetter;

        private readonly Dictionary<Type, Dictionary<TState, ITransition<TContext, TState>>> _transitions = new();

        private readonly Queue<QueuedImpulse> queue = new();

        private bool processing = false;

        #endregion Private Fields

        #region Public Constructors

        public FsmMachine(Func<TContext, TState> stateGetter, Action<TContext, TState> stateSetter)
        {
            this.stateGetter = stateGetter ?? throw new ArgumentNullException(nameof(stateGetter));
            this.stateSetter = stateSetter ?? throw new ArgumentNullException(nameof(stateSetter));
        }

        #endregion Public Constructors

        #region Public Methods

        public IFsmMachine<TContext, TState> Register<TImpulse>(TState from, TState to, TransitionHandler<TContext, TState, TImpulse> transitionHandler) where TImpulse : IFsmImpulse
        {
            var type = typeof(TImpulse);

            if (!_transitions.TryGetValue(type, out var transitions))
            {
                transitions = new Dictionary<TState, ITransition<TContext, TState>>();
                _transitions.Add(type, transitions);
            }

            if (!transitions.TryAdd(from, new Transition<TContext, TState, TImpulse>(from, to, transitionHandler, stateSetter)))
            {
                throw new InvalidOperationException();
            }

            return this;
        }

        public bool Process<TImpulse>(TContext context, TImpulse impulse) where TImpulse : IFsmImpulse
        {
            queue.Enqueue(new QueuedImpulse(context, impulse));

            if (processing)
                return true;

            processing = true;

            try
            {
                while (queue.TryDequeue(out var item))
                {
                    ProcessSingle(item.Context, item.Impulse);
                }
            }
            finally
            {
                processing = false;
            }

            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private bool ProcessSingle(
            TContext context,
            IFsmImpulse impulse)
        {
            var state = stateGetter.Invoke(context);

            if (!_transitions.TryGetValue(
                    impulse.GetType(),
                    out var transitions))
            {
                return false;
            }

            if (!transitions.TryGetValue(state, out var transition))
            {
                return false;
            }

            transition.Execute(context, impulse);

            return true;
        }

        #endregion Private Methods
    }
}