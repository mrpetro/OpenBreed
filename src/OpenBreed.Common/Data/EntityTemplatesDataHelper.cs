using OpenBreed.Common.Model.Maps;
using OpenBreed.Common.Model.Maps.Blocks;
using OpenBreed.Common.Model.EntityTemplates;
using OpenBreed.Common.Builders.EntityTemplates;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    internal class EntityTemplatesDataHelper
    {
        internal static EntityTemplateModel FromText(DataProvider provider, IEntityTemplateFromFileEntry entry)
        {
            if (entry.DataRef == null)
                return null;

            var data = provider.GetData(entry.DataRef) as EntityTemplateModel;

            if (data == null)
                return null;

            var builder = EntityTemplateBuilder.NewEntityTemplateModel();
            builder.SetEntityTemplate(data.EntityTemplate);
            return builder.Build();
        }
    }
}
