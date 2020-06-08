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
    public class TextInputSystem : WorldSystem, ICommandExecutor
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
        }

        public override bool ExecuteCommand(object sender, ICommand cmd)
        {
            switch (cmd.Type)
            {
                case TextCaretSetPosition.TYPE:
                    return HandleTextCaretSetPosition(sender, (TextCaretSetPosition)cmd);

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

        private bool HandleTextCaretSetPosition(object sender, TextCaretSetPosition cmd)
        {
            var toModify = entities.FirstOrDefault(item => item.Id == cmd.EntityId);
            if (toModify == null)
                return false;

            var caretCmp = toModify.GetComponent<TextCaretComponent>();
            var textCmp = toModify.GetComponent<TextDataComponent>();

            // Nothing to do here
            if (cmd.NewPosition == caretCmp.Position)
                return true;

            if (cmd.NewPosition < 0)
                caretCmp.Position = 0;
            else if (cmd.NewPosition >= textCmp.Data.Length)
                caretCmp.Position = textCmp.Data.Length - 1;
            else
                caretCmp.Position = cmd.NewPosition;

            toModify.RaiseEvent(new TextCaretPositionChanged(cmd.NewPosition));

            return true;
        }

        #endregion Private Methods
    }
}