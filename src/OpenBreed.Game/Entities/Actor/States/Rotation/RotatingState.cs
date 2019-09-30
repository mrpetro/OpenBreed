using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenBreed.Game.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Entities.Actor.States.Rotation
{
    public class RotatingState : IState
    {
        #region Private Fields

        private readonly string animPrefix;

        #endregion Private Fields

        #region Public Constructors

        public RotatingState(string id, string animPrefix)
        {
            Name = id;
            this.animPrefix = animPrefix;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Name { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            var direction = Entity.Components.OfType<Direction>().First();
            var movement = Entity.Components.OfType<Motion>().First();
            Entity.Components.OfType<IThrust>().First().Value = direction.Value * movement.Acceleration;

            var animDirName = AnimHelper.ToDirectionName(direction.Value);
            var animMovementName = Entity.FsmList.First(item => item.Name == "Movement");

            Entity.PostMsg(new PlayAnimMsg(Entity, $"{animPrefix}/{animMovementName.CurrentStateName}/{animDirName}"));
            Entity.PostMsg(new TextSetMsg(Entity, String.Join(", ", Entity.CurrentStateNames.ToArray())));

            Entity.PostMsg(new StateChangeMsg(Entity, "Rotation", "Stop"));
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
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
                        return "Idle";
                    }
                default:
                    break;
            }

            return null;
        }

        #endregion Public Methods

    }
}
