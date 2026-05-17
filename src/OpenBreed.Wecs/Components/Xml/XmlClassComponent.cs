using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Wecs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Xml
{
    [XmlRoot("Class")]
    public class XmlClassComponent : XmlComponentTemplate, IClassComponentTemplate
    {
        #region Public Properties

        [XmlElement("Name")]
        public string Name { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            var entityClassMan = serviceProvider.GetRequiredService<IEntityClassMan>();

            var entityClass = entityClassMan.GetByName(Name);
            return new ClassComponent(entityClass.Id);
        }

        #endregion Public Methods
    }
}