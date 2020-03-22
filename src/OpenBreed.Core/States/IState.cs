using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core.States
{
    public interface IState
    {
        #region Public Properties

        IEntity Entity { get; }

        #endregion Public Properties

        #region Public Methods

        void Initialize(IEntity entity);

        void EnterState();

        void LeaveState();


        #endregion Public Methods
    }

    public interface IState<T> : IState where T : struct, IConvertible
    {
        T Id { get; }
        T Process(string actionName, object[] arguments);
    }

}