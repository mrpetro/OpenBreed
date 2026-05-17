using OpenBreed.Wecs.Core.Components.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using OpenBreed.Wecs.Components.Xml;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace OpenBreed.Fsm.Xml
{
    public class XmlMachineStateTemplate : IMachineStateTemplate
    {
        #region Public Properties

        [XmlAttribute("FsmName")]
        public string FsmName { get; set; }

        [XmlAttribute("StateName")]
        public string StateName { get; set; }

        #endregion Public Properties
    }

    [XmlRoot("Fsm")]
    public class XmlFsmComponent : XmlComponentTemplate, IFsmComponentTemplate
    {
        #region Public Properties

        [XmlIgnore]
        public IEnumerable<IMachineStateTemplate> States => XmlStates.Cast<IMachineStateTemplate>();

        [XmlArray("States")]
        [XmlArrayItem(ElementName = "State")]
        public XmlMachineStateTemplate[] XmlStates { get; set; }

        #endregion Public Properties

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            var fsmMan = serviceProvider.GetRequiredService<IFsmMan>();

            var fsmComponentBuilder = new FsmComponentBuilder(fsmMan);

            foreach (var state in States)
                fsmComponentBuilder.AddState(state.FsmName, state.StateName);

            return fsmComponentBuilder.Build();
        }



    }
}