using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Tools;
using OpenBreed.Editor.VM.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Tools
{
    public class AbtaPasswordGeneratorVM : BaseViewModel
    {
        #region Private Fields

        private readonly AbtaPasswordEncoder _passwordEncoder;
        private int _startingCreditsIndex;
        private int _startingLevelIndex;
        private int _startingEntranceIndex;

        private string _password;
        private int _p1StartingLivesIndex;
        private int _p1StartingKeysIndex;
        private int _p1AssaultGunIndex;
        private int _p1BodyArmourIndex;
        private int _p1FirewallIndex;
        private int _p1HeatsenceMissilesIndex;
        private int _p1TrilazerGunIndex;
        private int _p1RefractionLazerIndex;

        private int _p2StartingLivesIndex;
        private int _p2StartingKeysIndex;
        private int _p2AssaultGunIndex;
        private int _p2BodyArmourIndex;
        private int _p2FirewallIndex;
        private int _p2HeatsenceMissilesIndex;
        private int _p2TrilazerGunIndex;
        private int _p2RefractionLazerIndex;

        #endregion Private Fields

        #region Public Constructors

        public AbtaPasswordGeneratorVM(AbtaPasswordEncoder passwordEncoder)
        {
            this._passwordEncoder = passwordEncoder;

            Credits = new ObservableCollection<int>(Enumerable.Range(0, 256).Select(i => i * 1000));
            LifeNumbers = new ObservableCollection<int>(Enumerable.Range(0, 8));
            KeyNumbers = new ObservableCollection<int>(Enumerable.Range(0, 16));

            Levels = new ObservableCollection<string>(AbtaPasswordEncoder.LEVELS);
            EquipmentStates = new ObservableCollection<string>(AbtaPasswordEncoder.EQUIPMENT_STATES);
            EntranceNumbers = new ObservableCollection<int>(Enumerable.Range(0, 4));

            Password = _passwordEncoder.GetPassword();
        }

        #endregion Public Constructors

        #region Public Properties

        public int StartingCreditsIndex
        {
            get { return _startingCreditsIndex; }
            set { SetProperty(ref _startingCreditsIndex, value); }
        }

        public int StartingLevelIndex
        {
            get { return _startingLevelIndex; }
            set { SetProperty(ref _startingLevelIndex, value); }
        }

        public int StartingEntranceIndex
        {
            get { return _startingEntranceIndex; }
            set { SetProperty(ref _startingEntranceIndex, value); }
        }

        public int P1StartingLivesIndex
        {
            get { return _p1StartingLivesIndex; }
            set { SetProperty(ref _p1StartingLivesIndex, value); }
        }

        public int P1StartingKeysIndex
        {
            get { return _p1StartingKeysIndex; }
            set { SetProperty(ref _p1StartingKeysIndex, value); }
        }

        public int P1AssaultGunIndex
        {
            get { return _p1AssaultGunIndex; }
            set { SetProperty(ref _p1AssaultGunIndex, value); }
        }

        public int P1BodyArmourIndex
        {
            get { return _p1BodyArmourIndex; }
            set { SetProperty(ref _p1BodyArmourIndex, value); }
        }

        public int P1FirewallIndex
        {
            get { return _p1FirewallIndex; }
            set { SetProperty(ref _p1FirewallIndex, value); }
        }

        public int P1HeatsenceMissilesIndex
        {
            get { return _p1HeatsenceMissilesIndex; }
            set { SetProperty(ref _p1HeatsenceMissilesIndex, value); }
        }

        public int P1RefractionLazerIndex
        {
            get { return _p1RefractionLazerIndex; }
            set { SetProperty(ref _p1RefractionLazerIndex, value); }
        }

        public int P1TrilazerGunIndex
        {
            get { return _p1TrilazerGunIndex; }
            set { SetProperty(ref _p1TrilazerGunIndex, value); }
        }

        public int P2StartingLivesIndex
        {
            get { return _p2StartingLivesIndex; }
            set { SetProperty(ref _p2StartingLivesIndex, value); }
        }

        public int P2StartingKeysIndex
        {
            get { return _p2StartingKeysIndex; }
            set { SetProperty(ref _p2StartingKeysIndex, value); }
        }

        public int P2AssaultGunIndex
        {
            get { return _p2AssaultGunIndex; }
            set { SetProperty(ref _p2AssaultGunIndex, value); }
        }

        public int P2BodyArmourIndex
        {
            get { return _p2BodyArmourIndex; }
            set { SetProperty(ref _p2BodyArmourIndex, value); }
        }

        public int P2FirewallIndex
        {
            get { return _p2FirewallIndex; }
            set { SetProperty(ref _p2FirewallIndex, value); }
        }

        public int P2HeatsenceMissilesIndex
        {
            get { return _p2HeatsenceMissilesIndex; }
            set { SetProperty(ref _p2HeatsenceMissilesIndex, value); }
        }

        public int P2RefractionLazerIndex
        {
            get { return _p2RefractionLazerIndex; }
            set { SetProperty(ref _p2RefractionLazerIndex, value); }
        }

        public int P2TrilazerGunIndex
        {
            get { return _p2TrilazerGunIndex; }
            set { SetProperty(ref _p2TrilazerGunIndex, value); }
        }

        public ObservableCollection<int> Credits { get; }
        public ObservableCollection<int> LifeNumbers { get; }
        public ObservableCollection<int> KeyNumbers { get; }
        public ObservableCollection<string> Levels { get; }
        public ObservableCollection<string> EquipmentStates { get; }
        public ObservableCollection<int> EntranceNumbers { get; }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        #endregion Public Properties

        #region Protected Methods

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case nameof(StartingLevelIndex):
                    _passwordEncoder.SetLevel(StartingLevelIndex);
                    UpdatePassword();
                    break;

                case nameof(StartingCreditsIndex):
                    _passwordEncoder.SetCredits(StartingCreditsIndex);
                    UpdatePassword();
                    break;

                case nameof(StartingEntranceIndex):
                    _passwordEncoder.SetEntrance(StartingEntranceIndex);
                    UpdatePassword();
                    break;

                case nameof(P1StartingLivesIndex):
                    _passwordEncoder.SetP1Lives(P1StartingLivesIndex);
                    UpdatePassword();
                    break;

                case nameof(P1StartingKeysIndex):
                    _passwordEncoder.SetP1Keys(P1StartingKeysIndex);
                    UpdatePassword();
                    break;

                case nameof(P1AssaultGunIndex):
                    _passwordEncoder.SetP1ItemAssaultGun(P1AssaultGunIndex);
                    UpdatePassword();
                    break;

                case nameof(P1BodyArmourIndex):
                    _passwordEncoder.SetP1ItemBodyArmour(P1BodyArmourIndex);
                    UpdatePassword();
                    break;

                case nameof(P1FirewallIndex):
                    _passwordEncoder.SetP1ItemFirewall(P1FirewallIndex);
                    UpdatePassword();
                    break;

                case nameof(P1HeatsenceMissilesIndex):
                    _passwordEncoder.SetP1ItemHeatsenceMissiles(P1HeatsenceMissilesIndex);
                    UpdatePassword();
                    break;

                case nameof(P1TrilazerGunIndex):
                    _passwordEncoder.SetP1ItemTrilazerGun(P1TrilazerGunIndex);
                    UpdatePassword();
                    break;

                case nameof(P1RefractionLazerIndex):
                    _passwordEncoder.SetP1ItemRefractionLazer(P1RefractionLazerIndex);
                    UpdatePassword();
                    break;

                case nameof(P2StartingLivesIndex):
                    _passwordEncoder.SetP2Lives(P2StartingLivesIndex);
                    UpdatePassword();
                    break;

                case nameof(P2StartingKeysIndex):
                    _passwordEncoder.SetP2Keys(P2StartingKeysIndex);
                    UpdatePassword();
                    break;

                case nameof(P2AssaultGunIndex):
                    _passwordEncoder.SetP2ItemAssaultGun(P2AssaultGunIndex);
                    UpdatePassword();
                    break;

                case nameof(P2BodyArmourIndex):
                    _passwordEncoder.SetP2ItemBodyArmour(P2BodyArmourIndex);
                    UpdatePassword();
                    break;

                case nameof(P2FirewallIndex):
                    _passwordEncoder.SetP2ItemFirewall(P2FirewallIndex);
                    UpdatePassword();
                    break;

                case nameof(P2HeatsenceMissilesIndex):
                    _passwordEncoder.SetP2ItemHeatsenceMissiles(P2HeatsenceMissilesIndex);
                    UpdatePassword();
                    break;

                case nameof(P2TrilazerGunIndex):
                    _passwordEncoder.SetP2ItemTrilazerGun(P2TrilazerGunIndex);
                    UpdatePassword();
                    break;

                case nameof(P2RefractionLazerIndex):
                    _passwordEncoder.SetP2ItemRefractionLazer(P2RefractionLazerIndex);
                    UpdatePassword();
                    break;

                default:
                    break;
            }

            base.OnPropertyChanged(name);
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdatePassword()
        {
            Password = _passwordEncoder.GetPassword();
        }

        #endregion Private Methods
    }
}