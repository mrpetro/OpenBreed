namespace OpenBreed.Wecs.Core.Components
{
    public interface IFrameComponentTemplate : IComponentTemplate
    {
    }

    [ComponentName("Frame")]
    public class FrameComponent : IEntityComponent
    {
        #region Public Constructors

        public FrameComponent()
        {
            Target = -1;
        }

        /// <summary>
        /// Target frame 
        /// </summary>
        public int Target { get; set; }

        /// <summary>
        /// Current frame
        /// </summary>
        public int Current { get; set; }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties
    }
}