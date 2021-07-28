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


            var animationMan = core.GetManager<IClipMan>();
            var animatorMan = core.GetManager<IFrameUpdaterMan>();

            animatorMan.Register("Sprite.ImageId", (FrameUpdater<int>)OnImageIdUpdate);
            animatorMan.Register("Sprite.AtlasId", (FrameUpdater<string>)OnAtlasIdUpdate);

            var dataLoaderFactory = core.GetManager<IDataLoaderFactory>();
            var animationLoader = dataLoaderFactory.GetLoader<IClip>();

            var verticalDoorOpening = animationLoader.Load("Animations.DoorVertical.Opening");
            var verticalDoorClosing = animationLoader.Load("Animations.DoorVertical.Closing");
            var horizontalDoorOpening = animationLoader.Load("Animations.DoorHorizontal.Opening");
            var horizontalDoorClosing = animationLoader.Load("Animations.DoorHorizontal.Closing");
        }

        private void OnImageIdUpdate(Entity entity, int nextValue)
        {
            core.Commands.Post(new SpriteSetCommand(entity.Id, nextValue));
        }

        private void OnAtlasIdUpdate(Entity entity, string nextValue)
        {
            var atlas = core.GetManager<ISpriteMan>().GetByName(nextValue);
            core.Commands.Post(new SpriteSetAtlasCommand(entity.Id, atlas.Id));
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
            var tileMan = core.GetManager<ITileMan>();

            var tileSet = tileMan.GetByAlias("TileSets.L4");

            stampBuilder.ClearTiles();
            stampBuilder.SetName(STAMP_DOOR_HORIZONTAL_CLOSED);
            stampBuilder.SetSize(2, 1);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, tileSet.Id, 20 * 4 + 8);
            stampBuilder.AddTile(1, 0, tileSet.Id, 20 * 4 + 9);
            stampBuilder.Build();

            stampBuilder.ClearTiles();
            stampBuilder.SetName(STAMP_DOOR_HORIZONTAL_OPENED);
            stampBuilder.SetSize(2, 1);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, tileSet.Id, 20 * 2 + 0);
            stampBuilder.AddTile(1, 0, tileSet.Id, 20 * 2 + 1);
            stampBuilder.Build();

            stampBuilder.ClearTiles();
            stampBuilder.SetName(STAMP_DOOR_VERTICAL_CLOSED);
            stampBuilder.SetSize(1, 2);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, tileSet.Id, 20 * 1 + 8);
            stampBuilder.AddTile(0, 1, tileSet.Id, 20 * 2 + 8);
            stampBuilder.Build();

            stampBuilder.ClearTiles();
            stampBuilder.SetName(STAMP_DOOR_VERTICAL_OPENED);
            stampBuilder.SetSize(1, 2);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, tileSet.Id, 20 * 4 + 1);
            stampBuilder.AddTile(0, 1, tileSet.Id, 20 * 3 + 1);
            stampBuilder.Build();

        }
    }
}
