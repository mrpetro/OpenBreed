using OpenBreed.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenBreed.Common.UI.WinForms.Controls
{
    public partial class BaseCtrl : UserControl, IConfigCtrl
    {
        public event RestoreCfgEventHandler RestoreCfg;

        #region Constructors


        /// <summary>
        /// Generic constructor
        /// </summary>
        public BaseCtrl()
        {
            InitializeComponent();
        }

        #endregion

        public IConfigCtrl FindControlOfType(Type type)
        {
            //Get ConfigCtrl Root
            IConfigCtrl parent = GetConfigCtrlRoot(this);

            if (parent == null)
                return null;

            //Collect all IConfigCtrls from Root and it's childs and their childs, etc.
            var configCtrls = GetConfigCtrlChilds(parent as Control);

            foreach (IConfigCtrl control in configCtrls)
            {
                if (control.GetType() == type)
                    return control;
            }

            return null;
        }

        public static List<IConfigCtrl> GetConfigCtrlChilds(Control control)
        {
            List<IConfigCtrl> configCtrlList = new List<IConfigCtrl>();

            var configCtrls = control.Controls.OfType<IConfigCtrl>();

            configCtrlList.AddRange(configCtrls);

            foreach (Control subControl in control.Controls)
            {
                List<IConfigCtrl> subConfigCtrlList = GetConfigCtrlChilds(subControl);
                configCtrlList.AddRange(subConfigCtrlList);
            }

            return configCtrlList;
        }

        public static IConfigCtrl GetConfigCtrlRoot(Control control)
        {
            //Find first IConfigCtrl parent in hierarchy
            IConfigCtrl root = control as IConfigCtrl;

            Control parent = control.Parent;
            while (parent != null)
            {
                if (parent is IConfigCtrl)
                    root = parent as IConfigCtrl;
                parent = parent.Parent;
            }

            return root;
        }

        public virtual void RestoreFromCfg(IConfig cfg)
        {

        }

        public virtual void OnRestoreCfg(RestoreCfgEventArgs e)
        {
            SuspendLayout();

            foreach (var config in e.Cfg.Configs)
            {
                if (config == null)
                    continue;

                config.RestoreCtrlFromCfg();
            }

            ResumeLayout();

            if (RestoreCfg != null) RestoreCfg(this, e);
        }
    }

}
