using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Control;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Core.Systems.Control.Systems;
using OpenTK;
using OpenTK.Input;
using System;
using System.Linq;

namespace OpenBreed.Core.Systems.Control.Components
{
    public class KeyboardControl : IEntityComponent
    {
        #region Private Fields

        private StateMachine stateMachine;

        #endregion Private Fields

        #region Public Constructors

        public KeyboardControl(Key moveUpKey, Key moveDownKey, Key moveLeftKey, Key moveRightKey)
        {
            MoveUpKey = moveUpKey;
            MoveDownKey = moveDownKey;
            MoveLeftKey = moveLeftKey;
            MoveRightKey = moveRightKey;
        }

        #endregion Public Constructors

        #region Public Properties

        public Key MoveUpKey { get; set; }
        public Key MoveDownKey { get; set; }
        public Key MoveLeftKey { get; set; }
        public Key MoveRightKey { get; set; }
        public Type SystemType { get { return null; } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Initialize(IEntity entity)
        {

        }

        #endregion Public Methods
    }
}