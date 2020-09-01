using OpenBreed.Common.UI.WinForms.Controls;
using OpenBreed.Editor.VM.Maps;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class LevelSettingsView : DockContent
    {
        #region Private Fields

        private LevelPropertiesVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public LevelSettingsView()
        {
            InitializeComponent();

            //tbxXBLK.Validating += TextBoxEx_UInt32Validating;
            //tbxYBLK.Validating += TextBoxEx_UInt32Validating;
            //tbxXOFC.Validating += TextBoxEx_UInt32Validating;
            //tbxYOFC.Validating += TextBoxEx_UInt32Validating;
            //tbxXOFM.Validating += TextBoxEx_UInt32Validating;
            //tbxYOFM.Validating += TextBoxEx_UInt32Validating;
            //tbxXOFA.Validating += TextBoxEx_UInt32Validating;
            //tbxYOFA.Validating += TextBoxEx_UInt32Validating;
            //tbxXOFC.Validating += TextBoxEx_UInt32Validating;
            //tbxYOFC.Validating += TextBoxEx_UInt32Validating;
            tbxUNKN1.Validating += TextBoxEx_UInt16Validating;
            tbxUNKN2.Validating += TextBoxEx_UInt16Validating;
            tbxUNKN3.Validating += TextBoxEx_UInt16Validating;
            tbxUNKN4.Validating += TextBoxEx_UInt16Validating;
            tbxTIME.Validating += TextBoxEx_UInt16Validating;
            tbxUNKN6.Validating += TextBoxEx_UInt16Validating;
            tbxUNKN7.Validating += TextBoxEx_UInt16Validating;
            tbxUNKN8.Validating += TextBoxEx_UInt16Validating;
            tbxEXC1.Validating += TextBoxEx_UInt16Validating;
            tbxEXC2.Validating += TextBoxEx_UInt16Validating;
            tbxEXC3.Validating += TextBoxEx_UInt16Validating;
            tbxEXC4.Validating += TextBoxEx_UInt16Validating;
            tbxM1TY.Validating += TextBoxEx_UInt16Validating;
            tbxM1HE.Validating += TextBoxEx_UInt16Validating;
            tbxM1SP.Validating += TextBoxEx_UInt16Validating;
            tbxUNKN16.Validating += TextBoxEx_UInt16Validating;
            tbxUNKN17.Validating += TextBoxEx_UInt16Validating;
            tbxM2TY.Validating += TextBoxEx_UInt16Validating;
            tbxM2HE.Validating += TextBoxEx_UInt16Validating;
            tbxM2SP.Validating += TextBoxEx_UInt16Validating;
            tbxUNKN21.Validating += TextBoxEx_UInt16Validating;
            tbxUNKN22.Validating += TextBoxEx_UInt16Validating;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(LevelPropertiesVM vm)
        {
            _vm = vm;

            PropertiesCtrl.Initialize(_vm);

            _vm.PropertyChanged += _vm_PropertyChanged;

            //_vm.CurrentMapChanged += (s, a) => UpdateViewState();
            //_vm.Modified += (s, a) => UpdateDescription();
            //_vm.Saving += (s, a) => UpdateModelWithView();
            UpdateViewState();

            TabText = _vm.Title;
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.Title):
                    TabText = _vm.Title;
                    break;
                default:
                    break;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void SetNoMapState()
        {
            foreach (Control control in Controls)
                control.Enabled = false;
        }

        private void TextBoxEx_UInt16Validating(object sender, CancelEventArgs e)
        {
            TextBoxEx textBox = sender as TextBoxEx;

            UInt16 numberEntered;

            if (!UInt16.TryParse(textBox.Text, out numberEntered))
            {
                MessageBox.Show("You need to enter an UInt16 number.");
                textBox.Text = textBox.LastValidText;
            }
        }

        private void TextBoxEx_UInt32Validating(object sender, CancelEventArgs e)
        {
            TextBoxEx textBox = sender as TextBoxEx;

            UInt32 numberEntered;

            if (!UInt32.TryParse(textBox.Text, out numberEntered))
            {
                MessageBox.Show("You need to enter an UInt32 number.");
                textBox.Text = textBox.LastValidText;
            }
        }

        //private void UpdateModelWithView()
        //{
        //    MapModel map = _vm.Map.CurrentMap;

        //    map.Mission.UNKN1 = Convert.ToInt32(tbxUNKN1.Text);
        //    map.Mission.UNKN2 = Convert.ToInt32(tbxUNKN2.Text);
        //    map.Mission.UNKN3 = Convert.ToInt32(tbxUNKN3.Text);
        //    map.Mission.UNKN4 = Convert.ToInt32(tbxUNKN4.Text);
        //    map.Mission.TIME = Convert.ToInt32(tbxTIME.Text);
        //    map.Mission.UNKN6 = Convert.ToInt32(tbxUNKN6.Text);
        //    map.Mission.UNKN7 = Convert.ToInt32(tbxUNKN7.Text);
        //    map.Mission.UNKN8 = Convert.ToInt32(tbxUNKN8.Text);
        //    map.Mission.EXC1 = Convert.ToInt32(tbxEXC1.Text);
        //    map.Mission.EXC2 = Convert.ToInt32(tbxEXC2.Text);
        //    map.Mission.EXC3 = Convert.ToInt32(tbxEXC3.Text);
        //    map.Mission.EXC4 = Convert.ToInt32(tbxEXC4.Text);
        //    map.Mission.M1TY = Convert.ToInt32(tbxM1TY.Text);
        //    map.Mission.M1HE = Convert.ToInt32(tbxM1HE.Text);
        //    map.Mission.M1SP = Convert.ToInt32(tbxM1SP.Text);
        //    map.Mission.UNKN16 = Convert.ToInt32(tbxUNKN16.Text);
        //    map.Mission.UNKN17 = Convert.ToInt32(tbxUNKN17.Text);
        //    map.Mission.M2TY = Convert.ToInt32(tbxM2TY.Text);
        //    map.Mission.M2HE = Convert.ToInt32(tbxM2HE.Text);
        //    map.Mission.M2SP = Convert.ToInt32(tbxM2SP.Text);
        //    map.Mission.UNKN21 = Convert.ToInt32(tbxUNKN21.Text);
        //    map.Mission.UNKN22 = Convert.ToInt32(tbxUNKN22.Text);

        //    map.Mission.MTXT = tbxMTXT.Text;
        //    map.Mission.LCTX = tbxLCTX.Text;
        //    map.Mission.NOT1 = tbxNOT1.Text;
        //    map.Mission.NOT2 = tbxNOT2.Text;
        //    map.Mission.NOT3 = tbxNOT3.Text;
        //}

        private void UpdateViewState()
        {
            //if (_vm.Map.CurrentMap == null)
            //    SetNoMapState();
            //else
            //    UpdateViewWithModel();
        }
        //private void UpdateViewWithModel()
        //{
        //    MapModel map = _vm.Map.CurrentMap;

        //    foreach (Control control in Controls)
        //        control.Enabled = true;

        //    tbxUNKN1.Text = map.Mission.UNKN1.ToString();
        //    tbxUNKN2.Text = map.Mission.UNKN2.ToString();
        //    tbxUNKN3.Text = map.Mission.UNKN3.ToString();
        //    tbxUNKN4.Text = map.Mission.UNKN4.ToString();
        //    tbxTIME.Text = map.Mission.TIME.ToString();
        //    tbxUNKN6.Text = map.Mission.UNKN6.ToString();
        //    tbxUNKN7.Text = map.Mission.UNKN7.ToString();
        //    tbxUNKN8.Text = map.Mission.UNKN8.ToString();
        //    tbxEXC1.Text = map.Mission.EXC1.ToString();
        //    tbxEXC2.Text = map.Mission.EXC2.ToString();
        //    tbxEXC3.Text = map.Mission.EXC3.ToString();
        //    tbxEXC4.Text = map.Mission.EXC4.ToString();
        //    tbxM1TY.Text = map.Mission.M1TY.ToString();
        //    tbxM1HE.Text = map.Mission.M1HE.ToString();
        //    tbxM1SP.Text = map.Mission.M1SP.ToString();
        //    tbxUNKN16.Text = map.Mission.UNKN16.ToString();
        //    tbxUNKN17.Text = map.Mission.UNKN17.ToString();
        //    tbxM2TY.Text = map.Mission.M2TY.ToString();
        //    tbxM2HE.Text = map.Mission.M2HE.ToString();
        //    tbxM2SP.Text = map.Mission.M2SP.ToString();
        //    tbxUNKN21.Text = map.Mission.UNKN21.ToString();
        //    tbxUNKN22.Text = map.Mission.UNKN22.ToString();

        //    tbxMTXT.Text = map.Mission.MTXT;
        //    tbxLCTX.Text = map.Mission.LCTX;
        //    tbxNOT1.Text = map.Mission.NOT1;
        //    tbxNOT2.Text = map.Mission.NOT2;
        //    tbxNOT3.Text = map.Mission.NOT3;
        //}

        #endregion Private Methods
    }
}