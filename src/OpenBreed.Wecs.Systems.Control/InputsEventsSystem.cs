using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Entities;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Control
{
    public abstract class InputsEventSystem<TSystem> : MatchingSystemBase<TSystem>, IEventSystem<KeyboardStateEventArgs> where TSystem : IMatchingSystem
    {
        #region Private Fields

        private readonly IInputsMan inputsMan;

        #endregion Private Fields

        #region Protected Constructors

        protected InputsEventSystem(IInputsMan inputsMan)
        {
            this.inputsMan = inputsMan;

            inputsMan.KeyboardStateChanged += (s,a) => Update(a);
        }

        #endregion Protected Constructors

        #region Public Methods

        public void Update(KeyboardStateEventArgs e)
        {
            foreach (var entity in entities)
            {
                UpdateEntity(entity, e);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void UpdateEntity(IEntity entity, KeyboardStateEventArgs e);

        #endregion Protected Methods
    }
}