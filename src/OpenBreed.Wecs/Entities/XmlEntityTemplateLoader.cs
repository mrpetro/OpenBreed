using Microsoft.Extensions.Options;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Wecs.Entities.Xml;
using System.Collections.Generic;
using System.IO;

namespace OpenBreed.Wecs.Entities
{
    public class XmlEntityTemplateLoaderSettings
    {
        #region Public Properties

        public string DataDirPath { get; set; }

        #endregion Public Properties
    }

    internal class XmlEntityTemplateLoader : IEntityTemplateLoader
    {
        #region Private Fields

        private readonly string dataDirPath;

        #endregion Private Fields

        #region Public Constructors

        public XmlEntityTemplateLoader(IOptions<XmlEntityTemplateLoaderSettings> options)
        {
            this.dataDirPath = options.Value.DataDirPath;
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntityTemplate Load(string templateName, Dictionary<string, string> variables)
        {
            var templatePath = Path.Combine(dataDirPath, $"{templateName}.xml");

            return XmlHelper.RestoreFromXml<XmlEntityTemplate>(templatePath, variables);
        }

        #endregion Public Methods
    }
}