using OpenBreed.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.UI.WinForms.Controls
{
    public delegate void RestoreCfgEventHandler(object sender, RestoreCfgEventArgs e);

    public class RestoreCfgEventArgs : EventArgs
    {
        public IConfig Cfg { get; set; }

        public RestoreCfgEventArgs(IConfig cfg)
        {
            Cfg = cfg;
        }
    }

    public interface IConfigCtrl
    {
        event RestoreCfgEventHandler RestoreCfg;
        void OnRestoreCfg(RestoreCfgEventArgs e);
        void RestoreFromCfg(IConfig cfg);

        IConfigCtrl FindControlOfType(Type type);
    }
}
