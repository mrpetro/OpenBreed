using OpenBreed.Wecs.Components;

namespace OpenBreed.Wecs.Entities
{
    public interface IEntityFactory
    {
        #region Public Methods

        void RegisterComponentFactory<T>(IComponentFactory factory) where T : IComponentTemplate;

        ITemplateEntityBuilder Create(string entityTemplateName);

        Entity Create(IEntityTemplate template);

        #endregion Public Methods
    }
}