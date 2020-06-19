﻿using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Systems
{
    public class StateMachineSystem : WorldSystem, ICommandExecutor, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();

        private readonly CommandHandler cmdHandler;

        #endregion Private Fields

        #region Public Constructors

        public StateMachineSystem(ICore core) : base(core)
        {
            cmdHandler = new CommandHandler(this);
            Require<FsmComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.RegisterHandler(SetStateCommand.TYPE, cmdHandler);
        }

        public void Update(float dt)
        {
            cmdHandler.ExecuteEnqueued();
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
            cmdHandler.ExecuteEnqueued();
        }

        public override bool ExecuteCommand(ICommand cmd)
        {
            switch (cmd.Type)
            {
                case SetStateCommand.TYPE:
                    return HandleSetStateCommandMsg((SetStateCommand)cmd);

                default:
                    return false;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);

            InitializeComponent(entity);
            //World.Subscribe<EntityAddedEventArgs>(OnEntityEnteredWorld);
            //entity.Subscribe<EntityEnteredWorldEventArgs>(OnEntityEnteredWorld);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            //World.Unsubscribe<EntityAddedEventArgs>(OnEntityEnteredWorld);
            //entity.Unsubscribe<EntityEnteredWorldEventArgs>(OnEntityEnteredWorld);

            var index = entities.IndexOf(entity);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
        }

        #endregion Protected Methods

        #region Private Methods

        private void InitializeComponent(Entity entity)
        {
            var fsmComponent = entity.Get<FsmComponent>();

            foreach (var state in fsmComponent.States)
                Core.StateMachines.EnterState(entity, state);
        }

        private void DeinitializeComponent(Entity entity)
        {
            var fsmComponent = entity.Get<FsmComponent>();

            foreach (var state in fsmComponent.States)
                Core.StateMachines.EnterState(entity, state);
        }

        private bool HandleSetStateCommandMsg(SetStateCommand message)
        {
            var entity = Core.Entities.GetById(message.EntityId);

            var fsmComponent = entity.Get<FsmComponent>();

            if (fsmComponent == null)
            {
                Core.Logging.Warning($"Entity '{message.EntityId}' has missing FSM component.");
                return false;
            }

            var fsm = Core.StateMachines.GetById(message.FsmId);

            var fsmData = fsmComponent.States.FirstOrDefault(item => item.FsmId == message.FsmId);

            if (fsmData == null)
            {
                Core.Logging.Warning($"Entity '{message.EntityId}' has missing data for FSM '{fsm.Name}'.");
                return false;
            }

            Core.StateMachines.LeaveState(entity, fsmData);
            var nextStateId = fsm.GetNextStateId(fsmData.StateId, message.ImpulseId);

            if (nextStateId == -1)
            {
                var fromStateName = fsm.GetStateName(fsmData.StateId);
                var impulseName = fsm.GetImpulseName(message.ImpulseId);

                Core.Logging.Warning($"Entity '{message.EntityId}' has missing FSM transition from state '{fromStateName}' using impulse '{impulseName}'.");
                return false;
            }

            fsmData.StateId = nextStateId;
            Core.StateMachines.EnterState(entity, fsmData);

            return true;
        }

        #endregion Private Methods
    }
}