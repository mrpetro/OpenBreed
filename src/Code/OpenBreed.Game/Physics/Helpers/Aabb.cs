using OpenBreed.Game.Physics.Components;

namespace OpenBreed.Game.Physics.Helpers
{
    public struct Aabb
    {
        #region Public Properties

        public float Left { get; set; }
        public float Right { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }

        #endregion Public Properties
    };
}