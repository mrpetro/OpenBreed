using OpenBreed.Animation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Animation
{
    public class Animator
    {
        #region Internal Constructors
        public Animator()
        {
        }

        internal Animator(AnimationStateBuilder builder)
        {
            ClipId = builder.ClipId;
            Position = builder.Time;
            Speed = builder.Speed;
            Paused = builder.Paused;
            Loop = builder.Loop;
        }

        #endregion Internal Constructors

        #region Public Properties

        public int ClipId { get; set; }
        public float Position { get; set; }
        public float Speed { get; set; }
        public bool Paused { get; set; }
        public bool Loop { get; set; }

        #endregion Public Properties
    }
}
