using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Xml
{
    public class XmlComponentTemplate : IComponentTemplate
    {
        #region Internal Fields

        internal static readonly Dictionary<string, Type> componentTypeLookup = new Dictionary<string, Type>();

        #endregion Internal Fields

        #region Public Constructors

        public XmlComponentTemplate(string componentName)
        {
            ComponentName = componentName;
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected XmlComponentTemplate()
        {
        }

        #endregion Protected Constructors

        #region Public Properties

        public string ComponentName { get; }

        #endregion Public Properties

        #region Public Methods

        public virtual IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            if (!componentTypeLookup.TryGetValue(ComponentName, out var componentType))
            {
                return null;
            }

            return (IEntityComponent)Activator.CreateInstance(componentType);
        }

        #endregion Public Methods

        #region Internal Methods

        internal static void RegisterComponentType(Type componentType)
        {
            var componentNameAttribute = componentType.GetCustomAttributes(typeof(ComponentNameAttribute), true).FirstOrDefault() as ComponentNameAttribute;

            var name = componentNameAttribute?.Name ?? componentType.Name;

            componentTypeLookup.Add(name, componentType);
        }

        #endregion Internal Methods
    }
}