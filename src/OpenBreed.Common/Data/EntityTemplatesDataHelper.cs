using OpenBreed.Model.Maps;
using OpenBreed.Model.EntityTemplates;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Model.Texts;

namespace OpenBreed.Common.Data
{
    internal class EntityTemplatesDataHelper
    {
        internal static EntityTemplateModel FromText(IModelsProvider provider, IDbEntityTemplateFromFile entry)
        {
            if (entry.DataRef == null)
                return null;

            var data = provider.GetModel<TextModel>(entry.DataRef);

            if (data == null)
                return null;

            var builder = EntityTemplateBuilder.NewEntityTemplateModel();
            builder.SetEntityTemplate(data.Text);
            return builder.Build();
        }
    }
}
