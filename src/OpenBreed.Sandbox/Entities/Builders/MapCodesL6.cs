using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Builders
{
    public static class MapCodesL6
    {
        //GFX codes
        public const int GFX_MEDKIT_SMALL_F1 = 653;
        public const int GFX_MEDKIT_SMALL_F2 = 655;
        public const int GFX_CREDITS_SMALL_F1 = 600;
        public const int GFX_KEY_CARD_STANDARD_F1 = 602;
        public const int GFX_POWERUP_S_F1 = 603;
        public const int GFX_EXTRA_LIFE_F1 = 604;
        public const int GFX_SMART_CARD_F1 = 605;
        public const int GFX_CREDITS_BIG_F1 = 620;
        public const int GFX_POWERUP_F_F1 = 621;
        public const int GFX_MEDKIT_BIG_F1 = 622;
        public const int GFX_POWERUP_A_F1 = 623;
        public const int GFX_AMMO_F1 = 624;
        public const int GFX_AREA_SCANNER_F1 = 625;
        public const int GFX_CREDITS_SMALL_F2 = 606;
        public const int GFX_KEY_CARD_STANDARD_F2 = 608;
        public const int GFX_POWERUP_S_F2 = 609;
        public const int GFX_EXTRA_LIFE_F2 = 610;
        public const int GFX_SMART_CARD_F2 = 611;
        public const int GFX_CREDITS_BIG_F2 = 626;
        public const int GFX_POWERUP_F_F2 = 627;
        public const int GFX_MEDKIT_BIG_F2 = 628;
        public const int GFX_POWERUP_A_F2 = 629;
        public const int GFX_AMMO_F2 = 630;
        public const int GFX_AREA_SCANNER_F2 = 631;
        public const int GFX_CREDITS_SMALL_F3 = 612;
        public const int GFX_KEY_CARD_STANDARD_F3 = 614;
        public const int GFX_POWERUP_S_F3 = 615;
        public const int GFX_EXTRA_LIFE_F3 = 616;
        public const int GFX_SMART_CARD_F3 = 617;
        public const int GFX_CREDITS_BIG_F3 = 632;
        public const int GFX_POWERUP_F_F3 = 633;
        public const int GFX_MEDKIT_BIG_F3 = 634;
        public const int GFX_POWERUP_A_F3 = 635;
        public const int GFX_AMMO_F3 = 636;
        public const int GFX_AREA_SCANNER_F3 = 637;
        public const int GFX_KEYCARD_BLUE_F2 = 649;
        public const int GFX_KEYCARD_RED_F2 = 651;
        public const int GFX_KEYCARD_GREEN_F2 = 647;


        public static void SetupL6(this MapMapper mapper)
        {
            mapper.Register("GenericItem", GFX_MEDKIT_SMALL_F1, "MedkitSmall/F1");
            mapper.Register("GenericItem", GFX_MEDKIT_SMALL_F2, "MedkitSmall/F2");
            mapper.Register("GenericItem", GFX_CREDITS_SMALL_F1, "CreditsSmall/F1");
            mapper.Register("GenericItem", GFX_CREDITS_SMALL_F2, "CreditsSmall/F2");
            mapper.Register("GenericItem", GFX_CREDITS_SMALL_F3, "CreditsSmall/F3");
            mapper.Register("GenericItem", GFX_KEY_CARD_STANDARD_F1, "KeycardStandard/F1");
            mapper.Register("GenericItem", GFX_KEY_CARD_STANDARD_F2, "KeycardStandard/F2");
            mapper.Register("GenericItem", GFX_KEY_CARD_STANDARD_F3, "KeycardStandard/F3");
            mapper.Register("GenericItem", GFX_POWERUP_S_F1, "PowerUpS/F1");
            mapper.Register("GenericItem", GFX_POWERUP_S_F2, "PowerUpS/F2");
            mapper.Register("GenericItem", GFX_POWERUP_S_F3, "PowerUpS/F3");
            mapper.Register("GenericItem", GFX_EXTRA_LIFE_F1, "ExtraLife/F1");
            mapper.Register("GenericItem", GFX_EXTRA_LIFE_F2, "ExtraLife/F2");
            mapper.Register("GenericItem", GFX_EXTRA_LIFE_F3, "ExtraLife/F3");
            mapper.Register("GenericItem", GFX_CREDITS_BIG_F1, "CreditsBig/F1");
            mapper.Register("GenericItem", GFX_CREDITS_BIG_F2, "CreditsBig/F2");
            mapper.Register("GenericItem", GFX_CREDITS_BIG_F3, "CreditsBig/F3");
            mapper.Register("GenericItem", GFX_POWERUP_F_F1, "PowerUpF/F1");
            mapper.Register("GenericItem", GFX_POWERUP_F_F2, "PowerUpF/F2");
            mapper.Register("GenericItem", GFX_POWERUP_F_F3, "PowerUpF/F3");
            mapper.Register("GenericItem", GFX_MEDKIT_BIG_F1, "MedkitBig/F1");
            mapper.Register("GenericItem", GFX_MEDKIT_BIG_F2, "MedkitBig/F2");
            mapper.Register("GenericItem", GFX_MEDKIT_BIG_F3, "MedkitBig/F3");
            mapper.Register("GenericItem", GFX_POWERUP_A_F1, "PowerUpA/F1");
            mapper.Register("GenericItem", GFX_POWERUP_A_F2, "PowerUpA/F2");
            mapper.Register("GenericItem", GFX_POWERUP_A_F3, "PowerUpA/F3");
            mapper.Register("GenericItem", GFX_AMMO_F1, "Ammo/F1");
            mapper.Register("GenericItem", GFX_AMMO_F2, "Ammo/F2");
            mapper.Register("GenericItem", GFX_AMMO_F3, "Ammo/F3");
            mapper.Register("GenericItem", GFX_AREA_SCANNER_F1, "AreaScanner/F1");
            mapper.Register("GenericItem", GFX_AREA_SCANNER_F2, "AreaScanner/F2");
            mapper.Register("GenericItem", GFX_AREA_SCANNER_F3, "AreaScanner/F3");

            mapper.Register("Keycard", GFX_KEYCARD_RED_F2, "Red/F2");
            mapper.Register("Keycard", GFX_KEYCARD_GREEN_F2, "Green/F2");
            mapper.Register("Keycard", GFX_KEYCARD_BLUE_F2, "Blue/F2");
            mapper.Register("Keycard", MapMapper.GFX_ANY, "Trigger");
            mapper.RegisterAction("Keycard1", "Keycard", "1");
            mapper.RegisterAction("Keycard2", "Keycard", "2");
            mapper.RegisterAction("Keycard3", "Keycard", "3");

            mapper.Register("SmartCard", GFX_SMART_CARD_F1, "F1");
            mapper.Register("SmartCard", GFX_SMART_CARD_F2, "F2");
            mapper.Register("SmartCard", GFX_SMART_CARD_F3, "F3");
            mapper.Register("SmartCard", MapMapper.GFX_ANY, "Trigger");
            mapper.RegisterAction("SmartCard1", "SmartCard", "1");
            mapper.RegisterAction("SmartCard2", "SmartCard", "2");
            mapper.RegisterAction("SmartCard3", "SmartCard", "3");


        }
    }
}
