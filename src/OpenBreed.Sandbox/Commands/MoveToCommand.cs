﻿//using OpenBreed.Core.Commands;
//using OpenBreed.Core.Modules.Animation.Systems.Control.Components;
//using OpenBreed.Sandbox.Entities;
//using OpenTK;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OpenBreed.Sandbox.Commands
//{
//    public class MoveToCommand : ICommand
//    {
//        private AiControl controller;
//        private Vector2 position;

//        public MoveToCommand(Entity actor, Vector2 position)
//        {
//            controller = actor.TryGetComponent<AiControl>().FirstOrDefault();

//            this.position = position;
//        }

//        public void Execute()
//        {
//            controller.Waypoints.Add(position);
//        }
//    }
//}
