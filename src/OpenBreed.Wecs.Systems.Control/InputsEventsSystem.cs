using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Entities;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Control
{
    public abstract class InputsEventSystem<TSystem> : SystemBase<TSystem>, IEventSystem<KeyboardStateEventArgs> where TSystem : ISystem
    {
        #region Protected Fields

        protected readonly HashSet<IEntity> entities = new HashSet<IEntity>();

        #endregion Protected Fields

        #region Private Fields

        private readonly IInputsMan inputsMan;

        #endregion Private Fields

        #region Protected Constructors

        protected InputsEventSystem(IInputsMan inputsMan)
        {
            this.inputsMan = inputsMan;

            inputsMan.KeyboardStateChanged += Update;
        }

        #endregion Protected Constructors

        #region Public Methods

        public override void AddEntity(IEntity entity) => entities.Add(entity);

        public override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        public override void RemoveEntity(IEntity entity) => entities.Remove(entity);

        public void Update(object sender, KeyboardStateEventArgs e)
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