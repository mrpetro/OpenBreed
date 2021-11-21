using OpenBreed.Sandbox.Managers;

namespace OpenBreed.Sandbox.Entities
{
    public static class ItemTypes
    {
        #region Public Properties

        public static int KeyCardStandard { get; private set; }
        public static int KeyCardRed { get; private set; }
        public static int KeyCardGreen { get; private set; }
        public static int KeyCardBlue { get; private set; }
        public static int Ammo { get; private set; }
        public static int CreditsSmall { get; private set; }
        public static int CreditsBig { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static void RegisterAbtaItems(this ItemsMan itemsMan)
        {
            KeyCardStandard = itemsMan.RegisterItemType("KeyCardStandard");
            KeyCardRed = itemsMan.RegisterItemType("KeyCardRed");
            KeyCardGreen = itemsMan.RegisterItemType("KeyCardGreen");
            KeyCardBlue = itemsMan.RegisterItemType("KeyCardBlue");
            Ammo = itemsMan.RegisterItemType("Ammo");
            CreditsSmall = itemsMan.RegisterItemType("CreditsSmall");
            CreditsBig = itemsMan.RegisterItemType("CreditsBig");
        }

        #endregion Public Methods
    }
}