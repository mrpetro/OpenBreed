using OpenBreed.Components.Common;

namespace OpenBreed.Components.Control
{
    /// <summary>
    /// Placeholder class for attack control
    /// </summary>
    public class AttackControl : IControlComponent
    {
        #region Public Fields

        public const string TYPE = "Attack";

        #endregion Public Fields

        #region Public Properties

        public bool AttackPrimary { get; set; }
        public bool AttackSecondary { get; set; }

        public string Type => TYPE;

        #endregion Public Properties
    }
}