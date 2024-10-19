using OpenBreed.Wecs.Components;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.ComponentModel;

namespace OpenBreed.Common.Game.Wecs.Components
{
    public class PlayerInputsComponent : IEntityComponent
    {
        #region Public Properties

        public Keys Up { get; set; }
        public Keys Down { get; set; }
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public Keys Fire { get; set; }
        public Keys SwitchWeapon { get; set; }

        #endregion Public Properties
    }
}