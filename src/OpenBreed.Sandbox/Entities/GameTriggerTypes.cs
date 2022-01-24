using OpenBreed.Core.Managers;

namespace OpenBreed.Sandbox.Entities
{
    public static class GameTriggerTypes
    {
        #region Public Properties

        public static int HeroPickedItem { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static void RegisterGameTriggers(this ITriggerMan triggerMan)
        {
            HeroPickedItem = triggerMan.Register("Game/Hero/Picked");
        }

        #endregion Public Methods
    }
}