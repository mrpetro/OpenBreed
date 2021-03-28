﻿using OpenBreed.Core.Commands;
using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs;
using OpenBreed.Fsm;
using OpenBreed.Common.Logging;

namespace OpenBreed.Wecs.Systems.Core
{
    public class FsmSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly IEntityMan entityMan;
        private readonly IFsmMan fsmMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public FsmSystem(IEntityMan entityMan, ICommandsMan commandsMan, IFsmMan fsmMan, ILogger logger)
        {
            this.entityMan = entityMan;
            this.fsmMan = fsmMan;
            this.logger = logger;

            Require<FsmComponent>();



        }

        #endregion Public Constructors

        #region Public Methods

        public static void RegisterHandlers(ICommandsMan commandsMan)
        {
            commandsMan.Register<SetEntityStateCommand>(HandleSetStateCommand);
        }

        public void Update(float dt)
        {
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
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
                fsmMan.EnterState(entity, state, 0);
        }

        private void DeinitializeComponent(Entity entity)
        {
            var fsmComponent = entity.Get<FsmComponent>();

            foreach (var state in fsmComponent.States)
                fsmMan.EnterState(entity, state, 0);
        }

        private static bool HandleSetStateCommand(ICore core, SetEntityStateCommand cmd)
        {
            var entity = core.GetManager<IEntityMan>().GetById(cmd.EntityId);

            var fsmComponent = entity.Get<FsmComponent>();

            if (fsmComponent == null)
            {
                core.Logging.Warning($"Entity '{cmd.EntityId}' has missing FSM component.");
                return false;
            }

            var fsm = core.GetManager<IFsmMan>().GetById(cmd.FsmId);

            var fsmData = fsmComponent.States.FirstOrDefault(item => item.FsmId == cmd.FsmId);

            if (fsmData == null)
            {
                core.Logging.Warning($"Entity '{cmd.EntityId}' has missing data for FSM '{fsm.Name}'.");
                return false;
            }

            core.GetManager<IFsmMan>().LeaveState(entity, fsmData, cmd.ImpulseId);
            var nextStateId = fsm.GetNextStateId(fsmData.StateId, cmd.ImpulseId);

            if (nextStateId == -1)
            {
                var fromStateName = fsm.GetStateName(fsmData.StateId);
                var impulseName = fsm.GetImpulseName(cmd.ImpulseId);

                core.Logging.Warning($"Entity '{cmd.EntityId}' has missing FSM transition from state '{fromStateName}' using impulse '{impulseName}'.");
                return false;
            }

            fsmData.StateId = nextStateId;
            core.GetManager<IFsmMan>().EnterState(entity, fsmData, cmd.ImpulseId);

            return true;
        }

        #endregion Private Methods
    }
}