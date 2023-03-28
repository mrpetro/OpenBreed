using OpenBreed.Wecs.Components;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace OpenBreed.Sandbox.Wecs.Components
{
    public interface IDamagerComponentTemplate : IComponentTemplate
    {
    }

    public class DamagerComponent : IEntityComponent
    {
        #region Public Properties

        /// <summary>
        /// Damage distributions
        /// </summary>
        public List<DamageDistribution> Distributions { get; } = new List<DamageDistribution>();

        #endregion Public Properties
    }

    public sealed class DamagerComponentFactory : ComponentFactoryBase<IDamagerComponentTemplate>
    {
        #region Public Constructors

        public DamagerComponentFactory()
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IDamagerComponentTemplate template)
        {
            return new DamagerComponent();
        }

        #endregion Protected Methods
    }

    public class DamageDistribution
    {
        #region Public Constructors

        public DamageDistribution(
            int amount,
            ICollection<int> targets)
        {
            Amount = amount;
            Targets = targets.ToImmutableArray();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Amount of damage to inflict
        /// </summary>
        public int Amount { get; }

        /// <summary>
        /// IDs of entites which suposed to be damaged
        /// </summary>
        public ImmutableArray<int> Targets { get; }

        #endregion Public Properties
    }
}