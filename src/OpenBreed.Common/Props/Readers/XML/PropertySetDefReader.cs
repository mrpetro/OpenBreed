using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Props.Builders;
using OpenBreed.Common.Database.Items.Props;

namespace OpenBreed.Common.Props.Readers.XML
{
    public class PropertySetDefReader
    {
        private readonly PropertySetBuilder m_Builder;

        public PropertySetDefReader(PropertySetBuilder builder)
        {
            m_Builder = builder;
        }

        public PropertySetModel Read(PropertySetDef propertySetDef)
        {
            PropertyBuilder propertyBuilder = PropertyBuilder.NewProperty();

            propertyBuilder.SetName(propertySetDef.Name);

            foreach (var propertyDef in propertySetDef.PropertyDefs)
            {
                propertyBuilder.SetId(propertyDef.Id);
                propertyBuilder.SetVisibility(propertyDef.Visibility);
                propertyBuilder.SetDescription(propertyDef.Description);
                propertyBuilder.SetName(propertyDef.Name);
                propertyBuilder.SetPresentation(propertyDef.ImagePath, System.Drawing.ColorTranslator.FromHtml(propertyDef.Color));

                m_Builder.AddProperty(propertyBuilder.Build());
            }

            return m_Builder.Build();
        }

        public PropertySetModel Read(Stream stream)
        {
            var propertySetDef = Tools.RestoreFromXml<PropertySetDef>(stream);

            return Read(propertySetDef);
        }
    }
}
