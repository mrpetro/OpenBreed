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
            var animationLoader = dataLoaderFactory.GetLoader<IClip>();

            animationLoader.Load("Animations.DoorVertical.Opening");
            animationLoader.Load("Animations.DoorVertical.Closing");
            animationLoader.Load("Animations.DoorHorizontal.Opening");
            animationLoader.Load("Animations.DoorHorizontal.Closing");
        }

        public void AddVerticalDoor(World world, int x, int y)
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("startX", (16 * x).ToString(CultureInfo.InvariantCulture));
            dictionary.Add("startY", (16 * y).ToString(CultureInfo.InvariantCulture));

            var doorVerticalTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\Door\DoorVertical.xml", dictionary);
            var door = entityFactory.Create(doorVerticalTemplate);

            door.EnterWorld(world.Id);
        }

        public void AddHorizontalDoor(World world, int x, int y)
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("startX", (16 * x).ToString(CultureInfo.InvariantCulture));
            dictionary.Add("startY", (16 * y).ToString(CultureInfo.InvariantCulture));

            var doorHorizontalTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\Door\DoorHorizontal.xml", dictionary);
            var door = entityFactory.Create(doorHorizontalTemplate);

            door.EnterWorld(world.Id);
        }

        public void LoadStamps()
        {
            var tileStampLoader = dataLoaderFactory.GetLoader<ITileStamp>();

            tileStampLoader.Load("Tiles/Stamps/DoorHorizontal/Closed");
            tileStampLoader.Load("Tiles/Stamps/DoorHorizontal/Opened");
            tileStampLoader.Load("Tiles/Stamps/DoorVertical/Closed");
            tileStampLoader.Load("Tiles/Stamps/DoorVertical/Opened");
        }
    }
}
