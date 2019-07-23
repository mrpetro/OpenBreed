using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenTK;
using System.Linq;
using OpenBreed.Core.Common.Systems;

namespace OpenBreed.Game.Components.States
{
    public class ClosedState : IState
    {
        #region Private Fields

        private int tileId;

        #endregion Private Fields

        #region Public Constructors

        public ClosedState(string id, int tileId)
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
            Entity.Core.MessageBus.PostMsg(this, new SetTextMsg(Entity, "Door - Closed"));
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
        }

        private void HandleControlDirectionChangedEvent(IWorldSystem system, ControlDirectionChangedEvent systemEvent)
        {
            if (systemEvent.Direction != Vector2.Zero)
                Entity.PostMsg(new StateChangeMsg(Entity, "Walk", systemEvent.Direction));
            else
                Entity.PostMsg(new StateChangeMsg(Entity, "Stop"));
        }

        #endregion Private Methods
    }
}