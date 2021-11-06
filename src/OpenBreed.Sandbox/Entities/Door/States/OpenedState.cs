﻿using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenTK;
using System.Linq;
using OpenBreed.Sandbox.Entities.Door.States;
using System;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Rendering.Interface;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Physics.Extensions;

namespace OpenBreed.Sandbox.Components.States
{
    public class OpenedState : IState
    {
        #region Private Fields

        private readonly string stampPrefix;
        private readonly IFsmMan fsmMan;
        private readonly ICommandsMan commandsMan;
        private readonly IStampMan stampMan;

        #endregion Private Fields

        #region Public Constructors

        public OpenedState(IFsmMan fsmMan, ICommandsMan commandsMan, IStampMan stampMan)
        {
            this.stampPrefix = "Tiles/Stamps";
            this.fsmMan = fsmMan;
            this.commandsMan = commandsMan;
            this.stampMan = stampMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)(ValueType)FunctioningState.Opened;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            entity.SetSpriteOff();
            entity.SetBodyOff();

            var pos = entity.Get<PositionComponent>();

            //entity.PostCommand(new PutStampCommand(entity.World.Id, stampId, 0, pos.Value));

            var className = entity.Get<ClassComponent>().Name;
            var stateName = fsmMan.GetStateName(FsmId, Id);
            var stampId = stampMan.GetByName($"{stampPrefix}/{className}/{stateName}").Id;

            entity.PutStamp(stampId, 0, pos.Value);
            //commandsMan.Post(new PutStampCommand(entity.Id, stampId, 0, pos.Value));

            entity.SetText(0, "Door - Opened");
        }

        public void LeaveState(Entity entity)
        {
        }

        #endregion Public Methods

    }
}