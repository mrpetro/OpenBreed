using OpenBreed.Input.Abstractions;
using OpenBreed.Wecs.Abstractions.Events;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Control.Systems
{
    public abstract class InputsEventSystem : IEventSystem<WorldKeyboardEvent>
    {
        #region Private Fields

        private readonly IWorldMan worldMan;
        private readonly IInputsMan inputsMan;

        #endregion Private Fields

        #region Protected Constructors

        protected InputsEventSystem(IWorldMan worldMan, IInputsMan inputsMan)
        {
            this.worldMan = worldMan;
            this.inputsMan = inputsMan;
        }

        #endregion Protected Constructors

        #region Public Methods

        public void OnEvent(WorldKeyboardEvent e)
        {
            var world = this.worldMan.GetById(e.WorldId);

            var entities = world.GetMatchingEntities(this);

            foreach (var entity in entities)
            {
                UpdateEntity(entity, e);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void UpdateEntity(IEntity entity, WorldKeyboardEvent e);

        #endregion Protected Methods
    }
}