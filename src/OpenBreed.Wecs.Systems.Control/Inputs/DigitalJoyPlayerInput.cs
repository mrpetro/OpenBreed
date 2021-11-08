using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Components.Control;
using System;

namespace OpenBreed.Wecs.Systems.Control.Inputs
{
    public class DigitalJoyPlayerInput : IPlayerInput
    {
        #region Public Properties

        public bool Changed => AxisX != OldAxisX || AxisY != OldAxisY;


        public float OldAxisX { get; set; }
        public float OldAxisY { get; set; }
        public float AxisX { get; set; }
        public float AxisY { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Reset(IPlayer player)
        {
            OldAxisX = AxisX;
            OldAxisY = AxisY;
            AxisX = 0.0f;
            AxisY = 0.0f;
        }

        #endregion Public Methods
    }
}