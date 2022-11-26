using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Core
{
    public class TextInputSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Internal Constructors

        internal TextInputSystem(IEntityMan entityMan)
        {
            this.entityMan = entityMan;

            RequireEntityWith<TextCaretComponent>();
            RequireEntityWith<TextDataComponent>();
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
        }

        #endregion Protected Methods

        //private bool HandleTextDataInsert(TextDataInsert cmd)
        //{
        //    var toModify = entityMan.GetById(cmd.EntityId);
        //    if (toModify == null)
        //        return false;

        //    var caretCmp = toModify.Get<TextCaretComponent>();
        //    var textCmp = toModify.Get<TextDataComponent>();

        //    if (string.IsNullOrEmpty(cmd.Text))
        //        return true;

        //    textCmp.Insert(caretCmp.Position, cmd.Text);
        //    caretCmp.Position += cmd.Text.Length;

        //    toModify.RaiseEvent(new TextDataChanged(textCmp.Data, TextDataChanged.ChangeType.Inserted, caretCmp.Position - cmd.Text.Length, cmd.Text.Length));
        //    toModify.RaiseEvent(new TextCaretPositionChanged(caretCmp.Position));

        //    return true;
        //}

        //private bool HandleTextDataBackspace(TextDataBackspace cmd)
        //{
        //    var toModify = entityMan.GetById(cmd.EntityId);
        //    if (toModify == null)
        //        return false;

        //    var caretCmp = toModify.Get<TextCaretComponent>();
        //    var textCmp = toModify.Get<TextDataComponent>();

        //    if (string.IsNullOrEmpty(textCmp.Data))
        //        return true;

        //    if (caretCmp.Position == 0)
        //        return true;

        //    textCmp.Remove(caretCmp.Position - 1, 1);
        //    caretCmp.Position -= 1;

        //    toModify.RaiseEvent(new TextDataChanged(textCmp.Data, TextDataChanged.ChangeType.Removed, caretCmp.Position + 1, 1));
        //    toModify.RaiseEvent(new TextCaretPositionChanged(caretCmp.Position));

        //    return true;
        //}

        //private bool HandleTextCaretSetPosition(TextCaretSetPosition cmd)
        //{
        //    var toModify = entityMan.GetById(cmd.EntityId);
        //    if (toModify == null)
        //        return false;

        //    var caretCmp = toModify.Get<TextCaretComponent>();
        //    var textCmp = toModify.Get<TextDataComponent>();

        //    if (textCmp.Data.Length == 0)
        //        return true;

        //    var newPosition = cmd.NewPosition;

        //    if (newPosition < 0)
        //        newPosition = 0;
        //    else if (newPosition > textCmp.Data.Length)
        //        newPosition = textCmp.Data.Length;

        //    // Nothing to do here
        //    if (newPosition == caretCmp.Position)
        //        return true;

        //    caretCmp.Position = cmd.NewPosition;
        //    toModify.RaiseEvent(new TextCaretPositionChanged(caretCmp.Position));

        //    return true;
        //}
    }
}