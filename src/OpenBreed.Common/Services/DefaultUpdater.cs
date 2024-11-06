using OpenBreed.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OpenBreed.Common.Services
{
    public class DefaultUpdater : IUpdater
    {
        #region Private Fields

        private readonly Timer timer;
        private DateTime lastTime;
        private Action<float> updateCallback;

        #endregion Private Fields

        #region Public Constructors

        public DefaultUpdater(float ups, Action<float> updateCallback)
        {
            if (ups <= 0.0f)
            {
                throw new ArgumentOutOfRangeException(nameof(ups), $"{nameof(ups)} must be greater than zero.");
            }

            this.updateCallback = updateCallback ?? throw new ArgumentNullException(nameof(updateCallback));

            timer = new Timer(1.0f / ups * 1000);
            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Elapsed += Timer_Elapsed;
            lastTime = DateTime.Now;
        }

        #endregion Public Constructors

        #region Private Methods

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var dt = (float)(e.SignalTime - lastTime).TotalMilliseconds / 1000.0f;

            updateCallback.Invoke(dt);

            lastTime = e.SignalTime;

            timer.Start();
        }

        #endregion Private Methods
    }
}