using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems;
using OpenBreed.Core.Systems.Animation.Components;
using OpenBreed.Core.Systems.Animation.Events;
using OpenBreed.Core.Systems.Animation.Messages;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Control.Events;
using OpenBreed.Core.Systems.Movement.Components;
using OpenTK;
using System.Linq;

namespace OpenBreed.Game.Components.States
{
    public class OpenedState : IState
    {
        #region Private Fields

        private readonly int tileId;
        #endregion Private Fields

        #region Public Constructors

        public OpenedState(string id, int tileId)
        {
            Id = id;
            this.tileId = tileId;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Id { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            //Entity.PostMessage(new ChangeTileMsg(tileId));
            Entity.PostMessage(new SetTextMsg("Door - Opened"));
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;

            entity.HandleSystemEvent = HandleSystemEvent;
        }

        public void LeaveState()
        {
        }

        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Stop":
                    {
                        break;
                    }
                case "Walk":
                    {
                        break;
                    }
                default:
                    break;
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private void HandleSystemEvent(IWorldSystem system, ISystemEvent systemEvent)
        {
            switch (systemEvent.Type)
            {
                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}