using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Systems
{
    public interface ISystem
    {
        /// <summary>
        /// Perform cleanup of entites and their components related with this system
        /// </summary>
        void Cleanup();

        /// <summary>
        /// Deinitialize the system when world is destroyed
        /// </summary>
        void Deinitialize();

        bool Matches(Entity entity);

        void AddEntity(Entity entity);

        void RemoveEntity(Entity entity);

        /// <summary>
        /// Handle given command
        /// </summary>
        /// <param name="sender">Object is sending the command</param>
        /// <param name="cmd">Command to recieve</param>
        /// <returns>True if command was handled, false otherwise</returns>
        bool ExecuteCommand(object sender, ICommand cmd);
    }
}
