using OpenBreed.Common.Model.EntityTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Builders.EntityTemplates
{
    public class EntityTemplateBuilder
    {
        internal string EntityTemplate;

        public static EntityTemplateBuilder NewEntityTemplateModel()
        {
            return new EntityTemplateBuilder();
        }
        public EntityTemplateModel Build()
        {
            return new EntityTemplateModel(this);
        }

        public void SetEntityTemplate(string entityTemplate)
        {
            EntityTemplate = entityTemplate;
        }
    }
}
