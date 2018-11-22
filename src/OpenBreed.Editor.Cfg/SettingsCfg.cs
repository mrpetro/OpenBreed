using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenABEdCfg.Options;
using OpenBreed.Editor.Cfg.Layout;

namespace OpenBreed.Editor.Cfg
{
    [Serializable]
    public class SettingsCfg
    {
        public OptionsCfg Options { get; set; }
        public LayoutCfg Layout { get; set; }
    }
}
