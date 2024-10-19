using OpenBreed.Common.Game.Managers;

namespace OpenBreed.Common.Game
{
    public static class ItemTypes
    {
        #region Public Properties

        public static int KeycardStandard { get; private set; }
        public static int KeycardSpecial { get; private set; }
        public static int Keycard1 { get; private set; }
        public static int Keycard2 { get; private set; }
        public static int Keycard3 { get; private set; }
        public static int Ammo { get; private set; }
        public static int CreditsSmall { get; private set; }
        public static int CreditsBig { get; private set; }
        public static int MedkitSmall { get; private set; }
        public static int MedkitBig { get; private set; }
        public static int SmartCard1 { get; private set; }
        public static int SmartCard2 { get; private set; }
        public static int SmartCard3 { get; private set; }
        public static int ExtraLife { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static void RegisterAbtaItems(this ItemsMan itemsMan)
        {
            KeycardStandard = itemsMan.RegisterItemType("KeycardStandard");
            Keycard1 = itemsMan.RegisterItemType("Keycard1");
            Keycard2 = itemsMan.RegisterItemType("Keycard2");
            Keycard3 = itemsMan.RegisterItemType("Keycard3");
            Ammo = itemsMan.RegisterItemType("Ammo");
            CreditsSmall = itemsMan.RegisterItemType("CreditsSmall");
            CreditsBig = itemsMan.RegisterItemType("CreditsBig");
            MedkitSmall = itemsMan.RegisterItemType("MedkitSmall");
            MedkitBig = itemsMan.RegisterItemType("MedkitBig");
            SmartCard1 = itemsMan.RegisterItemType("SmartCard1");
            SmartCard2 = itemsMan.RegisterItemType("SmartCard2");
            SmartCard3 = itemsMan.RegisterItemType("SmartCard3");
            ExtraLife = itemsMan.RegisterItemType("ExtraLife");

            KeycardSpecial = itemsMan.RegisterItemType("KeycardSpecial");
        }

        #endregion Public Methods
    }
}