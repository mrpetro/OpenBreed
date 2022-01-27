using OpenBreed.Core.Managers;

namespace OpenBreed.Sandbox.Entities
{
    public static class GameEventTypes
    {
        #region Public Properties

        public static int HeroPickedItem { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static void RegisterGameEvents(this IEventsManEx triggerMan)
        {
            HeroPickedItem = triggerMan.Register("Game/Hero/Picked");
        }

        #endregion Public Methods
    }
}