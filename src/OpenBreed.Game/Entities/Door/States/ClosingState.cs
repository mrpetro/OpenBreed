using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Messages;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenTK;
using System.Linq;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Modules.Physics.Messages;

namespace OpenBreed.Game.Components.States
{
    public class ClosingState : IState
    {
        #region Private Fields

        private readonly string animationId;
        private IEntity[] doorParts;
        private Animator<int> animator;
        private ISprite sprite;

        #endregion Private Fields

        #region Public Constructors

        public ClosingState(string id, string animationId)
        {
            Id = id;
            this.animationId = animationId;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Id { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            Entity.PostMsg(new SpriteOnMsg(Entity));

            foreach (var part in doorParts)
                Entity.PostMsg(new BodyOnMsg(part));

            Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostMsg(new TextSetMsg(Entity, "Door - Closing"));
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            sprite = entity.Components.OfType<ISprite>().First();
            animator = entity.Components.OfType<Animator<int>>().First();
            doorParts = Entity.World.Systems.OfType<GroupSystem>().First().GetGroup(Entity).ToArray();
        }

        public void LeaveState()
        {
        }

        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                case "Close":
                    return "Closed";
                default:
                    break;
            }

            return null;
        }

        #endregion Public Methods

    }
}