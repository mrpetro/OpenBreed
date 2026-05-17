using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Physics.Components.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Physics.Components.Xml
{
    public class XmlBodyFixtureTemplate : IBodyFixtureTemplate
    {
        #region Public Properties

        [XmlElement("ShapeName")]
        public string ShapeName { get; set; }

        [XmlIgnore]
        public IEnumerable<string> Groups => XmlGroups.Cast<string>();

        [XmlArray("Groups")]
        [XmlArrayItem(ElementName = "Group")]
        public string[] XmlGroups { get; set; }

        #endregion Public Properties
    }

    [XmlRoot("Body")]
    public class XmlBodyComponent : XmlComponentTemplate, IBodyComponentTemplate
    {
        #region Public Properties

        [XmlElement("CofFactor")]
        public float CofFactor { get; set; }

        [XmlElement("CorFactor")]
        public float CorFactor { get; set; }

        [XmlIgnore]
        public IEnumerable<IBodyFixtureTemplate> Fixtures => XmlFixtures.Cast<IBodyFixtureTemplate>();

        [XmlArray("Fixtures")]
        [XmlArrayItem(ElementName = "Fixture")]
        public XmlBodyFixtureTemplate[] XmlFixtures { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            var builderFactory = serviceProvider.GetRequiredService<IBuilderFactory>();

            var bodyComponentBuilder = builderFactory.GetBuilder<BodyComponentBuilder>();

            var fixtureBuilder = builderFactory.GetBuilder<BodyFixtureBuilder>();

            bodyComponentBuilder.SetCofFactor(CofFactor);
            bodyComponentBuilder.SetCorFactor(CorFactor);

            foreach (var fixture in Fixtures)
            {
                fixtureBuilder.ClearGroups();

                fixtureBuilder.SetShape(fixture.ShapeName);

                foreach (var groupName in fixture.Groups)
                {
                    fixtureBuilder.AddGroup(groupName);
                }

                bodyComponentBuilder.AddFixture(fixtureBuilder.Build());
            }

            return bodyComponentBuilder.Build();
        }

        #endregion Public Methods
    }
}