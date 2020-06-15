using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Systems;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class TextPresenterSystem : WorldSystem, ICommandExecutor, IRenderableSystem
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private CommandHandler cmdHandler;

        #endregion Private Fields

        #region Internal Constructors

        public TextPresenterSystem(ICore core) : base(core)
        {
            cmdHandler = new CommandHandler(this);

            Require<TextDataComponent>();
            Require<TextPresentationComponent>();
            Require<PositionComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.RegisterHandler(TextSetCommand.TYPE, cmdHandler);
        }

        public void Render(Box2 clipBox, int depth, float dt)
        {
            cmdHandler.ExecuteEnqueued();

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black);
            //GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < entities.Count; i++)
                RenderText(entities[i], clipBox);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        public override bool ExecuteCommand(ICommand cmd)
        {
            switch (cmd.Type)
            {
                case TextSetCommand.TYPE:
                    return HandleTextSetCommand((TextSetCommand)cmd);

                default:
                    return false;
            }
        }

        public bool EnqueueMsg(object sender, IEntityCommand msg)
        {
            return false;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private void RenderText(IEntity entity, Box2 clipBox)
        {
            var pos = entity.GetComponent<PositionComponent>();
            var tp = entity.GetComponent<TextPresentationComponent>();
            var td = entity.GetComponent<TextDataComponent>();

            GL.Enable(EnableCap.Texture2D);
            GL.PushMatrix();

            GL.Translate(pos.Value.X, pos.Value.Y, 0.0f);

            var caretPosX = 0.0f;
            var font = Core.Rendering.Fonts.GetById(tp.FontId);
            var height = font.Height;

            for (int i = 0; i < td.Data.Length; i++)
            {
                var ch = td.Data[i];

                switch (ch)
                {
                    case '\r':
                        GL.Translate(-caretPosX, 0.0f, 0.0f);
                        caretPosX = 0.0f;
                        continue;
                    case '\n':
                        GL.Translate(0.0f, -height, 0.0f);
                        continue;
                    default:
                        break;
                }

                font.Draw(ch);
                var width = font.GetWidth(ch);
                caretPosX += width;
                GL.Translate(width, 0.0f, 0.0f);
            }

            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        private bool HandleTextSetCommand(TextSetCommand cmd)
        {
            var toModify = entities.FirstOrDefault(item => item.Id == cmd.EntityId);
            if (toModify == null)
                return false;

            var text = toModify.GetComponent<TextComponent>();

            if (cmd.PartId < 0 || cmd.PartId >= text.Parts.Count)
            {
                Core.Logging.Error($"Unknown text part ID({cmd.PartId}) to modify.");
            }

            text.Parts[cmd.PartId].Text = cmd.Text;

            return true;
        }

        #endregion Private Methods
    }
}