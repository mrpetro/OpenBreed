namespace OpenBreed.Wecs.Abstractions.Systems
{
    /// <summary>
    /// Event system base interface
    /// </summary>
    public interface IEventSystem : ISystem
    {
    }

    /// <summary>
    /// System that updates on specified event
    /// </summary>
    /// <typeparam name="TEvent">Event which should occur to trigger the update.</typeparam>
    public interface IEventSystem<TEvent> : IEventSystem
    {
        /// <summary>
        /// Update system when event occurs
        /// </summary>
        /// <param name="world">World to notify.</param>
        /// <param name="e">Event data.</param>
        void OnEvent(IWorld world, TEvent e);
    }
}
