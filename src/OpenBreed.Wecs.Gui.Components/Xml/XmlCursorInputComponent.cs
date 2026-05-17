using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Gui.Components.Xml
{
    [XmlRoot("Cursor")]
    public class XmlCursorInputComponent : XmlComponentTemplate, ICursorInputComponentTemplate
    {
        //[XmlArray("Actions")]
        //[XmlArrayItem(ElementName = "Action")]
        //public ICursorAction[] Actions { get; set; }

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            var actions = new List<int>();

            //foreach (var action in template.Actions)
            //{
            //    if (actionCodeProvider.TryGetCode(action.Type, action.Name, out int code))
            //        actions.Add(code);
            //}

            return new CursorInputComponent(actions);
        }

        #endregion Public Methods
    }
}