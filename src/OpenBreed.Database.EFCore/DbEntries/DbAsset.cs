using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.EFCore.DbEntries
{
    [ComplexType]
    public class AssetFormatParameter
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }

    public class DbAsset : DbEntry, IDbAsset
    {
        public DbAsset()
        { }

        protected DbAsset(DbAsset other)
            : base(other)
        {
            DataSourceRef = other.DataSourceRef;
            FormatType = other.FormatType;

        }

        public string DataSourceRef { get; set; }

        public string FormatType { get; set; }

        [Column("Parameters")]
        public List<AssetFormatParameter> AssetParameters { get; set; }

        [NotMapped]
        public List<FormatParameter> Parameters
        {
            get
            {
                return ToParameters();
            }
        }

        public override IDbEntry Copy()
        {
            return new DbAsset(this);
        }


        public static List<AssetFormatParameter> FromParameters(List<FormatParameter> parameters)
        {
            var assetParameters = new List<AssetFormatParameter>();

            foreach (var parameter in parameters)
            {
                if (string.IsNullOrWhiteSpace(parameter.Name) ||
                    parameter.Type == null)
                    continue;

                if (assetParameters.Any(item => item.Name == parameter.Name))
                    continue;

                var assetParameter = new AssetFormatParameter()
                {
                    Name = parameter.Name,
                    Type = parameter.Type.ToString(),
                    Value = parameter.Value.ToString()
                };
                assetParameters.Add(assetParameter);
            }

            return assetParameters;
        }

        private List<FormatParameter> ToParameters()
        {
            var parameters = new List<FormatParameter>();

            foreach (var parameterDef in AssetParameters)
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
    }
}
