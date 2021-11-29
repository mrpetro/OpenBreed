using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Builders
{
    public static class MapCodesL4
    {
        //Action codes

        public const int VOID_CODE = 0;


        public const int ELECTRIC_GATE_UP = 7;
        public const int ELECTRIC_GATE_DOWN = 8;
        public const int ELECTRIC_GATE_RIGHT = 12;
        public const int ELECTRIC_GATE_LEFT = 13;
        public const int TV_FLICKERING_CODE = 14;



        public const int SMARTCARD_1 = 22;
        public const int SMARTCARD_2 = 23;
        public const int SMARTCARD_3 = 24;


        public const int DOOR_RED = 28;
        public const int DOOR_GREEN = 29;
        public const int DOOR_BLUE = 30;
        public const int TELEPORT_ENTRY_CODE = 31;
        public const int KEYCARD_RED = 33;
        public const int KEYCARD_GREEN = 34;
        public const int KEYCARD_BLUE = 35;
        public const int TELEPORT_EXIT_CODE = 36;
        public const int MAP_EXIT_2 = 37;
        public const int MAP_EXIT_3 = 38;

        public const int KEYCARD_SPECIAL = 39;
        public const int MONSTER_EATING_CODE = 42;


        public const int MAP_ENTRY_1 = 45;
        public const int MAP_ENTRY_2 = 46;

        public const int MAP_EXIT_1 = 54;
        public const int GENERIC_ITEM = 55;
        public const int MAP_ENTRY_3 = 56;

        public const int ACTOR_ONLY_OBSTACLE_CODE = 60;
        public const int DOOR_STANDARD = 62;
        public const int FULL_OBSTACLE_CODE = 63;


        //GFX codes
        public const int MEDKIT_SMALL_F1 = 598;
        public const int MEDKIT_SMALL_F2 = 649;
        public const int MEDKIT_SMALL_F3 = 653;
        public const int CREDITS_SMALL_F1 = 600;
        public const int KEY_CARD_STANDARD_F1 = 602;
        public const int POWERUP_S_F1 = 603;
        public const int EXTRA_LIFE_F1 = 604;
        public const int SMART_CARD_F1 = 605;
        public const int CREDITS_BIG_F1 = 620;
        public const int POWERUP_F_F1 = 621;
        public const int MEDKIT_BIG_F1 = 622;
        public const int POWERUP_A_F1 = 623;
        public const int AMMO_F1 = 624;
        public const int AREA_SCANNER_F1 = 625;
        public const int CREDITS_SMALL_F2 = 606;
        public const int KEY_CARD_STANDARD_F2 = 608;
        public const int POWERUP_S_F2 = 609;
        public const int EXTRA_LIFE_F2 = 610;
        public const int SMART_CARD_F2 = 611;
        public const int CREDITS_BIG_F2 = 626;
        public const int POWERUP_F_F2 = 627;
        public const int MEDKIT_BIG_F2 = 628;
        public const int POWERUP_A_F2 = 629;
        public const int AMMO_F2 = 630;
        public const int AREA_SCANNER_F2 = 631;
        public const int CREDITS_SMALL_F3 = 612;
        public const int KEY_CARD_STANDARD_F3 = 614;
        public const int POWERUP_S_F3 = 615;
        public const int EXTRA_LIFE_F3 = 616;
        public const int SMART_CARD_F3 = 617;
        public const int CREDITS_BIG_F3 = 632;
        public const int POWERUP_F_F3 = 633;
        public const int MEDKIT_BIG_F3 = 634;
        public const int POWERUP_A_F3 = 635;
        public const int AMMO_F3 = 636;
        public const int AREA_SCANNER_F3 = 637;
        public const int KEYCARD_SPECIAL_F2 = 618;
        public const int KEYCARD_BLUE_F1 = 638;
        public const int KEYCARD_RED_F1 = 362;
        public const int KEYCARD_GREEN_F1 = 534;


        public static void SetupL4(this MapMapper mapper)
        {
            mapper.Register(MAP_ENTRY_1, MapMapper.GFX_ANY, "MapEntry1", "");
            mapper.Register(MAP_ENTRY_2, MapMapper.GFX_ANY, "MapEntry2", "");
            mapper.Register(MAP_ENTRY_3, MapMapper.GFX_ANY, "MapEntry3", "");

            mapper.Register(MAP_EXIT_1, MapMapper.GFX_ANY, "MapExit1", "");
            mapper.Register(MAP_EXIT_2, MapMapper.GFX_ANY, "MapExit2", "");
            mapper.Register(MAP_EXIT_3, MapMapper.GFX_ANY, "MapExit3", "");

            mapper.Register(TELEPORT_ENTRY_CODE, MapMapper.GFX_ANY, "TeleportEntry", "");
            mapper.Register(TELEPORT_EXIT_CODE, MapMapper.GFX_ANY, "TeleportExit", "");

            mapper.Register(VOID_CODE, MapMapper.GFX_ANY, "Void", "");
            mapper.Register(FULL_OBSTACLE_CODE, MapMapper.GFX_ANY, "FullObstacle", "");
            mapper.Register(ACTOR_ONLY_OBSTACLE_CODE, MapMapper.GFX_ANY, "ActorOnlyObstacle", "");

            mapper.Register(MONSTER_EATING_CODE, MapMapper.GFX_ANY, "MonsterEating", "");
            mapper.Register(TV_FLICKERING_CODE, MapMapper.GFX_ANY, "TVFlickering", "");

            mapper.Register(ELECTRIC_GATE_UP, MapMapper.GFX_ANY, "ElectricGateUp", "");
            mapper.Register(ELECTRIC_GATE_DOWN, MapMapper.GFX_ANY, "ElectricGateDown", "");
            mapper.Register(ELECTRIC_GATE_LEFT, MapMapper.GFX_ANY, "ElectricGateLeft", "");
            mapper.Register(ELECTRIC_GATE_RIGHT, MapMapper.GFX_ANY, "ElectricGateRight", "");

            mapper.Register(DOOR_STANDARD, MapMapper.GFX_ANY, "DoorStandard", "");
            mapper.Register(DOOR_RED, MapMapper.GFX_ANY, "DoorRed", "");
            mapper.Register(DOOR_GREEN, MapMapper.GFX_ANY, "DoorGreen", "");
            mapper.Register(DOOR_BLUE, MapMapper.GFX_ANY, "DoorBlue", "");

            mapper.Register(GENERIC_ITEM, MEDKIT_SMALL_F1, "MedkitSmall", "F1");
            mapper.Register(GENERIC_ITEM, MEDKIT_SMALL_F2, "MedkitSmall", "F2");
            mapper.Register(GENERIC_ITEM, MEDKIT_SMALL_F3, "MedkitSmall", "F3");

            mapper.Register(GENERIC_ITEM, CREDITS_SMALL_F1, "CreditsSmall", "F1");
            mapper.Register(GENERIC_ITEM, CREDITS_SMALL_F2, "CreditsSmall", "F2");
            mapper.Register(GENERIC_ITEM, CREDITS_SMALL_F3, "CreditsSmall", "F3");

            mapper.Register(GENERIC_ITEM, KEY_CARD_STANDARD_F1, "KeycardStandard", "F1");
            mapper.Register(GENERIC_ITEM, KEY_CARD_STANDARD_F2, "KeycardStandard", "F2");
            mapper.Register(GENERIC_ITEM, KEY_CARD_STANDARD_F3, "KeycardStandard", "F3");

            mapper.Register(GENERIC_ITEM, POWERUP_S_F1, "PowerUpS", "F1");
            mapper.Register(GENERIC_ITEM, POWERUP_S_F2, "PowerUpS", "F2");
            mapper.Register(GENERIC_ITEM, POWERUP_S_F3, "PowerUpS", "F3");

            mapper.Register(GENERIC_ITEM, EXTRA_LIFE_F1, "ExtraLife", "F1");
            mapper.Register(GENERIC_ITEM, EXTRA_LIFE_F2, "ExtraLife", "F2");
            mapper.Register(GENERIC_ITEM, EXTRA_LIFE_F3, "ExtraLife", "F3");

            mapper.Register(GENERIC_ITEM, CREDITS_BIG_F1, "CreditsBig", "F1");
            mapper.Register(GENERIC_ITEM, CREDITS_BIG_F2, "CreditsBig", "F2");
            mapper.Register(GENERIC_ITEM, CREDITS_BIG_F3, "CreditsBig", "F3");

            mapper.Register(GENERIC_ITEM, POWERUP_F_F1, "PowerUpF", "F1");
            mapper.Register(GENERIC_ITEM, POWERUP_F_F2, "PowerUpF", "F2");
            mapper.Register(GENERIC_ITEM, POWERUP_F_F3, "PowerUpF", "F3");

            mapper.Register(GENERIC_ITEM, MEDKIT_BIG_F1, "MedkitBig", "F1");
            mapper.Register(GENERIC_ITEM, MEDKIT_BIG_F2, "MedkitBig", "F2");
            mapper.Register(GENERIC_ITEM, MEDKIT_BIG_F3, "MedkitBig", "F3");

            mapper.Register(GENERIC_ITEM, POWERUP_A_F1, "PowerUpA", "F1");
            mapper.Register(GENERIC_ITEM, POWERUP_A_F2, "PowerUpA", "F2");
            mapper.Register(GENERIC_ITEM, POWERUP_A_F3, "PowerUpA", "F3");

            mapper.Register(GENERIC_ITEM, AMMO_F1, "PowerUpA", "F1");
            mapper.Register(GENERIC_ITEM, AMMO_F2, "PowerUpA", "F2");
            mapper.Register(GENERIC_ITEM, AMMO_F3, "PowerUpA", "F3");

            mapper.Register(GENERIC_ITEM, AREA_SCANNER_F1, "AreaScanner", "F1");
            mapper.Register(GENERIC_ITEM, AREA_SCANNER_F2, "AreaScanner", "F2");
            mapper.Register(GENERIC_ITEM, AREA_SCANNER_F3, "AreaScanner", "F3");

            mapper.Register(KEYCARD_RED, KEYCARD_RED_F1, "KeycardRed", "F1");
            mapper.Register(KEYCARD_GREEN, KEYCARD_GREEN_F1, "KeycardGreen", "F1");
            mapper.Register(KEYCARD_BLUE, KEYCARD_BLUE_F1, "KeycardBlue", "F1");
            mapper.Register(KEYCARD_RED, MapMapper.GFX_ANY, "KeycardRed", "Trigger");
            mapper.Register(KEYCARD_GREEN, MapMapper.GFX_ANY, "KeycardGreen", "Trigger");
            mapper.Register(KEYCARD_BLUE, MapMapper.GFX_ANY, "KeycardBlue", "Trigger");

            mapper.Register(KEYCARD_SPECIAL, KEYCARD_SPECIAL_F2, "KeycardSpecial", "F2");

            mapper.Register(SMARTCARD_1, SMART_CARD_F1, "SmartCard1", "F1");
            mapper.Register(SMARTCARD_1, SMART_CARD_F2, "SmartCard1", "F2");
            mapper.Register(SMARTCARD_1, SMART_CARD_F3, "SmartCard1", "F3");
            mapper.Register(SMARTCARD_2, SMART_CARD_F1, "SmartCard2", "F1");
            mapper.Register(SMARTCARD_2, SMART_CARD_F2, "SmartCard2", "F2");
            mapper.Register(SMARTCARD_2, SMART_CARD_F3, "SmartCard2", "F3");
            mapper.Register(SMARTCARD_3, SMART_CARD_F1, "SmartCard3", "F1");
            mapper.Register(SMARTCARD_3, SMART_CARD_F2, "SmartCard3", "F2");
            mapper.Register(SMARTCARD_3, SMART_CARD_F3, "SmartCard3", "F3");
            mapper.Register(SMARTCARD_1, MapMapper.GFX_ANY, "SmartCard", "Trigger");
            mapper.Register(SMARTCARD_2, MapMapper.GFX_ANY, "SmartCard", "Trigger");
            mapper.Register(SMARTCARD_3, MapMapper.GFX_ANY, "SmartCard", "Trigger");

        }
    }
}
