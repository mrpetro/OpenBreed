using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Control;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Core.Systems.Control.Systems;
using OpenBreed.Core.Systems.Movement;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Core.Systems.Control.Components
{
    public class AiControl : IEntityComponent
    {
        #region Private Fields

        private Position position;

        #endregion Private Fields

        #region Public Constructors

        public AiControl()
        {
            Waypoints = new List<Vector2>();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<Vector2> Waypoints { get; }
        public Type SystemType { get { return null; } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Initialize(IEntity entity)
        {
            position = entity.Components.OfType<Position>().First();
        }

        #endregion Public Methods

        #region Private Methods

        private Vector2 Snap(Vector2 position, float gridSize)
        {
            position = Vector2.Divide(position, gridSize);
            position.X = (int)position.X * gridSize + gridSize / 2.0f;
            position.Y = (int)position.Y * gridSize + gridSize / 2.0f;
            return position;
        }

        #endregion Private Methods
    }
}