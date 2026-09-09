using OpenBreed.Wecs.Abstractions.Systems;
using System;

namespace OpenBreed.Fsm
{
    public interface IFsmMachine<TContext, TState> : ISystem where TState : Enum
    {
        IFsmMachine<TContext, TState> Register<TImpulse>(TState from, TState to, TransitionHandler<TContext, TState, TImpulse> transitionHandler) where TImpulse : IFsmImpulse;

        bool Process<TImpulse>(TContext context, TImpulse impulse) where TImpulse : IFsmImpulse;
    }
}
