using System.Collections.Generic;

namespace OpenBreed.Wecs.Entities
{
    public interface IEntityTemplateLoader
    {
        #region Public Methods

        IEntityTemplate Load(string templateName, Dictionary<string, string> variables);

        #endregion Public Methods
    }
}