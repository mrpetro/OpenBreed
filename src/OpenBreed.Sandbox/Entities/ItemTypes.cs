using OpenBreed.Sandbox.Managers;

namespace OpenBreed.Sandbox.Entities
{
    public static class ItemTypes
    {
        #region Public Properties

        public static int KeycardStandard { get; private set; }
        public static int KeycardSpecial { get; private set; }
        public static int KeycardRed { get; private set; }
        public static int KeycardGreen { get; private set; }
        public static int KeycardBlue { get; private set; }
        public static int Ammo { get; private set; }
        public static int CreditsSmall { get; private set; }
        public static int CreditsBig { get; private set; }
        public static int SmartCard1 { get; private set; }
        public static int SmartCard2 { get; private set; }
        public static int SmartCard3 { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static void RegisterAbtaItems(this ItemsMan itemsMan)
        {
            KeycardStandard = itemsMan.RegisterItemType("KeycardStandard");
            KeycardRed = itemsMan.RegisterItemType("KeycardRed");
            KeycardGreen = itemsMan.RegisterItemType("KeycardGreen");
            KeycardBlue = itemsMan.RegisterItemType("KeycardBlue");
            Ammo = itemsMan.RegisterItemType("Ammo");
            CreditsSmall = itemsMan.RegisterItemType("CreditsSmall");
            CreditsBig = itemsMan.RegisterItemType("CreditsBig");
            SmartCard1 = itemsMan.RegisterItemType("SmartCard1");
            SmartCard2 = itemsMan.RegisterItemType("SmartCard2");
            SmartCard3 = itemsMan.RegisterItemType("SmartCard3");



            KeycardSpecial = itemsMan.RegisterItemType("KeycardSpecial");
        }

        #endregion Public Methods
    }
}