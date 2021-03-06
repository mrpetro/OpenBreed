﻿//using System;
//using OpenBreed.Core;
//using OpenBreed.Core.Common;
//using OpenBreed.Core.Common.Systems.Components;
//using OpenBreed.Core.Entities;
//using OpenBreed.Physics.Generic.Components;
//using OpenBreed.Physics.Generic.Events;
//using OpenBreed.Core.States;
//using OpenBreed.Sandbox.Entities.Pickable.States;
//using OpenBreed.Sandbox.Helpers;
//using OpenTK;

//namespace OpenBreed.Sandbox.Entities.Pickable
//{
//    public class PickableHelper
//    {
//        private const string STAMP_AMMO_LYING = "Tiles/Stamps/Items/Ammo/Lying";
//        private const string STAMP_AMMO_PICKED = "Tiles/Stamps/Items/Ammo/Picked";
//        #region Public Methods

//        public static StateMachine CreateFSM(Entity entity)
//        {
//            var stateMachine = entity.AddFSM("Functioning");

//            var lyingStamp = entity.Core.Rendering.Stamps.GetByName(STAMP_AMMO_LYING);
//            var pickedStamp = entity.Core.Rendering.Stamps.GetByName(STAMP_AMMO_PICKED);

//            stateMachine.AddState(new LyingState("Lying", lyingStamp.Id));
//            stateMachine.AddState(new PickingState("Picking", pickedStamp.Id));

//            return stateMachine;
//        }

//        public static void AddItem(ICore core, World world, int x, int y)
//        {
//            var item = core.GetManager<IEntityMan>().Create();

//            //item.Add(new Animator(5.0f, false));
//            item.Add(BodyComponent.Create(1.0f, 1.0f, "Trigger"));
//            item.Add(Position.Create(x * 16, y * 16));
//            item.Add(AxisAlignedBoxShape.Create(0, 0, 16, 16));
//            item.Add(TextHelper.Create(core, new Vector2(0, 20), "Ammo"));

//            var doorSm = PickableHelper.CreateFSM(item);
//            doorSm.SetInitialState("Lying");
//            world.AddEntity(item);
//        }

//        #endregion Public Methods

//        #region Private Methods

//        public static void CreateStamps(ICore core)
//        {
//            var stampBuilder = core.Rendering.Stamps.Create();

//            stampBuilder.ClearTiles();
//            stampBuilder.SetName(STAMP_AMMO_LYING);
//            stampBuilder.SetSize(1, 1);
//            stampBuilder.SetOrigin(0, 0);
//            stampBuilder.AddTile(0, 0, 13);
//            stampBuilder.Build();

//            stampBuilder.ClearTiles();
//            stampBuilder.SetName(STAMP_AMMO_PICKED);
//            stampBuilder.SetSize(1, 1);
//            stampBuilder.SetOrigin(0, 0);
//            stampBuilder.AddTile(0, 0, 12);
//            stampBuilder.Build();
//        }

//        #endregion Private Methods
//    }
//}