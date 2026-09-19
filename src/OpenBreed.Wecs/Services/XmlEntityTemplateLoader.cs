using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OpenBreed.Common.Tools.Xml;
using OpenBreed.Wecs.Entities.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace OpenBreed.Wecs.Services
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
        private readonly IMemoryCache cache;

        #endregion Private Fields

        #region Public Constructors

        public XmlEntityTemplateLoader(IOptions<XmlEntityTemplateLoaderSettings> options, IMemoryCache cache)
        {
            dataDirPath = options.Value.DataDirPath;
            this.cache = cache;
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntityTemplate Load(string templateName, Dictionary<string, string> variables)
        {
            var xmlDocument = LoadXmlDocument(templateName);
            //var xmlDocument = GetXmlDocument(templateName);

            return XmlHelper.RestoreFromXml<XmlEntityTemplate>(xmlDocument, variables);
        }

        public XmlDocument GetXmlDocument(string templateName)
        {
            return cache.GetOrCreate(
                templateName,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(10);

                    return LoadXmlDocument(templateName);
                });
        }

        private XmlDocument LoadXmlDocument(string templateName)
        {
            var templatePath = Path.Combine(dataDirPath, $"{templateName}.xml");

            return XmlHelper.RestoreFromXml(templatePath);
        }

        #endregion Public Methods
    }
}