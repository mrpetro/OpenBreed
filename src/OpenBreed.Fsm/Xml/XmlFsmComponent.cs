using OpenBreed.Wecs.Core.Components.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using OpenBreed.Wecs.Components.Xml;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime;

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

    public class XmlFsmStateTemplate : IFsmStateTemplate
    {
        #region Public Properties

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Value")]
        public string Value { get; set; }

        #endregion Public Properties
    }

    [XmlRoot("Fsm")]
    public class XmlFsmComponent : XmlComponentTemplate, IFsmComponentTemplate
    {
        #region Public Properties

        [XmlIgnore]
        public IEnumerable<IMachineStateTemplate> States => XmlStates ?? Array.Empty<IMachineStateTemplate>();

        [XmlIgnore]
        public IEnumerable<IFsmStateTemplate> ExStates => XmlExStates ?? Array.Empty<IFsmStateTemplate>();

        [XmlArray("States")]
        [XmlArrayItem(ElementName = "State")]
        public XmlMachineStateTemplate[] XmlStates { get; set; }

        [XmlArray("ExStates")]
        [XmlArrayItem(ElementName = "ExState")]
        public XmlFsmStateTemplate[] XmlExStates { get; set; }

        #endregion Public Properties

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            var fsmMan = serviceProvider.GetRequiredService<IFsmMan>();

            var fsmComponentBuilder = new FsmComponentBuilder(fsmMan);

            foreach (var state in States)
                fsmComponentBuilder.AddState(state.FsmName, state.StateName);

            foreach (var exState in ExStates)
            {
                if (!TryGetEnum(exState, out var result))
                {
                    continue;
                }

                fsmComponentBuilder.SetState(result);
            }

            return fsmComponentBuilder.Build();
        }

        private bool TryGetEnum(IFsmStateTemplate state, out Enum result)
        {
            var enumType = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t =>
                    t.IsEnum &&
                    t.Name.Equals(state.Name, StringComparison.OrdinalIgnoreCase));

            if (enumType is null)
            {
                result = null;
                return false;
            }

            if (!Enum.TryParse(enumType, state.Value, ignoreCase: true, out object enumObj))
            {
                result = null;
                return false;
            }

            result = (Enum)enumObj;
            return true;
        }
    }
}