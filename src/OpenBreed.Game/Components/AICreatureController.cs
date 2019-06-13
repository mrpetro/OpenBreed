using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Control;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Core.Systems.Movement;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Game.Components
{
    public class AICreatureController : IAutoController
    {
        #region Private Fields

        private List<Vector2> waypoints = new List<Vector2>();

        private StateMachine stateMachine;
        private Position position;

        #endregion Private Fields

        #region Public Constructors

        public AICreatureController()
        {
            Waypoints = new ReadOnlyCollection<Vector2>(waypoints);
        }

        #endregion Public Constructors

        #region Public Properties

        public ReadOnlyCollection<Vector2> Waypoints { get; }
        public Type SystemType { get { return typeof(ControlSystem); } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void SetWaypoint(Vector2 position)
        {
            waypoints.Add(position);
        }

        public void Initialize(IEntity entity)
        {
            stateMachine = entity.Components.OfType<StateMachine>().First();
            position = entity.Components.OfType<Position>().First();
        }

        public void Update(float dt)
        {
            if (waypoints.Any())
            {
                var distanceVec = Vector2.Subtract(waypoints.First(), position.Value);
                var targetVector = distanceVec;

                targetVector.Normalize();

                int compass = (((int)Math.Round(Math.Atan2(targetVector.Y, targetVector.X) / (2 * Math.PI / 8))) + 8) % 8;

                //Console.WriteLine($"Distance Left: {targetVector.Length}");

                //Console.WriteLine($"Move Dir: {targetVector}");

                if (distanceVec.Length < 16.0f)
                {
                    //Force position of entity when it's close enough to waypoint
                    position.Value = waypoints.First();
                    waypoints.RemoveAt(0);
                    return;
                }

                var direction = MovementTools.SnapToCompass8Way(targetVector);
                stateMachine.Perform("Walk", direction);
            }
            else
                stateMachine.Perform("Stop");

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