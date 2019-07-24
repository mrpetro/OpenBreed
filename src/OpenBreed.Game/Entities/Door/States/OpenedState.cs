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
using OpenBreed.Core.Common.Helpers;

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
            Entity.Core.MessageBus.PostMsg(this, new SetTextMsg(Entity, "Door - Opened"));
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