using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core
{
    public class WorldJob : IJob
    {
        #region Private Fields

        private World world;

        private string actionName;

        #endregion Private Fields

        #region Public Constructors

        public WorldJob(World world, string actionName)
        {
            this.world = world;
            this.actionName = actionName;
        }

        #endregion Public Constructors

        #region Public Properties

        public Action<IJob> Complete { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Execute()
        {
            switch (actionName)
            {
                case "Pause":
                    world.Paused = true;
                    break;
                case "Unpause":
                    world.Paused = false;
                    break;
                default:
                    break;
            }

            Complete(this);
        }

        public void Update(float dt)
        {
        }

        public void Dispose()
        {

        }

        #endregion Public Methods

    }
}
