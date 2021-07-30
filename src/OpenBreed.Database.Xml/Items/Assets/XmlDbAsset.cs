using OpenBreed.Common;
using OpenBreed.Common.Formats;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Database.Xml.Items.Assets
{
    [Serializable]
    public class XmlDbAsset : XmlDbEntry, IDbAsset
    {
        #region Public Properties

        [XmlAttribute("DataSourceRef")]
        public string DataSourceRef { get; set; }

        [XmlAttribute("FormatType")]
        public string FormatType { get; set; }

        [XmlIgnore]
        public List<FormatParameter> Parameters
        {
            get
            {
                return ToParameters();
            }
        }

        [XmlArray("Parameters")]
        [XmlArrayItem(ElementName = "Parameter")]
        public List<XmlFormatParameter> ParameterDefs { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IDbEntry Copy()
        {
            return new XmlDbAsset()
            {
                Id = this.Id,
                DataSourceRef = this.DataSourceRef
            };
        }

        #endregion Public Methods

        #region Private Methods

        private List<XmlFormatParameter> FromParameters()
        {
            var parameterDefs = new List<XmlFormatParameter>();

            foreach (var parameter in Parameters)
            {
                if (string.IsNullOrWhiteSpace(parameter.Name) ||
                    parameter.Type == null)
                    continue;

                if (parameterDefs.Any(item => item.Name == parameter.Name))
                    continue;

                var parameterDef = new XmlFormatParameter()
                {
                    Name = parameter.Name,
                    Type = parameter.Type.ToString(),
                    Value = parameter.Value.ToString()
                };
                parameterDefs.Add(parameterDef);
            }

            return parameterDefs;
        }

        private List<FormatParameter> ToParameters()
        {
            var parameters = new List<FormatParameter>();

            foreach (var parameterDef in ParameterDefs)
            {
                if (string.IsNullOrWhiteSpace(parameterDef.Name) ||
                    string.IsNullOrWhiteSpace(parameterDef.Type))
                    continue;

                if (parameters.Any(item => item.Name == parameterDef.Name))
                    continue;

                var type = Type.GetType(parameterDef.Type);
                var tc = TypeDescriptor.GetConverter(type);
                var value = tc.ConvertFromString(null, CultureInfo.InvariantCulture, parameterDef.Value);
                var parameter = new FormatParameter(parameterDef.Name, type, value);
                parameters.Add(parameter);
            }

            return parameters;
        }

        #endregion Private Methods
    }
}