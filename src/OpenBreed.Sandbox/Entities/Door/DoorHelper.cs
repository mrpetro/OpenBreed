using OpenBreed.Common.Tools;
using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Wecs.Systems.Rendering.Commands;
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
using OpenBreed.Wecs.Commands;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Common;

namespace OpenBreed.Sandbox.Entities.Door
{
    public class DoorHelper
    {
        public DoorHelper(ICore core)
        {
            this.core = core;
        }

        private const string TILE_ATLAS = "Atlases/Tiles/16/Test";
        private const string STAMP_DOOR_HORIZONTAL_CLOSED = "Tiles/Stamps/DoorHorizontal/Closed";
        private const string STAMP_DOOR_HORIZONTAL_OPENED = "Tiles/Stamps/DoorHorizontal/Opened";
        private const string STAMP_DOOR_VERTICAL_CLOSED = "Tiles/Stamps/DoorVertical/Closed";
        private const string STAMP_DOOR_VERTICAL_OPENED = "Tiles/Stamps/DoorVertical/Opened";
        private readonly ICore core;

        public void CreateAnimations()
        {
            var dataLoaderFactory = core.GetManager<IDataLoaderFactory>();
            var animationLoader = dataLoaderFactory.GetLoader<IClip>();

            animationLoader.Load("Animations.DoorVertical.Opening");
            animationLoader.Load("Animations.DoorVertical.Closing");
            animationLoader.Load("Animations.DoorHorizontal.Opening");
            animationLoader.Load("Animations.DoorHorizontal.Closing");
        }

        public void AddVerticalDoor(World world, int x, int y)
        {
            var doorVerticalTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\Door\DoorVertical.xml");
            var door = core.GetManager<IEntityFactory>().Create(doorVerticalTemplate);

            door.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);
            door.Add(new CollisionComponent());

            core.Commands.Post(new AddEntityCommand(world.Id, door.Id));
        }

        public void AddHorizontalDoor(World world, int x, int y)
        {
            var doorHorizontalTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\Door\DoorHorizontal.xml");
            var door = core.GetManager<IEntityFactory>().Create(doorHorizontalTemplate);

            door.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);
            door.Add(new CollisionComponent());

            core.Commands.Post(new AddEntityCommand(world.Id, door.Id));
        }

        public void LoadStamps()
        {
            var dataLoaderFactory = core.GetManager<IDataLoaderFactory>();
            var tileStampLoader = dataLoaderFactory.GetLoader<ITileStamp>();

            tileStampLoader.Load("Tiles/Stamps/DoorHorizontal/Closed");
            tileStampLoader.Load("Tiles/Stamps/DoorHorizontal/Opened");
            tileStampLoader.Load("Tiles/Stamps/DoorVertical/Closed");
            tileStampLoader.Load("Tiles/Stamps/DoorVertical/Opened");
        }
    }
}
