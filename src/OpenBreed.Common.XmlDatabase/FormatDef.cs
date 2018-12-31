using OpenBreed.Common.XmlDatabase.Items.Sources;
using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Common.XmlDatabase
{
    public class FormatDef : IFormatEntity
    {
        #region Public Fields

        [XmlArray("Parameters")]
        [XmlArrayItem(ElementName = "Parameter")]
        public List<FormatParameterDef> ParameterDefs { get; set; }

        private List<FormatParameterDef> FromParameters()
        {
            var parameterDefs = new List<FormatParameterDef>();

            foreach (var parameter in Parameters)
            {
                if (string.IsNullOrWhiteSpace(parameter.Name) ||
                    parameter.Type == null)
                    continue;

                if (parameterDefs.Any(item => item.Name == parameter.Name))
                    continue;

                var parameterDef = new FormatParameterDef() { Name = parameter.Name,
                                                              Type = parameter.Type.ToString(),
                                                              Value =parameter.Value.ToString() };
                parameterDefs.Add(parameterDef);
            }

            return parameterDefs;
        }

        #endregion Public Fields

        #region Public Properties

        [XmlAttribute]
        public string Name { get; set; }

        [XmlIgnore]
        public List<FormatParameter> Parameters
        {
            get
            {
                return ToParameters();
            }
        }

        #endregion Public Properties

        #region Private Methods

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
