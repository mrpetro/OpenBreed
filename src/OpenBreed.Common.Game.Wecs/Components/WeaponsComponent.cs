using OpenBreed.Wecs.Components;

namespace OpenBreed.Common.Game.Wecs.Components
{
    public interface IWeaponsComponentTemplate : IComponentTemplate
    {
    }

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

        public int CurrentWeaponNo { get; set; }
        public int CurrentWeaponState { get; set; }
        public bool FireReady { get; set; } = true;

        #endregion Public Properties
    }
}