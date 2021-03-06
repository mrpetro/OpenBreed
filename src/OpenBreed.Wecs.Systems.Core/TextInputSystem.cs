﻿using OpenBreed.Core.Commands;
using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Events;
using OpenBreed.Core.Managers;
using System.Collections.Generic;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Systems.Core.Commands;

namespace OpenBreed.Wecs.Systems.Core
{
    public class TextInputSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Public Constructors

        internal TextInputSystem(IEntityMan entityMan)
        {
            this.entityMan = entityMan;

            Require<TextCaretComponent>();
            Require<TextDataComponent>();

            RegisterHandler<TextCaretSetPosition>(HandleTextCaretSetPosition);
            RegisterHandler<TextDataInsert>(HandleTextDataInsert);
            RegisterHandler<TextDataBackspace>(HandleTextDataBackspace);
        }

        #endregion Public Constructors

        #region Public Methods

        public void UpdatePauseImmuneOnly(float dt)
        {
            ExecuteCommands();
        }

        public void Update(float dt)
        {
            ExecuteCommands();
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

        private bool HandleTextDataInsert(TextDataInsert cmd)
        {
            var toModify = entityMan.GetById(cmd.EntityId);
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

        private bool HandleTextDataBackspace(TextDataBackspace cmd)
        {
            var toModify = entityMan.GetById(cmd.EntityId);
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

        private bool HandleTextCaretSetPosition(TextCaretSetPosition cmd)
        {
            var toModify = entityMan.GetById(cmd.EntityId);
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