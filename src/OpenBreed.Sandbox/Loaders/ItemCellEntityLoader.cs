using OpenBreed.Model.Maps;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Pickable;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Loaders
{
    public class ItemCellEntityLoader : IMapWorldEntityLoader
    {
        #region Public Fields

        public static readonly int CODE = 55;

        #endregion Public Fields

        #region Private Fields

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
            PutItem(layout, visited, world, ix, iy, gfxValue);
        }

        #endregion Public Methods

        #region Private Methods

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

        private bool TryPutItem(World world, int ix, int iy, int gfxValue)
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
                case SMART_CARD:
                    pickableHelper.AddItem(world, ix, iy, "SmartCard");
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
            visited[ix, iy] = TryPutItem(world, ix, iy, gfxValue);

            if (!visited[ix, iy])
            {
            }

            //pickableHelper.AddItem(world, ix, iy);
            //visited[ix, iy] = true;
        }

        #endregion Private Methods
    }
}