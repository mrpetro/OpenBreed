using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game
{
    public enum ActionContexts
    {
        Player,
        Menu
    }

    public enum MenuAction
    {
        MoveRight,
        MoveLeft,
        MoveUp,
        MoveDown,
        Fire
    }

    public enum PlayerActions
    {
        MoveRight,
        MoveLeft,
        MoveUp,
        MoveDown,
        Fire,
        SwitchWeapon
    }

    public static class GameActions
    {
        public const string Fire = "Fire";
        public const string Secondary = "Secondary";
        public const string PreviousWeapon = "PreviousWeapon";
        public const string NextWeapon = "NextWeapon";
    }
}
