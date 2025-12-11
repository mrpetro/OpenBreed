using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Managers
{
    public interface IWeapon
    {
        string Name { get; }
        string Projectile { get; }
        int FireRate { get; }
        string MuzzleFlash { get; }
        int Speed { get; }
    }

    public class Weapon : IWeapon
    {

        public string Name { get; init; }
        public string Projectile { get; init; }
        public int FireRate { get; init; }
        public string MuzzleFlash { get; init; }
        public int Speed { get; init; }
    }


    public interface IWeaponMan
    {
        #region Public Methods

        IWeapon GetWeapon(int weaponId);

        #endregion Public Methods
    }

    public class WeaponMan : IWeaponMan
    {
        #region Private Fields

        private readonly (string Name, string Projectile, int FireRate, string MuzzleFlash, int Speed)[] weapons =
                [
                ("AssaultGun",      "AssaultGun",      25, "AssaultGun", 12),
                ("MissileLauncher", "Missile",         1,  "",           10),
                ("TrilazerGun",     "TrilazerGun",     7,  "",           12),
                ("Flamethrower",    "Firewall",        25, "",           10),
                ("RefractionGun",   "RefractionLazer", 10, "",           8)];

        #endregion Private Fields

        #region Public Constructors

        public WeaponMan()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public IWeapon GetWeapon(int weaponId)
        {
            var weapon = weapons[weaponId];

            return new Weapon
            {
                Name = weapon.Name,
                FireRate = weapon.FireRate,
                Speed = weapon.Speed,
                MuzzleFlash = weapon.MuzzleFlash,
                Projectile = weapon.Projectile,
            };
        }

        #endregion Public Methods
    }
}