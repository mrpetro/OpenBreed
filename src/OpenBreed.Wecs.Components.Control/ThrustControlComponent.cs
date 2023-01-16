using System.ComponentModel;

namespace OpenBreed.Wecs.Components.Control
{
    public class ThrustControlComponent : IEntityComponent
    {
        #region Public Properties

        public int UpCode { get; set; }
        public int DownCode { get; set; }
        public int LeftCode { get; set; }
        public int RightCode { get; set; }

        #endregion Public Properties
    }
}