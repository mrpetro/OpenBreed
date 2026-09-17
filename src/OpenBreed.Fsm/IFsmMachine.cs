using OpenBreed.Wecs.Abstractions.Systems;
using System;

namespace OpenBreed.Fsm
{
    public record CompletionImpulse() : IFsmImpulse;

    public static class FsmMachineExtensions
    {
        public static bool Complete<TContext, TState>(this IFsmMachine<TContext, TState> machine, TContext context) where TState : Enum
        {
            return machine.Process(context, new CompletionImpulse());
        }
    }

    public interface IFsmMachine<TContext, TState> : ISystem where TState : Enum
    {
        IFsmMachine<TContext, TState> Register<TImpulse>(TState from, TransitionHandler<TContext, TState, TImpulse> transitionHandler) where TImpulse : IFsmImpulse;

        bool Process<TImpulse>(TContext context, TImpulse impulse) where TImpulse : IFsmImpulse;
    }
}
