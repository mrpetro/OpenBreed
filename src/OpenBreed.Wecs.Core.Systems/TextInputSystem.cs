using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Systems.Categories;

namespace OpenBreed.Wecs.Core.Systems
{
    [RequireEntityWith(
        typeof(TextCaretComponent),
        typeof(TextDataComponent))]
    [SystemCategory(CommonCategories.General)]
    public class TextInputSystem : IUpdatableSystem
    {
        #region Private Fields

        private readonly IWorldMan worldMan;

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public TextInputSystem(
            IWorldMan worldMan,
            IEntityMan entityMan,
            IEventsMan eventsMan)
        {
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
            this.entityMan = entityMan ?? throw new System.ArgumentNullException(nameof(entityMan));
            this.eventsMan = eventsMan ?? throw new System.ArgumentNullException(nameof(eventsMan));
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(IUpdateContext context)
        {
            var world = worldMan.GetById(context.WorldId);

            var entities = world.GetMatchingEntities(this);

            foreach (var entity in entities)
            {
                UpdateEntity(entity, context);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void UpdateEntity(IEntity entity, IUpdateContext context)
        {
        }

        #endregion Private Methods

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

        //    eventsMan.Raise(null, new TextDataChanged(textCmp.Data, TextDataChanged.ChangeType.Inserted, caretCmp.Position - cmd.Text.Length, cmd.Text.Length));
        //    eventsMan.Raise(null, new TextCaretPositionChanged(caretCmp.Position));

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

        //    eventsMan.Raise(null, new TextDataChanged(textCmp.Data, TextDataChanged.ChangeType.Removed, caretCmp.Position + 1, 1));
        //    eventsMan.Raise(null, new TextCaretPositionChanged(caretCmp.Position));

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
        //    eventsMan.Raise(null, new TextCaretPositionChanged(caretCmp.Position));

        //    return true;
        //}
    }
}