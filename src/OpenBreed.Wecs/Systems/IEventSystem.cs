using Microsoft.VisualBasic;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems
{
    public interface IEventSystem<TEvent>
    {
        /// <summary>
        /// Update all entities in this system when event occurs
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event data</param>
        void Update(object sender, TEvent e);
    }
}
