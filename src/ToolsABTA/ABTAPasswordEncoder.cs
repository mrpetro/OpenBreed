using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolsABTA
{



    public enum EnumPlayerItems
    {
        WEAPON_ASSAULT_GUN_NONE = 0x0000,
        WEAPON_ASSAULT_GUN_LOW = 0x0200,
        WEAPON_ASSAULT_GUN_MEDIUM = 0x0100,
        WEAPON_ASSAULT_GUN_HIGH = 0x0300,
        BODY_ARMOUR_NONE = 0x0300,
        BODY_ARMOUR_LOW = 0x0700,
        BODY_ARMOUR_MEDIUM = 0x0C00,
        BODY_ARMOUR_HIGH = 0x0F00,
        WEAPON_HEATSENSE_MISSILES_NONE =    0x0000,
        WEAPON_HEATSENSE_MISSILES_LOW =     0x0010,
        WEAPON_HEATSENSE_MISSILES_MEDIUM =  0x0020,
        WEAPON_HEATSENSE_MISSILES_HIGH =    0x0030,
        WEAPON_TRILAZER_GUN_NONE =          0x0000,
        WEAPON_TRILAZER_GUN_LOW =           0x0040,
        WEAPON_TRILAZER_GUN_MEDIUM =        0x0080,
        WEAPON_TRILAZER_GUN_HIGH =          0x00C0,
        WEAPON_REFRACTION_LAZER_NONE =      0x0000,
        WEAPON_REFRACTION_LAZER_LOW =       0x0001,
        WEAPON_REFRACTION_LAZER_MEDIUM =    0x0002,
        WEAPON_REFRACTION_LAZER_HIGH =      0x0003,
        WEAPON_FIREWALL_NONE =              0x0000,
        WEAPON_FIREWALL_LOW =               0x0004,
        WEAPON_FIREWALL_MEDIUM =            0x0008,
        WEAPON_FIREWALL_HIGH =              0x000C
    };

    public class ABTAPasswordEncoder
    {
        public static readonly Int32[] WEAPON_ASSAULT_GUN =        { 0x0000, 0x0100, 0x0200, 0x0300 };
        public static readonly Int32[] BODY_ARMOUR =               { 0x0000, 0x0400, 0x0800, 0x0C00 };
        public static readonly Int32[] WEAPON_HEATSENSE_MISSILES = { 0x0000, 0x0010, 0x0020, 0x0030 };
        public static readonly Int32[] WEAPON_TRILAZER_GUN =       { 0x0000, 0x0040, 0x0080, 0x00C0 };
        public static readonly Int32[] WEAPON_REFRACTION_LAZER =   { 0x0000, 0x0001, 0x0002, 0x0003 };
        public static readonly Int32[] WEAPON_FIREWALL =           { 0x0000, 0x0004, 0x0008, 0x000C };

        public static readonly string[] LEVELS = {
        "0. GAME CRASH(!!!)        ",
        "1. CRASH LANDING SITE     ",
        "2. CIVILIAN ZONE 1        ",
        "3. CIVILIAN ZONE 2        ",
        "4. CIVILIAN ZONE 3        ",
        "5. CIVILIAN ZONE 4        ",
        "6. OUTDOOR SECTOR 2       ",
        "7. STORES LEVEL 1         ",
        "8. STORES LEVEL 2         ",
        "9. STORES SECTOR 3        ",
        "10.STORES SECTOR 4        ",
        "11.OUTDOOR SECTOR 3       ",
        "12.MILITARY SECTOR 1      ",
        "13.MILITARY SECTOR 2      ",
        "14.MILITARY SECTOR 3      ",
        "15.MILITARY SECTOR 4      ",
        "16.SECURITY ZONE 1        ",
        "17.SECURITY ZONE 2        ",
        "18.SECURITY ZONE 3        ",
        "19.SECURITY ZONE 4        ",
        "20.COLONY GROUND SECTOR 3 ",
        "21.SCIENCE SECTOR 1       ",
        "22.SCIENCE SECTOR 2       ",
        "23.SCIENCE SECTOR 3       ",
        "24.SCIENCE SECTOR 4       ",
        "25.COLONY GROUND SECTOR 4 ",
        "26.ENGINEERING SECTOR 1   ",
        "27.ENGINEERING SECTOR 2   ",
        "28.ENGINEERING SECTOR 3   ",
        "29.ENGINEERING SECTOR 4   ",
        "30.CORRIDOR ZONE 1        ",
        "31.CORRIDOR 2             ",
        "32.CORRIDOR 3             ",
        "33.CORRIDOR 4             ",
        "34.CORRIDOR 5             ",
        "35.CORRIDOR ZONE 6        ",
        "36.CORRIDOR ZONE 7        ",
        "37.CORRIDOR ZONE 8        ",
        "38.CORRIDOR ZONE 9        ",
        "39.CORRIDOR ZONE 10       ",
        "40.CORRIDOR ZONE 11       ",
        "41.CORRIDOR ZONE 12       ",
        "42.CORRIDOR ZONE 13       ",
        "43.CORRIDOR ZONE 14       ",
        "44.CORRIDOR 15            ",
        "45.CORRIDOR 16            ",
        "46.CORRIDOR 17            ",
        "47.MAIN TOWER LEVEL 1     ",
        "48.MAIN TOWER LEVEL 2     ",
        "49.MAIN TOWER LEVEL 3     ",
        "50.MAIN TOWER LEVEL 4     ",
        "51.MAIN TOWER LEVEL 5     ",
        "52.TOWER LEVEL 6          ",
        "53.TOWER LEVEL 7          ",
        "54.Mission One(???)       ",
        "55.OUTER COLONY SECTOR 5  ",
        "56.CUSTOM 1"};

        private static readonly char[] CHARACTERS = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'P', 'S' };

        private int m_Level;
        private int m_Entrance;
        private int m_Credits;

        private int m_P1Lives;
        private int m_P1Keys;
        private int m_P1ItemBodyArmour;
        private int m_P1ItemAssaultGun;
        private int m_P1ItemHeatsenceMissiles;
        private int m_P1ItemTrilazerGun;
        private int m_P1ItemRefractionLazer;
        private int m_P1ItemFirewall;

        private int m_P2Lives;
        private int m_P2Keys;
        private int m_P2ItemBodyArmour;
        private int m_P2ItemAssaultGun;
        private int m_P2ItemHeatsenceMissiles;
        private int m_P2ItemTrilazerGun;
        private int m_P2ItemRefractionLazer;
        private int m_P2ItemFirewall;

        public void SetLevel(int level)
        {
            m_Level = level;
        }

        public void SetEntrance(int entrance)
        {
            m_Entrance = entrance;
        }

        public void SetCredits(int credits)
        {
            m_Credits = credits;
        }

        public void SetP1Lives(int lives)
        {
            m_P1Lives = lives;
        }

        public void SetP2Lives(int lives)
        {
            m_P2Lives = lives;
        }

        public void SetP1Keys(int keys)
        {
            m_P1Keys = keys;
        }

        public void SetP2Keys(int keys)
        {
            m_P2Keys = keys;
        }

        public void SetP1ItemBodyArmour(int power)
        {
            m_P1ItemBodyArmour = power;
        }

        public void SetP1ItemAssaultGun(int power)
        {
            m_P1ItemAssaultGun = power;
        }

        public void SetP1ItemHeatsenceMissiles(int power)
        {
            m_P1ItemHeatsenceMissiles = power;
        }

        public void SetP1ItemTrilazerGun(int power)
        {
            m_P1ItemTrilazerGun = power;
        }

        public void SetP1ItemRefractionLazer(int power)
        {
            m_P1ItemRefractionLazer = power;
        }

        public void SetP1ItemFirewall(int power)
        {
            m_P1ItemFirewall = power;
        }

        public void SetP2ItemBodyArmour(int power)
        {
            m_P2ItemBodyArmour = power;
        }

        public void SetP2ItemAssaultGun(int power)
        {
            m_P2ItemAssaultGun = power;
        }

        public void SetP2ItemHeatsenceMissiles(int power)
        {
            m_P2ItemHeatsenceMissiles = power;
        }

        public void SetP2ItemTrilazerGun(int power)
        {
            m_P2ItemTrilazerGun = power;
        }

        public void SetP2ItemRefractionLazer(int power)
        {
            m_P2ItemRefractionLazer = power;
        }

        public void SetP2ItemFirewall(int power)
        {
            m_P2ItemFirewall = power;
        }

        private char[] ConvertToLevelCode(byte level)
        {
            char[] level_code = new char[2];

            level_code[0] = CHARACTERS[(level & 0xF0 ) >> 4];
            level_code[1] = CHARACTERS[level & 0x0F];

            return level_code;
        }

        private char[] ConvertToMoneyCode(byte level)
        {
            char[] money_code = new char[2];

            money_code[0] = CHARACTERS[(level & 0xF0) >> 4];
            money_code[1] = CHARACTERS[level & 0x0F];

            return money_code;
        }

        private char[] ConvertToWeaponsCode(Int16 weapons)
        {
            char[] weapons_code = new char[3];

            weapons_code[0] = CHARACTERS[(weapons & 0x0F00) >> 8];
            weapons_code[1] = CHARACTERS[(weapons & 0x00F0) >> 4];
            weapons_code[2] = CHARACTERS[weapons & 0x000F];

            return weapons_code;
        }

        private char[] ConvertToLivesCode(byte lives)
        {
            char[] lives_code = new char[1];

            if(lives>7)
               lives_code[0] = 'S';
            else
               if(lives==0)
                  lives_code[0] = 'A';
               else
                  lives_code[0] = CHARACTERS[lives + 8];

            return lives_code;
        }

        private char[] ConvertToKeysCode(byte keys)
        {
            char[] keys_code = { CHARACTERS[keys] };

            return keys_code;
        }

        private char[] ConvertToCheckSumCode(byte level)
        {
            char[] check_sum_code = new char[2];

            check_sum_code[0] = CHARACTERS[(level & 0xF0) >> 4];
            check_sum_code[1] = CHARACTERS[level & 0x0F];

            return check_sum_code;
        }

        private byte GetCheckSum(char[] string_code)
        {
           byte result_sum = 0;
           int i = 0;
           while (i < string_code.Length)
           {
              result_sum+=  (byte)(string_code[i] - 'A');
              i++;
           }

           return result_sum;
        }

        private Int16 GetP1ItemsCodeEx()
        {
            Int32 p1_items = 0;
            p1_items += WEAPON_ASSAULT_GUN[m_P1ItemAssaultGun];
            p1_items += BODY_ARMOUR[m_P1ItemBodyArmour];
            p1_items += WEAPON_HEATSENSE_MISSILES[m_P1ItemHeatsenceMissiles];
            p1_items += WEAPON_TRILAZER_GUN[m_P1ItemTrilazerGun];
            p1_items += WEAPON_REFRACTION_LAZER[m_P1ItemRefractionLazer];
            p1_items += WEAPON_FIREWALL[m_P1ItemFirewall];
            return (Int16)p1_items;
        }

        private Int16 GetP2ItemsCodeEx()
        {
            Int32 p2_items = 0;
            p2_items += WEAPON_ASSAULT_GUN[m_P2ItemAssaultGun];
            p2_items += BODY_ARMOUR[m_P2ItemBodyArmour];
            p2_items += WEAPON_HEATSENSE_MISSILES[m_P2ItemHeatsenceMissiles];
            p2_items += WEAPON_TRILAZER_GUN[m_P2ItemTrilazerGun];
            p2_items += WEAPON_REFRACTION_LAZER[m_P2ItemRefractionLazer];
            p2_items += WEAPON_FIREWALL[m_P2ItemFirewall];
            return (Int16)p2_items;
        }

        //private Int16 GetP1ItemsCode()
        //{
        //    Int16 p1_items = 0;
        //    p1_items += (Int16)(m_P1ItemAssaultGun * 0x0100);
        //    p1_items += (Int16)(m_P1ItemBodyArmour * 0x0400);
        //    p1_items += (Int16)(m_P1ItemHeatsenceMissiles * 0x0010);
        //    p1_items += (Int16)(m_P1ItemTrilazerGun * 0x0040);
        //    p1_items += (Int16)(m_P1ItemRefractionLazer* 0x0001);
        //    p1_items += (Int16)(m_P1ItemFirewall * 0x0004);
        //    return p1_items;
        //}

        //private Int16 GetP2ItemsCode()
        //{
        //    Int16 p2_items = 0;
        //    p2_items += (Int16)(m_P2ItemAssaultGun * 0x0100);
        //    p2_items += (Int16)(m_P2ItemBodyArmour * 0x0400);
        //    p2_items += (Int16)(m_P2ItemHeatsenceMissiles * 0x0010);
        //    p2_items += (Int16)(m_P2ItemTrilazerGun * 0x0040);
        //    p2_items += (Int16)(m_P2ItemRefractionLazer * 0x0001);
        //    p2_items += (Int16)(m_P2ItemFirewall * 0x0004);
        //    return p2_items;
        //}

        public string GetPassword()
        {
            Int16 p1_weapons = GetP1ItemsCodeEx();

            Int16 p2_weapons = GetP2ItemsCodeEx();

            char[] level_code = {'A', 'A'};
            char[] money_code = {'A', 'A', 'A'};
            char[] p1_lives_code = { 'A' };
            char[] p1_keys_code = { 'A' };
            char[] p1_weapons_code = {'A', 'A', 'A'};
            char[] p2_lives_code = { 'A' };
            char[] p2_keys_code = { 'A' };
            char[] p2_weapons_code = {'A', 'A', 'A'};
            char[] check_sum_code = { 'H', 'B' };

            level_code = ConvertToLevelCode((byte)(m_Level + m_Entrance * 64));
            //Convert money attribute
            money_code = ConvertToMoneyCode((byte)m_Credits);
            //Convert player 1 values
            p1_lives_code = ConvertToLivesCode((byte)m_P1Lives);
            p1_keys_code = ConvertToKeysCode((byte)m_P1Keys);
            p1_weapons_code = ConvertToWeaponsCode(p1_weapons);
            //Convert player 2 values
            p2_lives_code = ConvertToLivesCode((byte)m_P2Lives);
            p2_keys_code = ConvertToKeysCode((byte)m_P2Keys);
            p2_weapons_code = ConvertToWeaponsCode(p2_weapons);

            //Calculate check sum of all values
            byte check_sum = 0x71;
            check_sum -= GetCheckSum(level_code);
            check_sum -= GetCheckSum(money_code);
            check_sum -= GetCheckSum(p1_lives_code);
            check_sum -= GetCheckSum(p1_keys_code);
            check_sum -= GetCheckSum(p1_weapons_code);
            check_sum -= GetCheckSum(p2_lives_code);
            check_sum -= GetCheckSum(p2_keys_code);
            check_sum -= GetCheckSum(p2_weapons_code);

            //Convert check sum
            check_sum_code = ConvertToCheckSumCode(check_sum);

            string password = string.Empty;
            password += new string(level_code);
            password += new string(money_code);
            password += new string(p1_lives_code);
            password += new string(p1_keys_code);
            password += new string(p1_weapons_code);
            password += new string(p2_lives_code);
            password += new string(p2_keys_code);
            password += new string(p2_weapons_code);
            password += new string(check_sum_code);

            return password;
        }
    }

}
