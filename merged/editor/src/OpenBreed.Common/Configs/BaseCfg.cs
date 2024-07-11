using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace OpenBreed.Common.Configs
{
    public abstract class BaseCfg : IConfig
    {
        private IConfigCtrl m_Ctrl = null;

        /// <summary>
        /// Configurations
        /// </summary>
        public IEnumerable<IConfig> Configs
        {
            get
            {
                var type = GetType();
                var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                var res = from property in properties
                          where typeof(IConfig).IsAssignableFrom(property.DeclaringType)
                          select property.GetValue(this,null) as IConfig;
                return res;
            }
        }

        public virtual IConfigCtrl Ctrl
        {
            get
            {
                return m_Ctrl;
            }
        }

        public virtual IConfigCtrl CreateCtrl(Converter<IConfig, IConfigCtrl> factory)
        {
            if (factory == null)
                return null;

            m_Ctrl = factory(this);
            return m_Ctrl;
        }

        public virtual void RestoreCtrlFromCfg()
        {
            Ctrl.OnRestoreCfg(new RestoreCfgEventArgs(this));
        }
    }
}
