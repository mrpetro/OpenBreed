using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Control.Components.Xml
{
    [XmlRoot("Animation")]
    public class XmlAnimationComponent : XmlComponentTemplate, IAnimationComponentTemplate
    {
        #region Public Properties

        [XmlArray("States")]
        [XmlArrayItem(ElementName = "State")]
        public List<XmlAnimationState> XmlStates { get; set; }

        [XmlIgnore]
        public ReadOnlyCollection<IAnimationStateTemplate> States
        {
            get
            {
                return new ReadOnlyCollection<IAnimationStateTemplate>(XmlStates.Cast<IAnimationStateTemplate>().ToList());
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            var builderFactory = serviceProvider.GetRequiredService<IBuilderFactory>();

            var builder = builderFactory.GetBuilder<AnimationComponentBuilder>();

            foreach (var stateTemplate in States)
            {
                var stateBuilder = builder.AddState();

                if (!string.IsNullOrEmpty(stateTemplate.ClipName))
                    stateBuilder.SetClipByName(stateTemplate.ClipName);

                stateBuilder.SetLoop(stateTemplate.Loop);
                stateBuilder.SetSpeed(stateTemplate.Speed);
            }

            return builder.Build();
        }

        #endregion Public Methods
    }

    public class XmlAnimationState : IAnimationStateTemplate
    {
        #region Public Properties

        [XmlElement("Speed")]
        public float Speed { get; set; } = 1;

        [XmlElement("Loop")]
        public bool Loop { get; set; }

        [XmlElement("ClipName")]
        public string ClipName { get; set; }

        #endregion Public Properties
    }
}