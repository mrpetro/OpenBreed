using OpenBreed.Core.Commands;
using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Core.Managers;
using System.Collections.Generic;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs;

namespace OpenBreed.Wecs.Systems.Core
{
    public class TextInputSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();

        #endregion Private Fields

        #region Public Constructors

        public TextInputSystem(ICore core) : base(core)
        {
            Require<TextCaretComponent>();
            Require<TextDataComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public static void RegisterHandlers(ICommandsMan commands)
        {
            commands.Register<TextCaretSetPosition>(HandleTextCaretSetPosition);
            commands.Register<TextDataInsert>(HandleTextDataInsert);
            commands.Register<TextDataBackspace>(HandleTextDataBackspace);
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
        }

        public void Update(float dt)
        {
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private static bool HandleTextDataInsert(ICore core, TextDataInsert cmd)
        {
            var toModify = core.GetManager<IEntityMan>().GetById(cmd.EntityId);
            if (toModify == null)
                return false;

            var caretCmp = toModify.Get<TextCaretComponent>();
            var textCmp = toModify.Get<TextDataComponent>();

            if (string.IsNullOrEmpty(cmd.Text))
                return true;

            textCmp.Insert(caretCmp.Position, cmd.Text);
            caretCmp.Position += cmd.Text.Length;

            toModify.RaiseEvent(new TextDataChanged(textCmp.Data, TextDataChanged.ChangeType.Inserted, caretCmp.Position - cmd.Text.Length, cmd.Text.Length));
            toModify.RaiseEvent(new TextCaretPositionChanged(caretCmp.Position));

            return true;
        }

        private static bool HandleTextDataBackspace(ICore core, TextDataBackspace cmd)
        {
            var toModify = core.GetManager<IEntityMan>().GetById(cmd.EntityId);
            if (toModify == null)
                return false;

            var caretCmp = toModify.Get<TextCaretComponent>();
            var textCmp = toModify.Get<TextDataComponent>();

            if (string.IsNullOrEmpty(textCmp.Data))
                return true;

            if (caretCmp.Position == 0)
                return true;

            textCmp.Remove(caretCmp.Position - 1, 1);
            caretCmp.Position -= 1;

            toModify.RaiseEvent(new TextDataChanged(textCmp.Data, TextDataChanged.ChangeType.Removed, caretCmp.Position + 1, 1));
            toModify.RaiseEvent(new TextCaretPositionChanged(caretCmp.Position));

            return true;
        }

        private static bool HandleTextCaretSetPosition(ICore core, TextCaretSetPosition cmd)
        {
            var toModify = core.GetManager<IEntityMan>().GetById(cmd.EntityId);
            if (toModify == null)
                return false;

            var caretCmp = toModify.Get<TextCaretComponent>();
            var textCmp = toModify.Get<TextDataComponent>();

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