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
            var animations = core.GetManager<IAnimMan>();

            var horizontalDoorOpening = animations.Create("Animations/DoorHorizontal/Opening", 5.0f);
            var hdo = horizontalDoorOpening.AddPart<int>(OnFrameUpdate, 0);
            hdo.AddFrame(0, 1.0f);
            hdo.AddFrame(1, 2.0f);
            hdo.AddFrame(2, 3.0f);
            hdo.AddFrame(3, 4.0f);
            hdo.AddFrame(4, 5.0f);
            var horizontalDoorClosing = animations.Create("Animations/DoorHorizontal/Closing", 5.0f);
            var hdc = horizontalDoorClosing.AddPart<int>(OnFrameUpdate, 4);
            hdc.AddFrame(4, 1.0f);
            hdc.AddFrame(3, 2.0f);
            hdc.AddFrame(2, 3.0f);
            hdc.AddFrame(1, 4.0f);
            hdc.AddFrame(0, 5.0f);
            var verticalDoorOpening = animations.Create("Animations/DoorVertical/Opening", 5.0f);
            var vdo = verticalDoorOpening.AddPart<int>(OnFrameUpdate, 0);
            vdo.AddFrame(0, 1.0f);
            vdo.AddFrame(1, 2.0f);
            vdo.AddFrame(2, 3.0f);
            vdo.AddFrame(3, 4.0f);
            vdo.AddFrame(4, 5.0f);
            var verticalDoorClosing = animations.Create("Animations/DoorVertical/Closing", 5.0f);
            var vdc = verticalDoorClosing.AddPart<int>(OnFrameUpdate, 4);
            vdc.AddFrame(4, 1.0f);
            vdc.AddFrame(3, 2.0f);
            vdc.AddFrame(2, 3.0f);
            vdc.AddFrame(1, 4.0f);
            vdc.AddFrame(0, 5.0f);
        }

        private void OnFrameUpdate(Entity entity, int nextValue)
        {
            core.Commands.Post(new SpriteSetCommand(entity.Id, nextValue));
        }

        private void OnOpenningEnding()
        {
            Console.WriteLine("Door -> OpenningEnding");
        }

        private void OnOpeningStarting()
        {
            Console.WriteLine("Door -> OpenningStarting");
        }

        private void OnOpen()
        {
            Console.WriteLine("Door -> Open");
        }

        private void OnOpened()
        {
            Console.WriteLine("Door -> Opened");
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

        public void CreateStamps()
        {
            var stampBuilder = core.GetManager<IStampMan>().Create();

            stampBuilder.ClearTiles();
            stampBuilder.SetName(STAMP_DOOR_HORIZONTAL_CLOSED);
            stampBuilder.SetSize(2, 1);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, 0);
            stampBuilder.AddTile(1, 0, 1);
            stampBuilder.Build();

            stampBuilder.ClearTiles();
            stampBuilder.SetName(STAMP_DOOR_HORIZONTAL_OPENED);
            stampBuilder.SetSize(2, 1);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, 12);
            stampBuilder.AddTile(1, 0, 12);
            stampBuilder.Build();

            stampBuilder.ClearTiles();
            stampBuilder.SetName(STAMP_DOOR_VERTICAL_CLOSED);
            stampBuilder.SetSize(1, 2);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, 4);
            stampBuilder.AddTile(0, 1, 8);
            stampBuilder.Build();

            stampBuilder.ClearTiles();
            stampBuilder.SetName(STAMP_DOOR_VERTICAL_OPENED);
            stampBuilder.SetSize(1, 2);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, 12);
            stampBuilder.AddTile(0, 1, 12);
            stampBuilder.Build();

        }
    }
}
