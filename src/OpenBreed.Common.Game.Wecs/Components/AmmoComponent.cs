using OpenBreed.Wecs.Components;

namespace OpenBreed.Common.Game.Wecs.Components
{
    public interface IAmmoComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        int MagazinesCount { get; }
        int MaximumRoundsCount { get; }
        int RoundsCount { get; }

        #endregion Public Properties
    }

    public class AmmoComponent : IEntityComponent
    {
        #region Public Constructors

        public AmmoComponent(
            int maximumRoundsCount,
            int roundsCount,
            int magazinesCount)
        {
            MaximumRoundsCount = maximumRoundsCount;
            RoundsCount = roundsCount;
            MagazinesCount = magazinesCount;
        }

        #endregion Public Constructors

        #region Public Properties

        public int MagazinesCount { get; set; }
        public int MaximumRoundsCount { get; set; }
        public int RoundsCount { get; set; }

        #endregion Public Properties

        #region Public Methods

        public float GetRoundsPercent() => RoundsCount / (float)MaximumRoundsCount;

        #endregion Public Methods
    }

    public sealed class AmmoComponentFactory : ComponentFactoryBase<IAmmoComponentTemplate>
    {
        #region Public Constructors

        public AmmoComponentFactory()
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IAmmoComponentTemplate template)
        {
            return new AmmoComponent(
                template.MaximumRoundsCount,
                template.RoundsCount,
                template.MagazinesCount);
        }

        #endregion Protected Methods
    }
}