using OpenBreed.Animation.Interface;
using OpenBreed.Common.Tools;
using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Rendering.Interface;
using OpenTK;
using OpenTK.Graphics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Common.Tools.Xml;

namespace OpenBreed.Sandbox
{
    public class Misc
    {
        public Misc(IClipMan clipMan, IFontMan fontMan, IEntityFactory entityFactory)
        {
            this.clipMan = clipMan;
            this.fontMan = fontMan;
            this.entityFactory = entityFactory;
        }

        #region Public Fields

        public const string CRAZY_MOVE_ANIM = "Animations/Misc/CrazyMove";
        private readonly IClipMan clipMan;
        private readonly IFontMan fontMan;
        private readonly IEntityFactory entityFactory;

        #endregion Public Fields

        #region Public Methods

        public void CreateAnimations()
        {
            var updatePosAnim = clipMan.CreateClip(CRAZY_MOVE_ANIM, 25.0f);
            var updateX = updatePosAnim.AddTrack<float>(FrameInterpolation.Linear, OnUpdatePosXFrame, 0);
            updateX.AddFrame(5 * 16, 5.0f);
            updateX.AddFrame(0 * 16, 10.0f);
            updateX.AddFrame(1 * 16, 15.0f);
            updateX.AddFrame(4 * 16, 20.0f);
            updateX.AddFrame(0 * 16, 25.0f);

            var updateY = updatePosAnim.AddTrack<float>(FrameInterpolation.Linear, OnUpdatePosYFrame, 0);
            updateY.AddFrame(0 * 16, 5.0f);
            updateY.AddFrame(1 * 16, 10.0f);
            updateY.AddFrame(5 * 16, 15.0f);
            updateY.AddFrame(2 * 16, 20.0f);
            updateY.AddFrame(0 * 16, 25.0f);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnUpdatePosXFrame(Entity entity, float nextValue)
        {
            var pos = entity.Get<PositionComponent>();
            pos.Value = new OpenTK.Vector2(nextValue, pos.Value.Y);
        }

        private void OnUpdatePosYFrame(Entity entity, float nextValue)
        {
            var pos = entity.Get<PositionComponent>();
            pos.Value = new OpenTK.Vector2(pos.Value.X, nextValue);
        }

        public Entity AddToWorld(World world)
        {
            var arial12 = fontMan.Create("ARIAL", 10);

            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(@"Entities\CrazyMover\CrazyMover.xml");
            var crazyMover = entityFactory.Create(entityTemplate);

            crazyMover.EnterWorld(world.Id);

            return crazyMover;
        }

        #endregion Private Methods
    }
}