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

        private const int MEDKIT_SMALL = 598;
        private const int CREDITS_SMALL = 606;
        private const int KEY_CARD_STANDARD = 608;
        private const int POWERUP_S = 609;
        private const int EXTRA_LIFE = 610;
        private const int SMART_CARD = 611;
        private const int CREDITS_BIG = 626;
        private const int POWERUP_F = 627;
        private const int MEDKIT_BIG = 628;
        private const int POWERUP_A = 629;
        private const int AMMO = 630;
        private const int AREA_SCANNER = 631;

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
            switch (actionValue)
            {
                case SMARTCARD_1:
                    pickableHelper.AddItem(world, ix, iy, "SmartCard");
                    return true;
                case SMARTCARD_2:
                    pickableHelper.AddItem(world, ix, iy, "SmartCard");
                    return true;
                case SMARTCARD_3:
                    pickableHelper.AddItem(world, ix, iy, "SmartCard");
                    return true;
                default:
                    return false;
            }
        }

        private bool PutItem(World world, int ix, int iy, int gfxValue)
        {
            switch (gfxValue)
            {
                case MEDKIT_SMALL:
                    pickableHelper.AddItem(world, ix, iy, "MedkitSmall");
                    return true;

                case CREDITS_SMALL:
                    pickableHelper.AddItem(world, ix, iy, "CreditsSmall");
                    return true;

                case KEY_CARD_STANDARD:
                    pickableHelper.AddItem(world, ix, iy, "KeycardStandard");
                    return true;

                case POWERUP_S:
                    pickableHelper.AddItem(world, ix, iy, "PowerUpS");
                    return true;

                case EXTRA_LIFE:
                    pickableHelper.AddItem(world, ix, iy, "ExtraLife");
                    return true;

                case POWERUP_F:
                    pickableHelper.AddItem(world, ix, iy, "PowerUpF");
                    return true;

                case MEDKIT_BIG:
                    pickableHelper.AddItem(world, ix, iy, "MedkitBig");
                    return true;

                case CREDITS_BIG:
                    pickableHelper.AddItem(world, ix, iy, "CreditsBig");
                    return true;

                case POWERUP_A:
                    pickableHelper.AddItem(world, ix, iy, "PowerUpA");
                    return true;

                case AMMO:
                    pickableHelper.AddItem(world, ix, iy, "Ammo");
                    return true;

                case AREA_SCANNER:
                    pickableHelper.AddItem(world, ix, iy, "AreaScanner");
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