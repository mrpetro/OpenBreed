namespace OpenBreed.Wecs.Abstractions.Primitives
{
    public interface ITemplateEntityBuilder : IEntityBuilder
    {
        #region Public Methods

        ITemplateEntityBuilder SetParameter<TValue>(string parameterName, TValue parameterValue);

        #endregion Public Methods
    }
}