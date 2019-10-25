using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Animation.Systems.Control.Messages;
using OpenBreed.Core.Systems.Control.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Systems
{
    public class WalkingControlSystem : WorldSystem, IUpdatableSystem, IMsgListener
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private MsgHandler msgHandler;

        #endregion Private Fields

        #region Public Constructors

        public WalkingControlSystem(ICore core) : base(core)
        {
            msgHandler = new MsgHandler(this);

            Require<WalkingControl>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.MessageBus.RegisterHandler(WalkingControlMsg.TYPE, msgHandler);
        }

        public void Update(float dt)
        {
            msgHandler.PostEnqueued();
        }

        public override bool RecieveMsg(object sender, IMsg message)
        {
            switch (message.Type)
            {
                case WalkingControlMsg.TYPE:
                    return HandleWalkingControlMsg(sender, (WalkingControlMsg)message);

                default:
                    return false;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            var index = entities.IndexOf(entity);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HandleWalkingControlMsg(object sender, WalkingControlMsg msg)
        {
            var index = entities.IndexOf(msg.Entity);
            if (index < 0)
                return false;

            var control = msg.Entity.Components.OfType<WalkingControl>().First();

            if (control.Direction != msg.Direction)
            {
                control.Direction = msg.Direction;
                msg.Entity.RaiseEvent(new ControlDirectionChangedEvent(control.Direction));
            }

            return true;
        }

        #endregion Private Methods
    }
}