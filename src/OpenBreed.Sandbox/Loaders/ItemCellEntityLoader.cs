using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Pickable;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class ItemCellEntityLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        public const int GENERIC_ITEM = 55;
        public const int SMARTCARD_1 = 22;
        public const int SMARTCARD_2 = 23;
        public const int SMARTCARD_3 = 24;
        public const int KEYCARD_RED = 33;
        public const int KEYCARD_GREEN = 34;
        public const int KEYCARD_BLUE = 35;
        public const int KEYCARD_SPECIAL = 39;

        #endregion Public Fields

        #region Private Fields

        private const int MEDKIT_SMALL_F1 = 598;
        private const int MEDKIT_SMALL_F2 = 649;
        private const int MEDKIT_SMALL_F3 = 653;

        private const int CREDITS_SMALL_F1 = 600;
        private const int KEY_CARD_STANDARD_F1 = 602;
        private const int POWERUP_S_F1 = 603;
        private const int EXTRA_LIFE_F1 = 604;
        private const int SMART_CARD_F1 = 605;
        private const int CREDITS_BIG_F1 = 620;
        private const int POWERUP_F_F1 = 621;
        private const int MEDKIT_BIG_F1 = 622;
        private const int POWERUP_A_F1 = 623;
        private const int AMMO_F1 = 624;
        private const int AREA_SCANNER_F1 = 625;

        private const int CREDITS_SMALL_F2 = 606;
        private const int KEY_CARD_STANDARD_F2 = 608;
        private const int POWERUP_S_F2 = 609;
        private const int EXTRA_LIFE_F2 = 610;
        private const int SMART_CARD_F2 = 611;
        private const int CREDITS_BIG_F2 = 626;
        private const int POWERUP_F_F2 = 627;
        private const int MEDKIT_BIG_F2 = 628;
        private const int POWERUP_A_F2 = 629;
        private const int AMMO_F2 = 630;
        private const int AREA_SCANNER_F2 = 631;


        private const int CREDITS_SMALL_F3 = 612;
        private const int KEY_CARD_STANDARD_F3 = 614;
        private const int POWERUP_S_F3 = 615;
        private const int EXTRA_LIFE_F3 = 616;
        private const int SMART_CARD_F3 = 617;
        private const int CREDITS_BIG_F3 = 632;
        private const int POWERUP_F_F3 = 633;
        private const int MEDKIT_BIG_F3 = 634;
        private const int POWERUP_A_F3 = 635;
        private const int AMMO_F3 = 636;
        private const int AREA_SCANNER_F3 = 637;


        private readonly PickableHelper pickableHelper;

        #endregion Private Fields

        #region Public Constructors

        public ItemCellEntityLoader(PickableHelper pickableHelper)
        {
            this.pickableHelper = pickableHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Load(WorldBlockBuilder worldBlockBuilder, MapLayoutModel layout, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            switch (actionValue)
            {
                case GENERIC_ITEM:
                    visited[ix, iy] = PutItem(world, ix, iy, gfxValue);
                    break;
                case SMARTCARD_1:
                case SMARTCARD_2:
                case SMARTCARD_3:
                    visited[ix, iy] = PutSmartCard(world, ix, iy, actionValue, gfxValue);
                    break;
                case KEYCARD_RED:
                case KEYCARD_GREEN:
                case KEYCARD_BLUE:
                case KEYCARD_SPECIAL:
                    visited[ix, iy] = PutKeycardCustom(world, ix, iy, actionValue, gfxValue);
                    break;
                default:
                    break;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private bool PutKeycardCustom(World world, int ix, int iy, int actionValue, int gfxValue)
        {
            switch (actionValue)
            {
                case KEYCARD_RED:
                    pickableHelper.AddItem(world, ix, iy, "KeycardRed");
                    return true;
                case KEYCARD_GREEN:
                    pickableHelper.AddItem(world, ix, iy, "KeycardGreen");
                    return true;
                case KEYCARD_BLUE:
                    pickableHelper.AddItem(world, ix, iy, "KeycardBlue");
                    return true;
                case KEYCARD_SPECIAL:
                    pickableHelper.AddItem(world, ix, iy, "KeycardSpecial");
                    return true;
                default:
                    return false;
            }
        }

        private bool PutSmartCard(World world, int ix, int iy, int actionValue, int gfxValue)
        {
            var flavor = default(string);

            switch (gfxValue)
            {
                case SMART_CARD_F1:
                    flavor = "F1";
                    break;
                case SMART_CARD_F2:
                    flavor = "F2";
                    break;
                case SMART_CARD_F3:
                    flavor = "F3";
                    break;
                default:
                    flavor = "Trigger";
                    break;
            }

            switch (actionValue)
            {
                case SMARTCARD_1:
                    pickableHelper.AddItem(world, ix, iy, "SmartCard", flavor);
                    return true;
                case SMARTCARD_2:
                    pickableHelper.AddItem(world, ix, iy, "SmartCard", flavor);
                    return true;
                case SMARTCARD_3:
                    pickableHelper.AddItem(world, ix, iy, "SmartCard", flavor);
                    return true;
                default:
                    return false;
            }
        }

        private bool PutItem(World world, int ix, int iy, int gfxValue)
        {
            switch (gfxValue)
            {
                case MEDKIT_SMALL_F1:
                    pickableHelper.AddItem(world, ix, iy, "MedkitSmall", "F1");
                    return true;
                case MEDKIT_SMALL_F2:
                    pickableHelper.AddItem(world, ix, iy, "MedkitSmall", "F2");
                    return true;
                case MEDKIT_SMALL_F3:
                    pickableHelper.AddItem(world, ix, iy, "MedkitSmall", "F3");
                    return true;
                case CREDITS_SMALL_F1:
                    pickableHelper.AddItem(world, ix, iy, "CreditsSmall", "F1");
                    return true;
                case CREDITS_SMALL_F2:
                    pickableHelper.AddItem(world, ix, iy, "CreditsSmall", "F2");
                    return true;
                case CREDITS_SMALL_F3:
                    pickableHelper.AddItem(world, ix, iy, "CreditsSmall", "F3");
                    return true;
                case KEY_CARD_STANDARD_F1:
                    pickableHelper.AddItem(world, ix, iy, "KeycardStandard", "F1");
                    return true;
                case KEY_CARD_STANDARD_F2:
                    pickableHelper.AddItem(world, ix, iy, "KeycardStandard", "F2");
                    return true;
                case KEY_CARD_STANDARD_F3:
                    pickableHelper.AddItem(world, ix, iy, "KeycardStandard", "F3");
                    return true;
                case POWERUP_S_F1:
                    pickableHelper.AddItem(world, ix, iy, "PowerUpS", "F1");
                    return true;
                case POWERUP_S_F2:
                    pickableHelper.AddItem(world, ix, iy, "PowerUpS", "F2");
                    return true;
                case POWERUP_S_F3:
                    pickableHelper.AddItem(world, ix, iy, "PowerUpS", "F3");
                    return true;
                case EXTRA_LIFE_F1:
                    pickableHelper.AddItem(world, ix, iy, "ExtraLife", "F1");
                    return true;
                case EXTRA_LIFE_F2:
                    pickableHelper.AddItem(world, ix, iy, "ExtraLife", "F2");
                    return true;
                case EXTRA_LIFE_F3:
                    pickableHelper.AddItem(world, ix, iy, "ExtraLife", "F3");
                    return true;
                case POWERUP_F_F1:
                    pickableHelper.AddItem(world, ix, iy, "PowerUpF", "F1");
                    return true;
                case POWERUP_F_F2:
                    pickableHelper.AddItem(world, ix, iy, "PowerUpF", "F2");
                    return true;
                case POWERUP_F_F3:
                    pickableHelper.AddItem(world, ix, iy, "PowerUpF", "F3");
                    return true;
                case MEDKIT_BIG_F1:
                    pickableHelper.AddItem(world, ix, iy, "MedkitBig", "F1");
                    return true;
                case MEDKIT_BIG_F2:
                    pickableHelper.AddItem(world, ix, iy, "MedkitBig", "F2");
                    return true;
                case MEDKIT_BIG_F3:
                    pickableHelper.AddItem(world, ix, iy, "MedkitBig", "F3");
                    return true;
                case CREDITS_BIG_F1:
                    pickableHelper.AddItem(world, ix, iy, "CreditsBig", "F1");
                    return true;
                case CREDITS_BIG_F2:
                    pickableHelper.AddItem(world, ix, iy, "CreditsBig", "F2");
                    return true;
                case CREDITS_BIG_F3:
                    pickableHelper.AddItem(world, ix, iy, "CreditsBig", "F3");
                    return true;
                case POWERUP_A_F1:
                    pickableHelper.AddItem(world, ix, iy, "PowerUpA", "F1");
                    return true;
                case POWERUP_A_F2:
                    pickableHelper.AddItem(world, ix, iy, "PowerUpA", "F2");
                    return true;
                case POWERUP_A_F3:
                    pickableHelper.AddItem(world, ix, iy, "PowerUpA", "F3");
                    return true;
                case AMMO_F1:
                    pickableHelper.AddItem(world, ix, iy, "Ammo", "F1");
                    return true;
                case AMMO_F2:
                    pickableHelper.AddItem(world, ix, iy, "Ammo", "F2");
                    return true;
                case AMMO_F3:
                    pickableHelper.AddItem(world, ix, iy, "Ammo", "F3");
                    return true;
                case AREA_SCANNER_F1:
                    pickableHelper.AddItem(world, ix, iy, "AreaScanner", "F1");
                    return true;
                case AREA_SCANNER_F2:
                    pickableHelper.AddItem(world, ix, iy, "AreaScanner", "F2");
                    return true;
                case AREA_SCANNER_F3:
                    pickableHelper.AddItem(world, ix, iy, "AreaScanner", "F3");
                    return true;
                default:
                    {
                        return false;
                    }
            }
        }

        private void PutItem(MapLayoutModel layout, bool[,] visited, World world, int ix, int iy, int gfxValue)
        {
            visited[ix, iy] = PutItem(world, ix, iy, gfxValue);
        }

        #endregion Private Methods
    }
}