namespace OpenBreed.Wecs.Abstractions.Services
{
    public interface IEntityFactory
    {
        #region Public Methods

        ITemplateEntityBuilder Create(string entityTemplateName);

        #endregion Public Methods
    }
}