using OpenBreed.Common.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenBreed.Editor.UI.WinForms.Forms
{
    public partial class ABTAPasswordGeneratorForm : Form
    {
        private readonly AbtaPasswordEncoder _passwordEncoder = new AbtaPasswordEncoder();

        public ABTAPasswordGeneratorForm()
        {
            InitializeComponent();

            Reset();

            ConnectEvents();
            UpdatePassword();
        }

        private void Reset()
        {
            cbxLevel.DataSource = AbtaPasswordEncoder.LEVELS;

            numP1Lives.Value = 3;
            cbxP1AssaultGun.SelectedIndex = 0;
            cbxP1BodyArmour.SelectedIndex = 0;
            cbxP1Firewall.SelectedIndex = 0;
            cbxP1HeatsenceMissiles.SelectedIndex = 0;
            cbxP1RefractionLazer.SelectedIndex = 0;
            cbxP1TrilazerGun.SelectedIndex = 0;

            numP2Lives.Value = 3;
            cbxP2AssaultGun.SelectedIndex = 0;
            cbxP2BodyArmour.SelectedIndex = 0;
            cbxP2Firewall.SelectedIndex = 0;
            cbxP2HeatsenceMissiles.SelectedIndex = 0;
            cbxP2RefractionLazer.SelectedIndex = 0;
            cbxP2TrilazerGun.SelectedIndex = 0;

            cbxCredits.Items.Clear();
            for (int i = 0; i < 256; i++)
                cbxCredits.Items.Add((i * 1000).ToString());

            cbxCredits.SelectedIndex = 0;
        }

        private void ConnectEvents()
        {
            cbxLevel.SelectedIndexChanged += (s, a) => { _passwordEncoder.SetLevel(cbxLevel.SelectedIndex); UpdatePassword(); };
            cbxCredits.SelectedIndexChanged += (s, a) => { _passwordEncoder.SetCredits(cbxCredits.SelectedIndex); UpdatePassword(); };
            radEntrance0.CheckedChanged += (s, a) => { _passwordEncoder.SetEntrance(0); UpdatePassword(); };
            radEntrance1.CheckedChanged += (s, a) => { _passwordEncoder.SetEntrance(1); UpdatePassword(); };
            radEntrance2.CheckedChanged += (s, a) => { _passwordEncoder.SetEntrance(2); UpdatePassword(); };
            radEntrance3.CheckedChanged += (s, a) => { _passwordEncoder.SetEntrance(3); UpdatePassword(); };

            numP1Lives.ValueChanged += (s, a) => { _passwordEncoder.SetP1Lives(Convert.ToInt32(numP1Lives.Value)); UpdatePassword(); };
            numP1Keys.ValueChanged += (s, a) => { _passwordEncoder.SetP1Keys(Convert.ToInt32(numP1Keys.Value)); UpdatePassword(); };
            cbxP1BodyArmour.SelectedIndexChanged += (s, a) => { _passwordEncoder.SetP1ItemBodyArmour(cbxP1BodyArmour.SelectedIndex); UpdatePassword(); };
            cbxP1AssaultGun.SelectedIndexChanged += (s, a) => { _passwordEncoder.SetP1ItemAssaultGun(cbxP1AssaultGun.SelectedIndex); UpdatePassword(); };
            cbxP1HeatsenceMissiles.SelectedIndexChanged += (s, a) => { _passwordEncoder.SetP1ItemHeatsenceMissiles(cbxP1HeatsenceMissiles.SelectedIndex); UpdatePassword(); };
            cbxP1TrilazerGun.SelectedIndexChanged += (s, a) => { _passwordEncoder.SetP1ItemTrilazerGun(cbxP1TrilazerGun.SelectedIndex); UpdatePassword(); };
            cbxP1Firewall.SelectedIndexChanged += (s, a) => { _passwordEncoder.SetP1ItemFirewall(cbxP1Firewall.SelectedIndex); UpdatePassword(); };
            cbxP1RefractionLazer.SelectedIndexChanged += (s, a) => { _passwordEncoder.SetP1ItemRefractionLazer(cbxP1RefractionLazer.SelectedIndex); UpdatePassword(); };

            numP2Lives.ValueChanged += (s, a) => { _passwordEncoder.SetP2Lives(Convert.ToInt32(numP2Lives.Value)); UpdatePassword(); };
            numP2Keys.ValueChanged += (s, a) => { _passwordEncoder.SetP2Keys(Convert.ToInt32(numP2Keys.Value)); UpdatePassword(); };
            cbxP2BodyArmour.SelectedIndexChanged += (s, a) => { _passwordEncoder.SetP2ItemBodyArmour(cbxP2BodyArmour.SelectedIndex); UpdatePassword(); };
            cbxP2AssaultGun.SelectedIndexChanged += (s, a) => { _passwordEncoder.SetP2ItemAssaultGun(cbxP2AssaultGun.SelectedIndex); UpdatePassword(); };
            cbxP2HeatsenceMissiles.SelectedIndexChanged += (s, a) => { _passwordEncoder.SetP2ItemHeatsenceMissiles(cbxP2HeatsenceMissiles.SelectedIndex); UpdatePassword(); };
            cbxP2TrilazerGun.SelectedIndexChanged += (s, a) => { _passwordEncoder.SetP2ItemTrilazerGun(cbxP2TrilazerGun.SelectedIndex); UpdatePassword(); };
            cbxP2Firewall.SelectedIndexChanged += (s, a) => { _passwordEncoder.SetP2ItemFirewall(cbxP2Firewall.SelectedIndex); UpdatePassword(); };
            cbxP2RefractionLazer.SelectedIndexChanged += (s, a) => { _passwordEncoder.SetP2ItemRefractionLazer(cbxP2RefractionLazer.SelectedIndex); UpdatePassword(); };
        }

        private void UpdatePassword()
        {
            tbxPassword.Text = _passwordEncoder.GetPassword();
        }
    }
}
