using OpenBreed.Wecs.Components;

namespace OpenBreed.Common.Game.Wecs.Components
{
    public interface IWeaponsComponentTemplate : IComponentTemplate
    {
    }

    public sealed class WeaponsComponentFactory : ComponentFactoryBase<IWeaponsComponentTemplate>
    {
        #region Public Constructors

        public WeaponsComponentFactory()
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IWeaponsComponentTemplate template)
        {
            return new WeaponsComponent();
        }

        #endregion Protected Methods
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