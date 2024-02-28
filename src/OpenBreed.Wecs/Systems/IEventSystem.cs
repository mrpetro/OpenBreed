using Microsoft.VisualBasic;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems
{
    /// <summary>
    /// System that updates on specified event
    /// </summary>
    /// <typeparam name="TEvent">Event which should occur to trigger the update.</typeparam>
    public interface IEventSystem<TEvent> : ISystem
    {
        /// <summary>
        /// Update system when event occurs
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event data</param>
        void Update(object sender, TEvent e);
    }
}
