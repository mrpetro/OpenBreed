using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Commands
{
    public class WorldSetPauseCommand : IWorldCommand
    {
        #region Public Fields

        public const string TYPE = "WORLD_PAUSE";

        #endregion Public Fields

        #region Public Constructors

        public WorldSetPauseCommand(int worldId, bool pause)
        {
            WorldId = worldId;
            Pause = pause; 
        }

        #endregion Public Constructors

        #region Public Properties

        public int WorldId { get; }
        public bool Pause { get; }
        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}
