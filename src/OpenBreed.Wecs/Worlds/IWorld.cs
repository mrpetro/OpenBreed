using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Worlds
{



    /// <summary>
    /// World interface
    ///
    public interface IWorld
    {
        #region Public Properties

        /// <summary>
        /// Time "speed" control value, can't be negative but can be 0 (Basicaly stops time).
        /// </summary>
        float DtMultiplier { get; set; }

        IEnumerable<IEntity> Entities { get; }

        /// <summary>
        /// Id of this world
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Name of this world
        /// </summary>
        string Name { get; }

        ISystem[] Systems { get; }

        #endregion Public Properties

        #region Public Methods

        TModule GetModule<TModule>();

        T GetSystem<T>() where T : ISystem;

        #endregion Public Methods
    }
}