using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Wecs.Systems.Rendering.Extensions
{
    public static class EntityExtensions
    {
        #region Public Methods

        public static void SetText(this Entity entity, int textPartId, string text)
        {
            var textCmp = entity.Get<TextComponent>();

            if (textPartId < 0 || textPartId >= textCmp.Parts.Count)
                return;

            textCmp.Parts[textPartId].Text = text;
        }

        #endregion Public Methods
    }
}