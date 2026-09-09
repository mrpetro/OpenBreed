using OpenBreed.Wecs.Abstractions.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Fsm
{
    public interface IFsmImpulse
    {

    }

    interface ITransition<TContext, TState> where TState : Enum
    {
        TState From { get; }

        void Execute<TImpulse>(
            TContext context,
            TImpulse impulse)
            where TImpulse : IFsmImpulse;
    }

    sealed class Transition<TContext, TState, TImpulse> : ITransition<TContext, TState>
        where TState : Enum where TImpulse : IFsmImpulse
    {
        private readonly TransitionHandler<TContext, TState, TImpulse> _handler;
        private readonly Action<TContext, TState> stateSetter;

        public TState From { get; }
        public TState To { get; }

        public Transition(
            TState from,
            TState to,
            TransitionHandler<TContext, TState, TImpulse> handler,
            Action<TContext, TState> stateSetter)
        {
            From = from;
            To = to;
            _handler = handler;
            this.stateSetter = stateSetter;
        }

        public void Execute<T>(
            TContext context,
            T impulse)
            where T : IFsmImpulse
        {
            var typedImpulse = (TImpulse)(object)impulse;

            stateSetter.Invoke(context, To);

            // Then notify the transition handler.
            _handler(context, typedImpulse, From, To);
        }
    }

    public delegate void TransitionHandler<TContext, TState, TFsmImpulse>(TContext context, TFsmImpulse impulse, TState from, TState to) where TState : Enum where TFsmImpulse : IFsmImpulse;

    public interface IFsmMan
    {
        IStateMachine GetById(int id);
        IStateMachine GetByName(string name);
        string GetStateName(int fsmId, int stateId);
        string GetStateName(MachineState state);
        IEnumerable<string> GetStateNames(IEntity entity);
        IStateMachine<TState, TImpulse> Create<TState, TImpulse>(string name) where TState : Enum where TImpulse : Enum;
        void EnterState(IEntity entity, MachineState state, int withImpulseId);
        void LeaveState(IEntity entity, MachineState state, int withImpulseId);
    }
}
