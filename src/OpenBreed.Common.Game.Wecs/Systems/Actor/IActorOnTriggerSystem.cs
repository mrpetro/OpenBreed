using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;

namespace OpenBreed.Common.Game.Wecs.Systems.Actor
{
    /// <summary>
    /// System for callback action when an actor triggered another entity
    /// </summary>
    public interface IActorOnTriggerSystem : ISystem
    {
        #region Public Properties

        /// <summary>
        /// Name of callback action
        /// </summary>
        string ActionName { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Called when actor triggered another entity.
        /// </summary>
        /// <param name="actorEntity">Actor that triggered another entity.</param>
        /// <param name="triggerEntity">Entity that is triggered.</param>
        void OnTrigger(IEntity actorEntity, IEntity triggerEntity);

        #endregion Public Methods
    }
}