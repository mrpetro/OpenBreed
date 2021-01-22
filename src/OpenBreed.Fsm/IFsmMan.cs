using OpenBreed.Ecsw.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Fsm
{
    public interface IFsmMan
    {
        IStateMachine GetById(int id);
        IStateMachine GetByName(string name);
        string GetStateName(int fsmId, int stateId);
        string GetStateName(MachineState state);
        IEnumerable<string> GetStateNames(Entity entity);
        IStateMachine<TState, TImpulse> Create<TState, TImpulse>(string name) where TState : Enum where TImpulse : Enum;
        void EnterState(Entity entity, MachineState state, int withImpulseId);
        void LeaveState(Entity entity, MachineState state, int withImpulseId);
    }
}
