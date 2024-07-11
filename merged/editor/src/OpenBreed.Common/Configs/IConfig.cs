using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Configs
{
    public interface IConfig
    {
        /// <summary>
        /// Gets list of dependend configurations
        /// </summary>
        IEnumerable<IConfig> Configs { get; }

        IConfigCtrl Ctrl { get; }

        IConfigCtrl CreateCtrl(Converter<IConfig, IConfigCtrl> factory);

        void RestoreCtrlFromCfg();
    }
}
