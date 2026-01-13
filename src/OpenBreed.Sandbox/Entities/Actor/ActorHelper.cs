using OpenBreed.Animation.Interface;
using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Input.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Control.Components;
using OpenBreed.Wecs.Physics.Components;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Control.Systems.Events;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Physics.Systems.Helpers;
using OpenBreed.Wecs.Scripting.Systems.Extensions;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Linq;
using System.Xml.Linq;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Physics.Interface;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Primitives;

namespace OpenBreed.Sandbox.Entities.Actor
{
    public enum HeroStates
    {
        Silent,
        Speaks,
        FinishedSpeaking
    }

    public enum SpeakerImpulses
    {

    }

    public class HeroFsm
    {



        public void Walking(int entityId, Vector2 direction)
        {

        }

        public void Standing(int entityId)
        {

        }

        public void Interacting(int entityId)
        {

        }

        public void Shooting(int entityId)
        {

        }
    }


    public class ActorHelper
    {
        #region Private Fields

        private readonly IClipMan<IEntity> clipMan;

        private readonly ICollisionMan<IEntity> collisionMan;
        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;

        private readonly DynamicResolver dynamicResolver;
        private readonly FixtureTypes fixtureTypes;
        private readonly IScriptMan scriptMan;
        private readonly IFsmMan fsmMan;
        private readonly ITriggerMan triggerMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public ActorHelper(
            IClipMan<IEntity> clipMan,           
            ICollisionMan<IEntity> collisionMan,
            IEntityMan entityMan,
            IWorldMan worldMan,
            IDataLoaderFactory dataLoaderFactory,
            IEntityFactory entityFactory,
            DynamicResolver dynamicResolver,
            FixtureTypes fixtureTypes,
            IScriptMan scriptMan,
            IFsmMan fsmMan,
            ITriggerMan triggerMan,
            IEventsMan eventsMan)
        {
            this.clipMan = clipMan;
            this.collisionMan = collisionMan;
            this.entityMan = entityMan;
            this.worldMan = worldMan;
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
            this.dynamicResolver = dynamicResolver;
            this.fixtureTypes = fixtureTypes;
            this.scriptMan = scriptMan;
            this.fsmMan = fsmMan;
            this.triggerMan = triggerMan;
            this.eventsMan = eventsMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntity CreateMission(string name)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\Mission")
                .SetTag(name)
                .Build();

            return entity;
        }

        public IEntity AddHeavyTurret(IWorld world, float x, float y)
        {
            var entity = entityFactory.Create($@"ABTA\Templates\L1\Turret")
                .SetParameter("startX", x)
                .SetParameter("startY", y)
                .Build();

            entity.CreateTimer("CooldownDelay");
            entity.CreateTimer("ActionDeley");

            entity.Add(new TrackingComponent(-1));

            worldMan.RequestAddEntity(entity, world.Id);
            return entity;
        }

        public IEntity CreatePlayerActor(string name, Vector2 pos)
        {
            var actor = CreateActor(name, pos);
            actor.CreateTimer("CooldownDelay");
            actor.CreateTimer("ActionDeley");

            //actor.Add(new InventoryComponent(new Bag[] { new Bag("Backpack") }));
            actor.Add(new EquipmentComponent(
                new []{
                    new EquipmentSlot("Torso"),
                    new EquipmentSlot("Hands")
                }));
            actor.Add(new InventoryComponent(16));

            return actor;
        }

        public IEntity CreateActor(string name, Vector2 pos)
        {
            var actor = entityFactory.Create($@"ABTA\Templates\Common\Actors\{name}")
                .SetParameter("startX", pos.X)
                .SetParameter("startY", pos.Y)
                .SetTag(name)
                .Build();

            return actor;
        }

        #endregion Public Methods

        #region Internal Methods

        #endregion Internal Methods
    }
}