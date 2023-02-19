using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Control
{
    public class ControlMapping
    {
        public ControlMapping(int code, string action)
        {
            Code = code;
            Action = action;
        }

        public int Code { get; }
        public string Action { get; }
    }

    public class ActionControlComponent : IEntityComponent
    {
        #region Public Constructors

        public ActionControlComponent()
        {
            Mappings = new List<ControlMapping>();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<ControlMapping> Mappings { get; }

        #endregion Public Properties
    }
}