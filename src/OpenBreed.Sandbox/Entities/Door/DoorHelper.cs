﻿using OpenBreed.Common.Tools;
using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Rendering.Interface;
using OpenBreed.Sandbox.Components;
using OpenBreed.Sandbox.Components.States;
using OpenBreed.Sandbox.Entities.Door.States;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Animation.Interface;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Entities;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Common;
using System.Globalization;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Audio.Interface.Data;

namespace OpenBreed.Sandbox.Entities.Door
{
    public class DoorHelper
    {
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;

        public DoorHelper(IDataLoaderFactory dataLoaderFactory, IEntityFactory entityFactory)
        {
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
        }

        public void LoadAnimations()
        {
            var animationLoader = dataLoaderFactory.GetLoader<IAnimationClipDataLoader>();

            animationLoader.Load("Vanilla/L4/Door/Opening/Vertical");
            animationLoader.Load("Vanilla/L4/Door/Closing/Vertical");
            animationLoader.Load("Vanilla/L4/Door/Opening/Horizontal");
            animationLoader.Load("Vanilla/L4/Door/Closing/Horizontal");

            var soundSampleLoader = dataLoaderFactory.GetLoader<ISoundSampleDataLoader>();
            
            soundSampleLoader.Load("Vanilla/Common/Door/Opening");
        }

        public void AddVertical(World world, int x, int y)
        {
            var door = entityFactory.Create(@"Entities\Door\DoorVertical.xml")
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();

            door.EnterWorld(world.Id);
        }

        public void AddHorizontal(World world, int x, int y)
        {
            var door = entityFactory.Create(@"Entities\Door\DoorHorizontal.xml")
                .SetParameter("startX", 16 * x)
                .SetParameter("startY", 16 * y)
                .Build();


            door.EnterWorld(world.Id);
        }

        public void LoadStamps()
        {
            var tileStampLoader = dataLoaderFactory.GetLoader<ITileStampDataLoader>();

            tileStampLoader.Load("Vanilla/L4/Door/Horizontal/Closed");
            tileStampLoader.Load("Vanilla/L4/Door/Horizontal/Opened");
            tileStampLoader.Load("Vanilla/L4/Door/Vertical/Closed");
            tileStampLoader.Load("Vanilla/L4/Door/Vertical/Opened");
        }
    }
}
