using OpenBreed.Wecs.Components;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace OpenBreed.Common.Game.Wecs.Components
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
        public List<DamageInfliction> Inflictions { get; } = new List<DamageInfliction>();

        #endregion Public Properties
    }

    public class DamageInfliction
    {
        #region Public Constructors

        public DamageInfliction(
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