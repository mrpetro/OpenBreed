using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Core.Helpers;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Systems
{
    public class TextInputSystem : WorldSystem, ICommandExecutor, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private CommandHandler cmdHandler;

        #endregion Private Fields

        #region Public Constructors

        public TextInputSystem(ICore core) : base(core)
        {
            cmdHandler = new CommandHandler(this);

            Require<TextCaretComponent>();
            Require<TextDataComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.RegisterHandler(TextCaretSetPosition.TYPE, cmdHandler);
            World.RegisterHandler(TextDataInsert.TYPE, cmdHandler);
            World.RegisterHandler(TextDataBackspace.TYPE, cmdHandler);
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
            cmdHandler.ExecuteEnqueued();
        }

        public void Update(float dt)
        {
            cmdHandler.ExecuteEnqueued();
        }

        public override bool ExecuteCommand(object sender, ICommand cmd)
        {
            switch (cmd.Type)
            {
                case TextCaretSetPosition.TYPE:
                    return HandleTextCaretSetPosition(sender, (TextCaretSetPosition)cmd);
                case TextDataInsert.TYPE:
                    return HandleTextDataInsert(sender, (TextDataInsert)cmd);
                case TextDataBackspace.TYPE:
                    return HandleTextDataBackspace(sender, (TextDataBackspace)cmd);
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

        private bool HandleTextDataInsert(object sender, TextDataInsert cmd)
        {
            var toModify = entities.FirstOrDefault(item => item.Id == cmd.EntityId);
            if (toModify == null)
                return false;

            var caretCmp = toModify.GetComponent<TextCaretComponent>();
            var textCmp = toModify.GetComponent<TextDataComponent>();

            if (string.IsNullOrEmpty(cmd.Text))
                return true;

            textCmp.Insert(caretCmp.Position, cmd.Text);
            caretCmp.Position += cmd.Text.Length;

            toModify.RaiseEvent(new TextDataChanged(textCmp.Data));
            toModify.RaiseEvent(new TextCaretPositionChanged(caretCmp.Position));

            return true;
        }

        private bool HandleTextDataBackspace(object sender, TextDataBackspace cmd)
        {
            var toModify = entities.FirstOrDefault(item => item.Id == cmd.EntityId);
            if (toModify == null)
                return false;

            var caretCmp = toModify.GetComponent<TextCaretComponent>();
            var textCmp = toModify.GetComponent<TextDataComponent>();

            if (string.IsNullOrEmpty(textCmp.Data))
                return true;

            if (caretCmp.Position == 0)
                return true;

            textCmp.Remove(caretCmp.Position - 1, 1);
            caretCmp.Position -= 1;

            toModify.RaiseEvent(new TextDataChanged(textCmp.Data));
            toModify.RaiseEvent(new TextCaretPositionChanged(caretCmp.Position));

            return true;
        }

        private bool HandleTextCaretSetPosition(object sender, TextCaretSetPosition cmd)
        {
            var toModify = entities.FirstOrDefault(item => item.Id == cmd.EntityId);
            if (toModify == null)
                return false;

            var caretCmp = toModify.GetComponent<TextCaretComponent>();
            var textCmp = toModify.GetComponent<TextDataComponent>();

            if (textCmp.Data.Length == 0)
                return true;

            var newPosition = cmd.NewPosition;

            if (newPosition < 0)
                newPosition = 0;
            else if (newPosition > textCmp.Data.Length)
                newPosition = textCmp.Data.Length;

            // Nothing to do here
            if (newPosition == caretCmp.Position)
                return true;

            caretCmp.Position = cmd.NewPosition;
            toModify.RaiseEvent(new TextCaretPositionChanged(caretCmp.Position));

            return true;
        }

        #endregion Private Methods
    }
}