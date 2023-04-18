using OpenBreed.Wecs.Components;

namespace OpenBreed.Sandbox.Wecs.Components
{
    public class WeaponsComponent : IEntityComponent
    {
        #region Public Constructors

        public WeaponsComponent()
        {
            //Slots = slots;
        }

        #endregion Public Constructors

        #region Public Properties

        public EquipmentSlot[] Slots { get; }

        #endregion Public Properties
    }
}