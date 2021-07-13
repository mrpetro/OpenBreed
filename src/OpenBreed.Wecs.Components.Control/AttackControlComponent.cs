using OpenBreed.Wecs.Components.Common;

namespace OpenBreed.Wecs.Components.Control
{
    /// <summary>
    /// Placeholder class for attack control
    /// </summary>
    public class AttackControlComponent : IControlComponent
    {
        #region Public Properties

        public bool AttackPrimary { get; set; }
        public bool AttackSecondary { get; set; }

        #endregion Public Properties
    }
}