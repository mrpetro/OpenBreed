using OpenBreed.Wecs.Components;

namespace OpenBreed.Wecs.Entities
{
    public interface IEntityFactory
    {
        #region Public Methods

        ITemplateEntityBuilder Create(string entityTemplateName);

        #endregion Public Methods
    }
}